using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MotoStudio.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MotoStudio.ViewModel
{
    class ProjectViewModel : ObservableObject
    {
        private Project proj;

        public RelayCommand Save { get; private set; }
        public RelayCommand Clean { get; private set; }
        public RelayCommand Build { get; private set; }

        public Project Root { get { return proj; } }
        public IList<ITreeItem> Items { get { return proj.Items; } }
        public string Name { get { return proj.Name; } }

        private ITreeItem selectedItem;
        public ITreeItem SelectedItem
        {
            get { return selectedItem; }
            set 
            { 
                if(Set(ref selectedItem, value, "SelectedItem"))
                {
                    if(!value.IsFolder)
                    {
                        bufferPath = null;
                        buffer = null;
                        RaisePropertyChanged("SelectedBuffer");
                    }
                    RaisePropertyChanged("CanSave");
                }
            }
        }

        private string bufferPath;
        private string buffer;
        public string SelectedBuffer
        {
            get
            {
                
                if(buffer == null && SelectedItem != null)
                {
                    bufferPath = GetFullPath(SelectedItem, true);
                    buffer = File.ReadAllText(bufferPath);
                }
                return buffer;
            }
            set
            {
                buffer = value;
            }
        }

        private string output;
        public string Output
        {
            get { return output; }
            set { Set(ref output, value, "Output"); }
        }

        public ProjectViewModel(Project proj)
        {
            this.proj = proj;
            Save = new RelayCommand(OnSave, CanSave);
            Clean = new RelayCommand(OnClean, CanBuild);
            Build = new RelayCommand(OnBuild, CanBuild);
        }

        private void OnSave()
        {
            File.WriteAllText(bufferPath, buffer);
        }

        private bool CanSave()
        {
            return SelectedItem != null && !SelectedItem.IsFolder;
        }

        private volatile bool isBuilding;

        private bool CanBuild()
        {
            return !isBuilding;
        }

        private void SignalBuildCommandsChange()
        {
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                Clean.RaiseCanExecuteChanged();
                Build.RaiseCanExecuteChanged();
            });
        }
        private void LockBuild()
        {
            Output = "";
            isBuilding = true;
            SignalBuildCommandsChange();
        }

        private void UnlockBuild()
        {
            isBuilding = false;
            SignalBuildCommandsChange();
        }

        private void OnClean()
        {
            Task.Run(() => 
            {
                LockBuild();
                var result = proj.Clean();
                UnlockBuild();
                Output = result.Output;
            });
        }

        private void OnBuild()
        {
            Task.Run(() => 
            {
                LockBuild();
                var result = proj.Build();
                UnlockBuild();
                Output = result.Output;
            });
        }

        private static string GetFullPath(ITreeItem item, bool first)
        {
            Project root = item as Project;
            if(root != null)
                return  first ? root.MsProject.FullPath : root.MsProject.DirectoryPath;
            return Path.Combine(GetFullPath(item.Parent, false), item.Name);
        }
    }
}
