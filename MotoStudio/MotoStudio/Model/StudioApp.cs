using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using MsProject = Microsoft.Build.Evaluation.Project;
using Microsoft.Win32;

namespace MotoStudio.Model
{
    public class StudioApp
    {
        public Project Project { get; private set; }

        private StudioApp(Project proj)
        {
            Project = proj;
        }

        public static StudioApp Create(
          Func<string> getProjectPath,
          Func<string, string, bool> errHandler)
        {
            while (true)
            {
                string path = getProjectPath();
                Project proj;
                bool? result = LoadProject(path, errHandler, out proj);
                if (!result.HasValue)
                    return null;
                if (result.Value)
                    return new StudioApp(proj);
            }
        }

        public static StudioApp CreateDefault()
        {
            return Create(GetProjectPath, HandleError);
        }

        // true: project loaded, proceed
        // false: project not loaded, retry
        // null: error and quit, quit
        private static bool? LoadProject(string path, Func<string, string, bool> errHandler, out Project proj)
        {
            try
            {
                proj = new Project(new MsProject(path));
            }
            catch (Exception ex)
            {
                proj = null;
                return errHandler(path, ex.Message) ? (bool?)false : null;
            }
            return true;
        }

        public static string GetProjectPath()
        {
            var dialog = new OpenFileDialog()
            {
                Filter = "C# project files (*.csproj)|*.csproj",
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false
            };
            bool? result = dialog.ShowDialog();
            if (!result.HasValue || !result.Value)
                return null;
            return dialog.FileName;
        }

        public static bool HandleError(string path, string msg)
        {
            MessageBoxResult result = MessageBox.Show(String.Format("Could not load {0}:\n{1}", path, msg), "Error", MessageBoxButton.OKCancel);
            return result == MessageBoxResult.OK;
        }
    }
}
