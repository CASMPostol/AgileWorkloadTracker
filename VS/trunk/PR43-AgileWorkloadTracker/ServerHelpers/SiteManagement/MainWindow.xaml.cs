//<summary>
//  Title   : class MainWindow
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.AgileWorkloadTracker.DataModel110.Linq;
using System;
using System.Collections.ObjectModel;
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
      MainWindowData _dta = new MainWindowData();
      x_MainGrid.DataContext = _dta;
    }
    private MainWindowData MainWindowData
    {
      get { return x_MainGrid.DataContext as MainWindowData; }
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

    private void BackgroundWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Error != null)
      {
        ShowExceptionBox(e.Error);
        return;
      }
      this.MainWindowData.MilestoneCollection = e.Result as ObservableCollection<IMilestoneWrapper>;
      x_MilestonesComboBox.SelectedIndex = 0;
    }

    private void GetRequirementsCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Error != null)
      {
        ShowExceptionBox(e.Error);
        return;
      }
      //TODO
      //this.MainWindowData.MilestoneCollection = e.Result as ObservableCollection<MilestoneWrapper>;
      //x_MilestonesComboBox.SelectedIndex = 0;
    }

    #region Buttons event handlers
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
    private void x_MakeInactiveButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      try
      {
        MainWindowData.Update((IMilestoneWrapper)x_MilestonesComboBox.SelectedItem, BackgroundWorkerCompleted);
      }
      catch (Exception _ex)
      {
        ShowExceptionBox(_ex);
      }
    }
    private void x_ForceMakeInactive_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      try
      {
        if (x_MilestonesComboBox.SelectedIndex == X_TargetMilestoneCombo.SelectedIndex)
        {
          string _msg = "Target milestone cannot be the same as the source.";
          MessageBox.Show(_msg, "Wrong target milestone.", MessageBoxButton.OK, MessageBoxImage.Error);
          return;
        }
        this.MainWindowData.ForceMakeInactive((IMilestoneWrapper)x_MilestonesComboBox.SelectedItem, (IMilestoneWrapper)X_TargetMilestoneCombo.SelectedItem, BackgroundWorkerCompleted);
      }
      catch (Exception _ex)
      {
        ShowExceptionBox(_ex);
      }
    }
    private void x_EditRequirementsButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      try
      {
        MainWindowData.GetRequirements((IMilestoneWrapper)x_MilestonesComboBox.SelectedItem, GetRequirementsCompleted);
      }
      catch (Exception _ex)
      {
        ShowExceptionBox(_ex);
      }
    }
    private void x_MoveButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      try
      {
        if (x_MilestonesComboBox.SelectedIndex == X_TargetMilestoneCombo.SelectedIndex)
        {
          string _msg = "Target milestone cannot be the same as the source.";
          MessageBox.Show(_msg, "Wrong target milestone.", MessageBoxButton.OK, MessageBoxImage.Error);
          return;
        }
        this.MainWindowData.Move((IMilestoneWrapper)x_MilestonesComboBox.SelectedItem, (IMilestoneWrapper)X_TargetMilestoneCombo.SelectedItem, BackgroundWorkerCompleted);
      }
      catch (Exception _ex)
      {
        ShowExceptionBox(_ex);
      }
    }
    #endregion

  }
}
