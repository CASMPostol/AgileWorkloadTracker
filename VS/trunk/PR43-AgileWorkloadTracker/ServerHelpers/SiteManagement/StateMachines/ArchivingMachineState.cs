//<summary>
//  Title   : ArchivingMachineState
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
using CAS.SmartFactory.Shepherd.Client.DataManagement;
using CAS.SmartFactory.Shepherd.Client.Management.Properties;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Regions;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace CAS.SmartFactory.Shepherd.Client.Management.StateMachines
{
  public abstract class ArchivingMachineState<ViewModelContextType> : BackgroundWorkerMachine<ShellViewModel, ViewModelContextType>, ILoggerFacade
    where ViewModelContextType : IViewModelContext
  {

    /// <summary>
    /// Initializes a new instance of the <see cref="ArchivingMachineState{ViewModelContextType}"/> class.
    /// </summary>
    internal ArchivingMachineState()
    {
      m_ButtonsTemplate = new CancelTemplate(String.Empty, Resources.ArchiveButtonTitle, Resources.SetupButtonTitle);
      m_StateMachineActionsArray = new Action<object>[4];
      m_StateMachineActionsArray[(int)m_ButtonsTemplate.CancelPosition] = x => this.Cancel();
      m_StateMachineActionsArray[(int)StateMachineEventIndex.RightMiddleButtonEvent] = x => this.OnSetupButton();
      m_StateMachineActionsArray[(int)StateMachineEventIndex.LeftMiddleButtonEvent] = x => this.OnArchiveButton();
      m_StateMachineActionsArray[(int)StateMachineEventIndex.LeftButtonEvent] = x => { };
    }

    #region object
    /// <summary>
    /// Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
    public override string ToString()
    {
      return Infrastructure.ViewNames.ArchivalStateName;
    }
    #endregion

    #region BackgroundWorkerMachine
    public override void OnEnteringState()
    {
      this.Log(String.Format("OnEnteringState {0}", Infrastructure.ViewNames.ArchivalStateName), Category.Debug, Priority.Low);
      base.OnEnteringState();
      Context.EnabledEvents = m_ButtonsTemplate.SetEventsMask(false, true, true);
    }
    protected override void BackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
    {
      Process _myProcesses = Process.GetCurrentProcess();
      ReportProgress(this, new ProgressChangedEventArgs(1, String.Format("Starting archiving background process {0}.", _myProcesses.MainWindowTitle)));
      BackgroundProcessArgument _argument = (BackgroundProcessArgument)e.Argument;
      if ((_argument.Phases & Phases.CleanupContent) > 0)
        CleanupContent.DoCleanupContent(_argument.URL, _argument.SQLConnectionString, x => ReportProgress(this, x), y => { MemoryUsage(_myProcesses); this.Log(y, Category.Debug, Priority.None); });
      else
        this.ReportProgress(this, new ProgressChangedEventArgs(0, "Cleanup content skipped because is not selected by the user."));
      if ((_argument.Phases & Phases.SynchronizationContent) > 0)
        SynchronizationContent.DoSynchronizationContent(_argument.URL, _argument.SQLConnectionString, x => ReportProgress(this, x), y => this.Log(y, Category.Debug, Priority.None));
      else
        this.ReportProgress(this, new ProgressChangedEventArgs(0, "Synchronization content skipped because is not selected by the user."));
      if ((_argument.Phases & Phases.ArchivingContent) > 0)
        ArchivingContent.DoArchivingContent(_argument.URL, _argument.SQLConnectionString, _argument.ArchivalDelay, _argument.RowLimit, x => ReportProgress(this, x), y => this.Log(y, Category.Debug, Priority.None));
      else
        this.ReportProgress(this, new ProgressChangedEventArgs(0, "Archiving content skipped because is not selected by the user."));
    }

    private void MemoryUsage(Process _myProcesses)
    {
      _myProcesses.Refresh();
      string _template = "Physical memory usage: {0}, PagedSystemMemorySize64: {1}, PagedMemorySize64: {2},  PrivateMemorySize64: {3}, total processor time: {4}";
      this.Log(String.Format(_template, _myProcesses.WorkingSet64, _myProcesses.PagedSystemMemorySize64, _myProcesses.PagedMemorySize64, _myProcesses.PrivateMemorySize64, _myProcesses.TotalProcessorTime),
               Category.Debug, 
               Priority.None);
    }
    protected override void RunWorkerCompleted(object result)
    {
      Context.EnabledEvents = m_ButtonsTemplate.SetEventsMask(false, true, true);
      Context.ProgressChang(this, new System.ComponentModel.ProgressChangedEventArgs(0, "Finished archiving background process."));
    }
    protected override void OnlyCancelActive()
    {
      Context.EnabledEvents = m_ButtonsTemplate.OnlyCancel();
    }
    protected override ButtonsSetState ButtonsPanelState
    {
      get { return m_ButtonsTemplate; }
    }
    public override Action<object>[] StateMachineActionsArray
    {
      get { return m_StateMachineActionsArray; }
    }
    public override void Cancel()
    {
      Log("In ArchivingMachineState requested Cancel operation", Category.Debug, Priority.Low);
      base.Cancel();
    }
    public override void OnException(Exception exception)
    {
      base.OnException(exception);
      Context.EnabledEvents = m_ButtonsTemplate.SetEventsMask(false, false, true);
    }
    #endregion

    #region private
    private void OnSetupButton()
    {
      Log("User requested navigation to setup dialog screen.", Category.Debug, Priority.Low);
      NavigationParameters _par = new NavigationParameters();
      _par.Add(Infrastructure.ViewNames.SetupStateName, String.Empty);
      Context.RequestNavigate(Infrastructure.ViewNames.SetupStateName, _par);
    }
    private void OnArchiveButton()
    {
      Context.ProgressChang(this, new System.ComponentModel.ProgressChangedEventArgs(1, "Starting archival process - it could take several minutes."));
      RunAsync(GetArgument);
    }
    private CancelTemplate m_ButtonsTemplate;
    private Action<object>[] m_StateMachineActionsArray;
    #endregion

    #region abstract
    [Flags]
    protected enum Phases
    {
      CleanupContent = 0x1,
      SynchronizationContent = 0x2,
      ArchivingContent = 0x4
    }
    protected struct BackgroundProcessArgument
    {
      internal string URL;
      internal string SQLConnectionString;
      internal Phases Phases;
      internal int ArchivalDelay;
      internal int RowLimit;
    }
    protected abstract BackgroundProcessArgument GetArgument { get; }
    //ILoggerFacade
    public abstract void Log(string message, Category category, Priority priority);
    #endregion

  }
}
