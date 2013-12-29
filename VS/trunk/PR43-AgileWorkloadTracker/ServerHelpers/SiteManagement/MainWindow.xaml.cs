//<summary>
//  Title   : class MainWindow
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL:$
//  $Id$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>
using System;
using System.ComponentModel;
using System.Windows;

namespace CAS.AgileWorkloadTracker.SiteManagement
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
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
        x_RefreshButton.IsEnabled = false;
        _mw.Connect(ConnectBackgroundWorkerCompleted);
      }
      catch (Exception ex)
      {
        ShowExceptionBox(ex);
      }
    }
    private void ConnectBackgroundWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Error != null)
        ShowExceptionBox(e.Error);
      x_RefreshButton.IsEnabled = true;
    }
    private void ShowExceptionBox(Exception exception)
    {
      string _msg = String.Format("The operation has been aborted by ex: {0}", exception.Message);
      MessageBox.Show(_msg, "Exception catched.", MessageBoxButton.OK, MessageBoxImage.Error);
    }
    private void Window_Closing(object sender, CancelEventArgs e)
    {
      MainWindowData _mw = x_MainGrid.DataContext as MainWindowData;
      if (_mw == null)
        return;
      if (!_mw.Connected)
        _mw.Dispose();
      else
      {
        _mw.Disconnect(RunWorkerCompletedDoDispose);
        e.Cancel = true;
      }
    }
    private void RunWorkerCompletedDoDispose(object sender, RunWorkerCompletedEventArgs e)
    {
      this.Close();
    }

  }
}
