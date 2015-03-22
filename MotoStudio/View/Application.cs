using MotoStudio.Model;
using MotoStudio.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MotoStudio.View
{
    static class App
    {
        [STAThread]
        public static void Main()
        {
            var app = StudioApp.CreateDefault();
            var window = new MainWindow();
            window.DataContext = new ProjectViewModel(app.Project);
            new Application().Run(window);
        }
    }
}
