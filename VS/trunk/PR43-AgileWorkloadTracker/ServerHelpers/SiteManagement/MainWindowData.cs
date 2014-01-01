//<summary>
//  Title   : class MainWindowData
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using CAS.AgileWorkloadTracker.SiteManagement.Linq;

namespace CAS.AgileWorkloadTracker.SiteManagement
{
  /// <summary>
  /// class MainWindowData
  /// </summary>
  sealed internal class MainWindowData : System.ComponentModel.INotifyPropertyChanged, IDisposable
  {

    #region public

    #region ctor
    public MainWindowData()
    {
      SiteURL = Properties.Settings.Default.SiteURL;
      MilestoneCollection = new ObservableCollection<MilestoneWrapper>();
      m_BackgroundWorker.DoWork += m_BackgroundWorker_DoWork;
      m_BackgroundWorker.ProgressChanged += m_BackgroundWorker_ProgressChanged;
      m_BackgroundWorker.RunWorkerCompleted += m_BackgroundWorker_RunWorkerCompleted;
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
        PropertyChanged.RaiseHandler<string>(value, ref b_SiteURL, "SiteURL", this);
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
        PropertyChanged.RaiseHandler<ObservableCollection<MilestoneWrapper>>(value, ref b_MilestoneCollection, "MilestoneCollection", this);
      }
    }
    public bool NotBusy
    {
      get
      {
        return b_NotBusy;
      }
      set
      {
        PropertyChanged.RaiseHandler<bool>(value, ref b_NotBusy, "NotBusy", this);
      }
    }
    #endregion

    #region API
    internal void Connect(RunWorkerCompletedEventHandler completedEventHandler)
    {
      if (m_BackgroundWorker.IsBusy)
        throw new System.ComponentModel.InvalidAsynchronousStateException("The operation cannot be started because the background worker is busy");
      if (completedEventHandler == null)
        throw new ArgumentNullException("result");
      CheckDisposed();
      m_BWDoWorkEventHandler = m_BackgroundWorker_DoConnect;
      m_BWCompletedEventHandler = completedEventHandler;
      StartBackgroundWorker();
    }
    internal void Disconnect(RunWorkerCompletedEventHandler completedEventHandler)
    {
      if (!Connected)
        return;
      if (m_BackgroundWorker.IsBusy)
        throw new System.ComponentModel.InvalidAsynchronousStateException("The operation cannot be started because the background worker is busy");
      m_BWDoWorkEventHandler = m_BackgroundWorker_DoDisconnect;
      m_BWCompletedEventHandler = completedEventHandler;
      StartBackgroundWorker();
    }
    internal bool Connected { get { return m_Entities != null; } }
    internal void MakeInactive(MilestoneWrapper milestoneWrapper, RunWorkerCompletedEventHandler completedEventHandler)
    {
      CheckDisposed();
      if (milestoneWrapper == null)
        throw new ArgumentNullException("milestoneWrapper");
      m_BWDoWorkEventHandler = m_BackgroundWorker_MakeInactive;
      m_BWCompletedEventHandler = completedEventHandler;
      StartBackgroundWorker(milestoneWrapper);
    }
    #endregion

    #endregion

    #region INotifyPropertyChanged Members
    public event PropertyChangedEventHandler PropertyChanged;
    #endregion

    #region IDisposable Members
    public void Dispose()
    {
      if (m_Disposed)
        return;
      if (m_BackgroundWorker == null)
        m_BackgroundWorker.Dispose();
      m_BackgroundWorker = null;
      m_Disposed = true;
    }
    #endregion

    #region private
    private ObservableCollection<MilestoneWrapper> b_MilestoneCollection;
    private string b_SiteURL;
    private CAS.AgileWorkloadTracker.DataModel.Linq.Entities m_Entities;  //Must be disposed.
    private bool m_Disposed = false;
    private bool b_NotBusy = true;

    #region BackgroundWorker
    private void StartBackgroundWorker()
    {
      NotBusy = false;
      m_BackgroundWorker.RunWorkerAsync();
    }
    private void StartBackgroundWorker(object argument)
    {
      NotBusy = false;
      m_BackgroundWorker.RunWorkerAsync(argument);
    }
    private System.ComponentModel.BackgroundWorker m_BackgroundWorker = new BackgroundWorker()
      {
        WorkerReportsProgress = true,
        WorkerSupportsCancellation = false
      };
    private DoWorkEventHandler m_BWDoWorkEventHandler = null;
    private RunWorkerCompletedEventHandler m_BWCompletedEventHandler = null;
    private ProgressChangedEventHandler m_BWProgressChangedEventHandler = null;
    private void m_BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      m_BWCompletedEventHandler(sender, e);
      m_BWCompletedEventHandler = null;
      NotBusy = true;
    }
    private void m_BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      if (m_BWProgressChangedEventHandler != null)
        m_BWProgressChangedEventHandler(sender, e);
    }
    private void m_BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      e.Cancel = false;
      e.Result = false;
      if (m_BWDoWorkEventHandler == null)
        return;
      m_BWDoWorkEventHandler(sender, e);
      m_BWDoWorkEventHandler = null;
    }
    //Dedicated DoWork delegates
    private void m_BackgroundWorker_DoConnect(object sender, DoWorkEventArgs e)
    {
      e.Cancel = false;
      e.Result = null;
      if (m_Entities != null)
        m_Entities.Dispose();
      m_Entities = new DataModel.Linq.Entities(SiteURL);
      IQueryable<DataModel.Linq.Milestone> _mls = from _mlsx in m_Entities.Milestone
                                                  let _prt = _mlsx.Milestone2ProjectTitle.Title
                                                  where _mlsx.Active.Value
                                                  orderby _mlsx.Title
                                                  orderby _mlsx.SortOrder.GetValueOrDefault(-999999) ascending
                                                  orderby _prt
                                                  select _mlsx;
      List<MilestoneWrapper> _empty = new List<MilestoneWrapper>();
      _empty.Add(new MilestoneWrapper(null));
      foreach (var _mstx in _mls)
        _empty.Add(new MilestoneWrapper(_mstx));
      MilestoneCollection = new ObservableCollection<MilestoneWrapper>(_empty);
    }
    private void m_BackgroundWorker_DoDisconnect(object sender, DoWorkEventArgs e)
    {
      e.Cancel = false;
      e.Result = null;
      if (m_Entities == null)
        return;
      m_Entities.Dispose();
      m_Entities = null;
      Properties.Settings.Default.Save();
    }
    private void m_BackgroundWorker_MakeInactive(object sender, DoWorkEventArgs e)
    {
      BackgroundWorker _wrkr = sender as BackgroundWorker;
      MilestoneWrapper _mlstn = e.Argument as MilestoneWrapper;
      _mlstn.MakeInactive();
      m_Entities.SubmitChanges();
    }
    #endregion

    private void CheckDisposed()
    {
      if (m_Disposed)
        throw new ObjectDisposedException(typeof(MainWindowData).Name);
    }
    #endregion

  }
}
