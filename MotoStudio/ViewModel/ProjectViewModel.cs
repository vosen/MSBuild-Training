using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MotoStudio.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MotoStudio.ViewModel
{
    class ProjectViewModel : ObservableObject
    {
        private Project proj;

        public RelayCommand Save { get; private set; }
        public RelayCommand Start { get; private set; }
        public RelayCommand Clean { get; private set; }
        public RelayCommand Build { get; private set; }

        private string output;
        public string Output
        {
            get { return output; }
            set { Set(ref output, value, "Output"); }
        }

        public ProjectViewModel(Project proj)
        {
            this.proj = proj;
            Save = new RelayCommand(() => {});
            Start = new RelayCommand(() => {}, CanBuild);
            Clean = new RelayCommand(OnClean, CanBuild);
            Build = new RelayCommand(OnBuild, CanBuild);
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
                Start.RaiseCanExecuteChanged();
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
    }
}
