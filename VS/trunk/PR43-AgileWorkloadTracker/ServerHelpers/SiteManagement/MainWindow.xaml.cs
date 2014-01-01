﻿//<summary>
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
using CAS.AgileWorkloadTracker.SiteManagement.Linq;

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
      MainWindowData _dta = new MainWindowData();
      x_MainGrid.DataContext = _dta;
    }
    private void x_RefreshButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      try
      {
        MainWindowData.Connect(BackgroundWorkerCompleted);
      }
      catch (Exception ex)
      {
        ShowExceptionBox(ex);
      }
    }
    private MainWindowData MainWindowData
    {
      get { return x_MainGrid.DataContext as MainWindowData; }
    }
    private void BackgroundWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Error != null)
        ShowExceptionBox(e.Error);
      else
        x_MilestonesComboBox.SelectedIndex = 1;
    }
    private void ShowExceptionBox(Exception exception)
    {
      string _msg = String.Format("The operation has been aborted by ex: {0}", exception.Message);
      MessageBox.Show(_msg, "Exception catched.", MessageBoxButton.OK, MessageBoxImage.Error);
    }
    private void Window_Closing(object sender, CancelEventArgs e)
    {
      if (DataContext == null)
        return;
      if (!MainWindowData.Connected)
        MainWindowData.Dispose();
      else
      {
        MainWindowData.Disconnect(RunWorkerCompletedDoDispose);
        e.Cancel = true;
      }
    }
    private void RunWorkerCompletedDoDispose(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Error != null)
        ShowExceptionBox(e.Error);
      this.Close();
    }
    private void x_MakeInactiveButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      MainWindowData.MakeInactive((MilestoneWrapper)x_MilestonesComboBox.SelectedItem, BackgroundWorkerCompleted);
    }

	}
}
