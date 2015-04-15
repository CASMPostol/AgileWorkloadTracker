//<summary>
//  Title   : SetupDataDialogMachine
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

using CAS.Common.ViewModel.Wizard;
using CAS.Common.ViewModel.Wizard.ButtonsPanelStateTemplates;
using CAS.SmartFactory.Shepherd.Client.DataManagement.Linq2SQL;
using CAS.SmartFactory.Shepherd.Client.Management.Properties;
using Microsoft.Practices.Prism.Regions;
using System;
using System.ComponentModel;
using System.Data.SqlClient;
using NsSPLinq = CAS.SmartFactory.Shepherd.Client.DataManagement.Linq;


/// <summary>
/// The StateMachines namespace.
/// </summary>
namespace CAS.SmartFactory.Shepherd.Client.Management.StateMachines
{
  /// <summary>
  /// Class SetupDataDialogMachine - setup settings state of the machine state <see cref="BackgroundWorkerMachine{StateMachineType, ViewModelType}"/>
  /// </summary>
  public abstract class SetupDataDialogMachine : BackgroundWorkerMachine<ShellViewModel, Controls.SettingsPanelViewModel>
  {

    #region creator
    /// <summary>
    /// Initializes a new instance of the <see cref="SetupDataDialogMachine"/> class.
    /// </summary>
    public SetupDataDialogMachine()
    {
      m_ButtonsTemplate = new ConnectCancelTemplate(Resources.RouteEditButtonTitle, Resources.ArchiveButtonTitle);
      m_StateMachineActionsArray = new Action<object>[4];
      m_StateMachineActionsArray[(int)m_ButtonsTemplate.ConnectPosition] = x => this.OnConnectCommand();
      m_StateMachineActionsArray[(int)m_ButtonsTemplate.CancelPosition] = x => this.Cancel();
      m_StateMachineActionsArray[(int)m_ButtonsTemplate.LeftButtonPosition] = x => this.OnRouteEditCommand(m_ConnectionData);
      m_StateMachineActionsArray[(int)m_ButtonsTemplate.LeftMiddleButtonPosition] = x => this.OnArchiveCommand(m_ConnectionData, m_ConnectionData);
    }
    #endregion

    #region BackgroundWorkerMachine
    /// <summary>
    /// Gets the state machine actions array.
    /// </summary>
    /// <value>The current state machine actions array.</value>
    public override Action<object>[] StateMachineActionsArray
    {
      get { return m_StateMachineActionsArray; }
    }
    /// <summary>
    /// Called on entering new state.
    /// </summary>
    public override void OnEnteringState()
    {
      base.OnEnteringState();
      Context.EnabledEvents = StateMachineEvents.RightButtonEvent | StateMachineEvents.RightMiddleButtonEvent;
    }
    /// <summary>
    /// Called when exception has occurred. Make context aware about exception.
    /// </summary>
    /// <param name="exception">The exception.</param>
    public override void OnException(Exception exception)
    {
      base.OnException(exception);
      //TODO Improve usage of teh mask bits. 
      Context.EnabledEvents = StateMachineEvents.RightButtonEvent | StateMachineEvents.RightMiddleButtonEvent;
    }
    /// <summary>
    /// Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
    public override string ToString()
    {
      return Properties.Resources.SetupDataDialogMachineName;
    }
    /// <summary>
    /// Handles the DoWork event of the BackgroundWorker control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="DoWorkEventArgs" /> instance containing the event data.</param>
    /// <exception cref="System.NotImplementedException"></exception>
    protected override void BackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
    {
      e.Cancel = false;
      e.Result = null;
      ConnectionData _cd = (ConnectionData)e.Argument;
      GetSQLContentState(_cd, args => ReportProgress(this, args));
      _cd.SPConnected = NsSPLinq.Connectivity.TestConnection(_cd.SharePointWebsiteURL, x => ReportProgress(this, x));
      e.Result = _cd;
    }
    /// <summary>
    /// Called when worker task has been completed.
    /// </summary>
    /// <param name="result">An object that represents the result of an asynchronous operation.</param>
    /// <exception cref="System.NotImplementedException"></exception>
    protected override void RunWorkerCompleted(object result)
    {
      m_ConnectionData = (ConnectionData)result;
      Context.EnabledEvents = m_ButtonsTemplate.SetEventsMask(m_ConnectionData.SPConnected, m_ConnectionData.SPConnected && m_ConnectionData.SQLConnected);
      if (m_ConnectionData.SPConnected)
        this.PublishSPURL();
      if (m_ConnectionData.SQLConnected)
      {
        ProgressChang(String.Format("Last cleanup run at {0} by {1}", m_ConnectionData.CleanupLastRunDate.LocalizedString(), m_ConnectionData.CleanupLastRunBy.GetValueOrDefault(Settings.Default.RunByUnknown)));
        ProgressChang(String.Format("Last synchronization run at {0} by {1}", m_ConnectionData.SyncLastRunDate.LocalizedString(), m_ConnectionData.SyncLastRunBy.GetValueOrDefault(Settings.Default.RunByUnknown)));
        ProgressChang(String.Format("Last Archiving run at {0} by {1}", m_ConnectionData.ArchivingLastRunDate.LocalizedString(), m_ConnectionData.ArchivingLastRunBy.GetValueOrDefault(Settings.Default.RunByUnknown)));
      }
      Context.ProgressChang(this, new ProgressChangedEventArgs(0, "Operation Connect finished."));
    }
    private void ProgressChang(string message)
    {
      Context.ProgressChang(this, new ProgressChangedEventArgs(1, message));
    }
    /// <summary>
    ///  Called when only cancel button must be active - after starting background worker.
    /// </summary>
    protected override void OnlyCancelActive()
    {
      Context.EnabledEvents = m_ButtonsTemplate.OnlyCancel();
    }
    /// <summary>
    /// Gets the state of the buttons panel.
    /// </summary>
    /// <value>The state of the buttons panel.</value>
    protected override ButtonsSetState ButtonsPanelState
    {
      get { return m_ButtonsTemplate; }
    }
    public override void Cancel()
    {
      Context.ProgressChang(this, new ProgressChangedEventArgs(1, "Cancellation pending"));
      base.Cancel();
    }
    #endregion

    #region abstract
    /// <summary>
    /// Gets the get connection data.
    /// </summary>
    /// <value>The get connection data.</value>
    protected abstract ConnectionDescription GetConnectionDescription { get; }
    /// <summary>
    /// Publishes the SharePoint site URL.
    /// </summary>
    /// <param name="URL">The URL of the website.</param>
    internal protected abstract void PublishSPURL();
    #endregion

    #region private
    /// <summary>
    /// Class ConnectionData contains recent information gathered during connection to the SharePoint and SQL database. 
    /// It is singleton to provide a common reference point.
    /// </summary>
    public class ConnectionData : ISQLContentState, ISPContentState
    {
      #region creator
      /// <summary>
      /// Creates the instance.
      /// </summary>
      /// <param name="ConnectionDescription">The connection description.</param>
      internal ConnectionData(ConnectionDescription ConnectionDescription) :
        base()
      {
        SQLConnectionString = GetConnectionString(ConnectionDescription.SQLServer, ConnectionDescription.DatabaseName);
        SharePointWebsiteURL = ConnectionDescription.SharePointServerURL;
      }
      #endregion

      #region SQLContentState
      /// <summary>
      /// Gets or sets the cleanup last run by.
      /// </summary>
      /// <value>The user name.</value>
      public string CleanupLastRunBy { get; set; }
      /// <summary>
      /// Gets or sets the synchronization last run by.
      /// </summary>
      /// <value>The user name.</value>
      public string SyncLastRunBy { get; set; }
      /// <summary>
      /// Gets or sets the archiving last run by.
      /// </summary>
      /// <value>The user name.</value>
      public string ArchivingLastRunBy { get; set; }
      /// <summary>
      /// Gets or sets the cleanup last run date.
      /// </summary>
      /// <value>The last cleanup <see cref="DateTime" />.</value>
      public System.DateTime? CleanupLastRunDate { get; set; }
      /// <summary>
      /// Gets or sets the synchronize last run date.
      /// </summary>
      /// <value>The las synchronize <see cref="DateTime" />.</value>
      public System.DateTime? SyncLastRunDate { get; set; }
      /// <summary>
      /// Gets or sets the archiving last run date.
      /// </summary>
      /// <value>The last archiving <see cref="DateTime" />.</value>
      public System.DateTime? ArchivingLastRunDate { get; set; }
      /// <summary>
      /// Gets or sets the SQL connection string.
      /// </summary>
      /// <value>The SQL connection string.</value>
      public string SQLConnectionString { get; set; }
      /// <summary>
      /// Gets or sets a value indicating whether the SQL database can be accessed using the <see cref="ISQLContentState.SQLConnectionString" />.
      /// </summary>
      /// <value><c>true</c> if SQL successfully connected and gathered all required data about the content state; otherwise, <c>false</c>.</value>
      public bool SQLConnected { get; set; }
      #endregion

      #region ISPContentState
      public string SharePointWebsiteURL
      {
        get;
        private set;
      }
      public bool SPConnected { get; set; }
      #endregion

      #region private
      /// <summary>
      /// Prevents a default instance of the <see cref="ConnectionData"/> class from being created.
      /// </summary>
      private ConnectionData()
      {
        CleanupLastRunBy = Properties.Settings.Default.RunByError;
        SyncLastRunBy = Properties.Settings.Default.RunByError;
        ArchivingLastRunBy = Properties.Settings.Default.RunByError;
        CleanupLastRunDate = new Nullable<DateTime>();
        SyncLastRunDate = new Nullable<DateTime>();
        ArchivingLastRunDate = new Nullable<DateTime>();
        SQLConnectionString = String.Empty;
        SQLConnected = false;
      }
      private static string GetConnectionString(string sqlServer, string databaseName)
      {
        return String.Format(Properties.Settings.Default.ConnectionString, sqlServer, databaseName);
      }
      #endregion

    }

    private void OnConnectCommand()
    {
      m_ConnectionData = null;
      this.RunAsync(new ConnectionData(GetConnectionDescription));
    }
    private void OnArchiveCommand(ISPContentState spContentState, ISQLContentState sqlContentState)
    {
      NavigationParameters _par = new NavigationParameters();
      _par.Add(typeof(ISPContentState).Name, spContentState);
      _par.Add(typeof(ISQLContentState).Name, sqlContentState);
      Context.RequestNavigate(Infrastructure.ViewNames.ArchivalStateName, _par);
    }
    private void OnRouteEditCommand(ISPContentState spContentState)
    {
      NavigationParameters _par = new NavigationParameters();
      _par.Add(typeof(ISPContentState).Name, spContentState);
      Context.RequestNavigate(Infrastructure.ViewNames.RouteEditorStateName, _par);
    }
    private ConnectionData m_ConnectionData = null;
    private static void GetLastOperation(SHRARCHIVE entities, CAS.SharePoint.Client.Link2SQL.ArchivingOperationLogs.OperationName operationName, Action<string> RunBy, Action<DateTime?> RunDate)
    {
      ArchivingOperationLogs _recentActions = CAS.SharePoint.Client.Link2SQL.ArchivingOperationLogs.GetRecentActions<ArchivingOperationLogs>(entities, operationName);
      if (_recentActions != null)
      {
        RunBy(_recentActions.UserName);
        RunDate(_recentActions.Date);
      }
      else
      {
        RunBy(String.Empty);
        RunDate(new Nullable<DateTime>());
      }
    }
    private ConnectCancelTemplate m_ButtonsTemplate = null;
    private Action<object>[] m_StateMachineActionsArray;
    private static void GetSQLContentState(ISQLContentState _cd, Action<ProgressChangedEventArgs> reportProgress)
    {
      try
      {
        reportProgress(new ProgressChangedEventArgs(1, String.Format("Connection string {0}", _cd.SQLConnectionString)));
        System.Data.IDbConnection _connection = new SqlConnection(_cd.SQLConnectionString);
        using (SHRARCHIVE _entities = new SHRARCHIVE(_connection))
          if (_entities.DatabaseExists())
          {
            reportProgress(new ProgressChangedEventArgs(1, "The specified database exists."));
            GetLastOperation(_entities, CAS.SharePoint.Client.Link2SQL.ArchivingOperationLogs.OperationName.Cleanup, x => _cd.CleanupLastRunBy = x, y => _cd.CleanupLastRunDate = y);
            GetLastOperation(_entities, CAS.SharePoint.Client.Link2SQL.ArchivingOperationLogs.OperationName.Synchronization, x => _cd.SyncLastRunBy = x, y => _cd.SyncLastRunDate = y);
            GetLastOperation(_entities, CAS.SharePoint.Client.Link2SQL.ArchivingOperationLogs.OperationName.Archiving, x => _cd.ArchivingLastRunBy = x, y => _cd.ArchivingLastRunDate = y);
            _cd.SQLConnected = true;
          }
          else
            reportProgress(new ProgressChangedEventArgs(1, "The specified database cannot be opened."));
      }
      catch (Exception _ex)
      {
        reportProgress(new ProgressChangedEventArgs(1, String.Format("Testing SQL connection has been aborted by the exception: {0}", _ex.Message)));
      }
    }
    #endregion

  }
}
