using System;
using System.Windows;

namespace CAS.AgileWorkloadTracker.SiteManagement
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
      x_MainGrid.DataContext = new MainWindowData();
    }

    private void x_RefreshButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      try
      {
        MainWindowData _mw = x_MainGrid.DataContext as MainWindowData;
        _mw.GetMilestoneCollection();
      }
      catch (Exception ex)
      {
        ShowExceptionBox(ex);
      }
    }
    private void ShowExceptionBox(Exception exception)
    {
      string _msg = String.Format("The operation has been aborted by ex: {0}", exception.Message);
      MessageBox.Show( _msg, "Exception catched.", MessageBoxButton.OK, MessageBoxImage.Error);
    }
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      MainWindowData _mw = x_MainGrid.DataContext as MainWindowData;
      if (_mw == null)
        return;
      _mw.Dispose();
    }

   }
}
