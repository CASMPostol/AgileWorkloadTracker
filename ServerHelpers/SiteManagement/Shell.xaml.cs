//<summary>
//  Title   : Shell
//  System  : Microsoft VisualStudio 2013 / C#
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.Common.Interactivity.InteractionRequest;
//using CAS.SharePoint.Logging;
using System.ComponentModel.Composition;
using System.Windows;

namespace CAS.AgileWorkloadTracker.SiteManagement
{
  /// <summary>
  /// Interaction logic for Shell.xaml
  /// </summary>
  [Export]
  public partial class Shell : Window
  {

    #region constructor
    /// <summary>
    /// Initializes a new instance of the <see cref="Shell"/> class.
    /// </summary>
    public Shell()
    {
      InitializeComponent();
    }
    #endregion

    /// <summary>
    /// Sets or gets the ViewModel <see cref="ShellViewModel"/>.
    /// </summary>
    /// <remarks>
    /// This set-only property is annotated with the <see cref="ImportAttribute"/> so it is injected by MEF with
    /// the appropriate view model.
    /// </remarks>
    [Import]
    public ShellViewModel ViewModel
    {
      set
      {
        //It is assigned twice but the handles must not be assigned more then once. 
        if (this.DataContext != null)
          return;
        this.DataContext = value;
        value.CloseWindow.Raised += CloseWindow_Raised;
        value.CancelConfirmation.Raised += CancelConfirmation_Raised;
        value.ExceptionNotification.Raised += ExceptionNotification_Raised;
      }
      get { return (ShellViewModel)this.DataContext; }
    }

    private void Window_Loaded(object sender, RoutedEventArgs e) { }
    private void ExceptionNotification_Raised(object sender, InteractionRequestedEventArgs<INotification> e)
    {
      MessageBox.Show((string)e.Context.Content, e.Context.Title, MessageBoxButton.OK, MessageBoxImage.Error);
      e.Callback();
    }
    private void CancelConfirmation_Raised(object sender, InteractionRequestedEventArgs<IConfirmation> e)
    {
      if (MessageBox.Show((string)e.Context.Content, e.Context.Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
        e.Context.Confirmed = true;
      else
        e.Context.Confirmed = false;
      e.Callback();
    }
    private void CloseWindow_Raised(object sender, InteractionRequestedEventArgs<INotification> e)
    {
      e.Callback();
      this.Close();
    }
    private void HelpButton_Click(object sender, RoutedEventArgs e)
    {
      var newWindow = new HelpWindow();
      newWindow.Show();
    }
    protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
    {
      Properties.Settings.Default.Save();
      ViewModel.Dispose();
      Services.NamedTraceLogger.Logger.TraceEvent(System.Diagnostics.TraceEventType.Stop, 91, "The application is closing");
      Services.NamedTraceLogger.Logger.Flush();
      Services.NamedTraceLogger.Logger.Dispose();
      base.OnClosing(e);
    }

  }
}
