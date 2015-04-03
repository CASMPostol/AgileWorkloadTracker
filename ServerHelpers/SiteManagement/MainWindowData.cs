//<summary>
//  Title   : class MainWindowData
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

using CAS.AgileWorkloadTracker.SiteManagement.Linq;
using CAS.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace CAS.AgileWorkloadTracker.SiteManagement
{

  /// <summary>
  /// class MainWindowData
  /// </summary>
  sealed internal class MainWindowData : ViewModelBackgroundWorker
  {

    #region public

    #region creator
    public MainWindowData()
    {
      SiteURL = Properties.Settings.Default.SiteURL;
      MilestoneCollection = new ObservableCollection<MilestoneWrapper>();
    }
    #endregion

    #region UI properties
    public string SiteURL
    {
      get
      {
        return b_SiteURL;
      }
      set
      {
        RaiseHandler<string>(value, ref b_SiteURL, "SiteURL", this);
      }
    }
    public ObservableCollection<MilestoneWrapper> MilestoneCollection
    {
      get
      {
        return b_MilestoneCollection;
      }
      set
      {
        RaiseHandler<ObservableCollection<MilestoneWrapper>>(value, ref b_MilestoneCollection, "MilestoneCollection", this);
      }
    }
    #endregion

    #region API
    internal void Connect(RunWorkerCompletedEventHandler completedEventHandler)
    {
      if (!NotBusy)
        throw new System.ComponentModel.InvalidAsynchronousStateException("The operation cannot be started because the background worker is busy");
      if (completedEventHandler == null)
        throw new ArgumentNullException("result");
      CheckDisposed<MainWindowData>();
      m_BWDoWorkEventHandler = BackgroundWorker_DoConnect;
      m_BWCompletedEventHandler = completedEventHandler;
      StartBackgroundWorker();
    }
    internal void Disconnect(RunWorkerCompletedEventHandler completedEventHandler)
    {
      if (!Connected)
        return;
      if (!NotBusy)
        throw new System.ComponentModel.InvalidAsynchronousStateException("The operation cannot be started because the background worker is busy");
      m_BWDoWorkEventHandler = BackgroundWorker_DoDisconnect;
      m_BWCompletedEventHandler = completedEventHandler;
      StartBackgroundWorker();
    }
    internal bool Connected { get { return m_Entities != null; } }
    internal void Update(MilestoneWrapper milestoneWrapper, RunWorkerCompletedEventHandler completedEventHandler)
    {
      CheckDisposed<MainWindowData>();
      if (milestoneWrapper == null)
        throw new ArgumentNullException("milestoneWrapper");
      m_BWDoWorkEventHandler = BackgroundWorker_DoMakeInactive;
      m_BWCompletedEventHandler = completedEventHandler;
      StartBackgroundWorker(milestoneWrapper);
    }
    internal void ForceMakeInactive(MilestoneWrapper source, MilestoneWrapper target, RunWorkerCompletedEventHandler completedEventHandler)
    {
      CheckDisposed<MainWindowData>();
      if (source == null)
        throw new ArgumentNullException("source");
      if (target == null)
        throw new ArgumentNullException("target");
      m_BWDoWorkEventHandler = BackgroundWorker_DoForceMakeInactive;
      m_BWCompletedEventHandler = completedEventHandler;
      StartBackgroundWorker(new ForceMakeInactiveArgument() { Source = source, Target = target });
    }
    internal void Move(MilestoneWrapper source, MilestoneWrapper target, RunWorkerCompletedEventHandler completedEventHandler)
    {
      CheckDisposed<MainWindowData>();
      if (source == null)
        throw new ArgumentNullException("source");
      if (target == null)
        throw new ArgumentNullException("target");
      m_BWDoWorkEventHandler = BackgroundWorker_DoMove;
      m_BWCompletedEventHandler = completedEventHandler;
      StartBackgroundWorker(new ForceMakeInactiveArgument() { Source = source, Target = target });
    }
    internal static void GetRequirements(MilestoneWrapper milestoneWrapper, Action<object, RunWorkerCompletedEventArgs> GetRequirementsCompleted)
    {
      throw new NotImplementedException();
    }
    #endregion

    #endregion

    #region IDisposable Members
    protected override void Dispose(bool disposing)
    {
      if (disposing)
        DisposeEntities();
      base.Dispose(disposing);
    }
    #endregion

    #region private
    private class ForceMakeInactiveArgument
    {
      public MilestoneWrapper Source { get; set; }
      public MilestoneWrapper Target { get; set; }
    }
    private ObservableCollection<MilestoneWrapper> b_MilestoneCollection;
    private string b_SiteURL;
    private DataModel.Linq.Entities m_Entities;  //Must be disposed.

    #region BackgroundWorker
    private DoWorkEventHandler m_BWDoWorkEventHandler = null;
    private RunWorkerCompletedEventHandler m_BWCompletedEventHandler = null;
    private ProgressChangedEventHandler m_BWProgressChangedEventHandler = null;
    //Dedicated DoWork delegates
    private object BackgroundWorker_DoConnect(object argument, Action<ProgressChangedEventArgs> progress, Func<bool> cancellationPending)
    {
      return GetMilestonesCollection();
    }
    private object BackgroundWorker_DoDisconnect(object argument, Action<ProgressChangedEventArgs> progress, Func<bool> cancellationPending)
    {
      progress(new ProgressChangedEventArgs(0, "DoDisconnect starting"));
      DisposeEntities();
      Properties.Settings.Default.Save();
      progress(new ProgressChangedEventArgs(100, "DoDisconnect ending"));
      return null;
    }
    private object BackgroundWorker_DoMakeInactive(object argument, Action<ProgressChangedEventArgs> progress, Func<bool> cancellationPending)
    {
      progress(new ProgressChangedEventArgs(0, "DoMakeInactive starting"));
      MilestoneWrapper _mlstn = argument as MilestoneWrapper;
      _mlstn.Update(m_Entities);
      m_Entities.SubmitChanges();
      ObservableCollection<MilestoneWrapper> _ret = GetMilestonesCollection();
      progress(new ProgressChangedEventArgs(100, "DoMakeInactive ending"));
      return _ret;
    }
    private object BackgroundWorker_DoForceMakeInactive(object argument, Action<ProgressChangedEventArgs> progress, Func<bool> cancellationPending)
    {
      progress(new ProgressChangedEventArgs(0, "DoForceMakeInactive starting"));
      ForceMakeInactiveArgument _agumnt = argument as ForceMakeInactiveArgument;
      _agumnt.Source.ForceMakeInactive(m_Entities, _agumnt.Target);
      m_Entities.SubmitChanges();
      ObservableCollection<MilestoneWrapper> _ret = GetMilestonesCollection();
      progress(new ProgressChangedEventArgs(100, "DoForceMakeInactive ending"));
      return _ret;
    }
    private object BackgroundWorker_DoMove(object argument, Action<ProgressChangedEventArgs> progress, Func<bool> cancellationPending)
    {
      progress(new ProgressChangedEventArgs(0, "DoMove starting"));
      ForceMakeInactiveArgument _agumnt = argument as ForceMakeInactiveArgument;
      _agumnt.Source.Move(m_Entities, _agumnt.Target);
      m_Entities.SubmitChanges();
      ObservableCollection<MilestoneWrapper> _ret = GetMilestonesCollection();
      progress(new ProgressChangedEventArgs(100, "DoMove ending"));
      return _ret;
    }
    #endregion

    private ObservableCollection<MilestoneWrapper> GetMilestonesCollection()
    {
      DisposeEntities();
      m_Entities = new DataModel.Linq.Entities(SiteURL);
      List<DataModel.Linq.Milestone> _mls = (from _mlsx in m_Entities.Milestone
                                             where _mlsx.Active.GetValueOrDefault(true)
                                             select _mlsx).ToList();
      _mls.Sort();
      List<MilestoneWrapper> _empty = new List<MilestoneWrapper>();
      _empty.Add(new MilestoneWrapper(null));
      foreach (var _mstx in _mls)
        _empty.Add(new MilestoneWrapper(_mstx));
      return new ObservableCollection<MilestoneWrapper>(_empty);
    }
    private void DisposeEntities()
    {
      if (m_Entities == null)
        return;
      m_Entities.Dispose();
      m_Entities = null;
    }
    #endregion

    #region BaseModelViewBackgroundWorker implementation
    /// <summary>
    /// Gets the do work event handler.
    /// </summary>
    /// <value>
    /// The do work event handler.
    /// </value>
    protected override ViewModelBackgroundWorker.DoWorkEventHandler GetDoWorkEventHandler
    {
      get { return m_BWDoWorkEventHandler; }
    }
    /// <summary>
    /// Gets the completed event handler.
    /// </summary>
    /// <value>
    /// The completed event handler.
    /// </value>
    protected override RunWorkerCompletedEventHandler CompletedEventHandler
    {
      get { return m_BWCompletedEventHandler; }
    }
    /// <summary>
    /// Gets the progress changed event handler.
    /// </summary>
    /// <value>
    /// The progress changed event handler.
    /// </value>
    protected override ProgressChangedEventHandler ProgressChangedEventHandler
    {
      get { return m_BWProgressChangedEventHandler; }
    }
    /// <summary>
    /// Called when the NotBusy has been changed].
    /// </summary>
    protected override void OnNotBusyChanged()
    {
    }
    #endregion


  }
}
