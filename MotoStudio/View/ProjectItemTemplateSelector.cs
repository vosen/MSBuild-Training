using MotoStudio.Model;
using MotoStudio.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MotoStudio.View
{
    class ProjectItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Project { get; set; }
        public DataTemplate Folder { get; set; }
        public DataTemplate File { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var root = item as ProjectViewModel;
            if(root != null)
                return Project;
            if(!((ITreeItem)item).IsFile)
                return Folder;
            return File;
        }
    }
}
