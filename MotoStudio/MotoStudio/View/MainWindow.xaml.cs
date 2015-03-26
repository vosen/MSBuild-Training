using MotoStudio.Model;
using MotoStudio.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MotoStudio.View
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
    }

    private void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        ITreeItem model;
        ProjectViewModel root = e.NewValue as ProjectViewModel;
        if(root != null)
            model = root.Root;
        else
            model = (ITreeItem)e.NewValue;
        ((ProjectViewModel)this.DataContext).SelectedItem = model;
    }

    private void SaveExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        if(this.DataContext == null)
            return;
        CodeBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
        ((ProjectViewModel)this.DataContext).Save.Execute(e.Parameter);
    }

    private void SaveCanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        if(this.DataContext == null)
            return;
        e.CanExecute = ((ProjectViewModel)this.DataContext).Save.CanExecute(e.Parameter);
    }
  }
}
