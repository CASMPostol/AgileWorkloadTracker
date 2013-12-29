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
      MilestoneCollection = new ObservableCollection<Linq.ElementWrapper<DataModel.Linq.Milestone>>();
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
    public ObservableCollection<Linq.ElementWrapper<CAS.AgileWorkloadTracker.DataModel.Linq.Milestone>> MilestoneCollection
    {
      get
      {
        return b_MilestoneCollection;
      }
      set
      {
        PropertyChanged.RaiseHandler<System.Collections.ObjectModel.ObservableCollection<Linq.ElementWrapper<CAS.AgileWorkloadTracker.DataModel.Linq.Milestone>>>(value, ref b_MilestoneCollection, "MilestoneCollection", this);
      }
    }
    #endregion

    #region API
    internal void GetMilestoneCollection(RunWorkerCompletedEventHandler result)
    {
      if (m_BackgroundWorker.IsBusy)
        throw new System.ComponentModel.InvalidAsynchronousStateException("The operation cannot be started because the background worker is busy");
      if (result == null)
        throw new ArgumentNullException("result");
      if (m_Disposed)
        throw new ObjectDisposedException(typeof(MainWindowData).Name);
      m_BWDoWorkEventHandler = m_BackgroundWorker_DoGetMilestones;
      m_BWCompletedEventHandler = result;
      m_BackgroundWorker.RunWorkerAsync();
    }
    #endregion

    #endregion

    #region INotifyPropertyChanged Members
    public event PropertyChangedEventHandler PropertyChanged;
    #endregion

    #region private
    private System.Collections.ObjectModel.ObservableCollection<Linq.ElementWrapper<CAS.AgileWorkloadTracker.DataModel.Linq.Milestone>> b_MilestoneCollection;
    private string b_SiteURL;
    private CAS.AgileWorkloadTracker.DataModel.Linq.Entities m_Entities;  //Must be disposed.
    private bool m_Disposed = false;
    private System.Threading.ManualResetEvent m_DisposeDone = null;

    #region BackgroundWorker
    private System.ComponentModel.BackgroundWorker m_BackgroundWorker = new BackgroundWorker()
      {
        WorkerReportsProgress = false,
        WorkerSupportsCancellation = false
      };
    private DoWorkEventHandler m_BWDoWorkEventHandler = null;
    private RunWorkerCompletedEventHandler m_BWCompletedEventHandler = null;
    private ProgressChangedEventHandler m_BWProgressChangedEventHandler = null;
    private void m_BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      m_BWCompletedEventHandler(sender, e);
      m_BWCompletedEventHandler = null;
    }
    private void m_BackgroundWorker_RunWorkerCompletedDoDispose(object sender, RunWorkerCompletedEventArgs e)
    {
      m_DisposeDone.Set();
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
    private void m_BackgroundWorker_DoGetMilestones(object sender, DoWorkEventArgs e)
    {
      e.Cancel = false;
      e.Result = false;
      if (m_Entities != null)
        m_Entities.Dispose();
      m_Entities = new DataModel.Linq.Entities(SiteURL);
      IQueryable<DataModel.Linq.Milestone> _mls = from _mlsx in m_Entities.Milestone orderby _mlsx.Title select _mlsx;
      List<ElementWrapper<DataModel.Linq.Milestone>> _empty = new List<ElementWrapper<DataModel.Linq.Milestone>>();
      _empty.Add(new ElementWrapper<DataModel.Linq.Milestone>(null));
      foreach (var _mstx in _mls)
        _empty.Add(new ElementWrapper<DataModel.Linq.Milestone>(_mstx));
      MilestoneCollection = new ObservableCollection<ElementWrapper<DataModel.Linq.Milestone>>(_empty);
    }
    private void m_BackgroundWorker_DoDispose(object sender, DoWorkEventArgs e)
    {
      if (m_Entities != null)
        m_Entities.Dispose();
      m_Entities = null;
    }
    #endregion

    #endregion


    #region IDisposable Members
    public void Dispose()
    {
      if (m_Disposed)
        return;
      if (m_BackgroundWorker.IsBusy)
        throw new System.ComponentModel.InvalidAsynchronousStateException("The operation cannot be started because the background worker is busy");
      m_DisposeDone = new System.Threading.ManualResetEvent(false);
      m_BWDoWorkEventHandler = m_BackgroundWorker_DoDispose;
      m_BWCompletedEventHandler = m_BackgroundWorker_RunWorkerCompletedDoDispose;
      m_BackgroundWorker.RunWorkerAsync();
      m_DisposeDone.WaitOne();
      m_BackgroundWorker.Dispose();
      m_BackgroundWorker = null;
      m_Disposed = true;
    }
    #endregion

  }
}
