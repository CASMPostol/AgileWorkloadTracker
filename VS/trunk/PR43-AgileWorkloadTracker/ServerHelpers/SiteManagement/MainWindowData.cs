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

    public MainWindowData()
    {
      SiteURL = String.Empty; // Properties.Settings.Default.SiteURL;
      MilestoneCollection = new ObservableCollection<Linq.ElementWrapper<DataModel.Linq.Milestone>>();
    }

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

    internal void GetMilestoneCollection()
    {
      Dispose();
      m_Entities = new DataModel.Linq.Entities(SiteURL);
      m_Disposed = false;
      IQueryable<ElementWrapper<DataModel.Linq.Milestone>> _mls = from _mlsx in m_Entities.Milestone orderby _mlsx.Title select new ElementWrapper<DataModel.Linq.Milestone>(_mlsx);
      List<ElementWrapper<DataModel.Linq.Milestone>> _empty = new List<ElementWrapper<DataModel.Linq.Milestone>>();
      _empty.Add(new ElementWrapper<DataModel.Linq.Milestone>(null));
      _empty.AddRange(_mls);
      MilestoneCollection = new ObservableCollection<ElementWrapper<DataModel.Linq.Milestone>>(_empty);
    }

    #region INotifyPropertyChanged Members
    public event PropertyChangedEventHandler PropertyChanged;
    #endregion

    #region private
    private System.Collections.ObjectModel.ObservableCollection<Linq.ElementWrapper<CAS.AgileWorkloadTracker.DataModel.Linq.Milestone>> b_MilestoneCollection;
    private string b_SiteURL;
    private CAS.AgileWorkloadTracker.DataModel.Linq.Entities m_Entities;  //Must be disposed.
    private bool m_Disposed = true;
    #endregion


    #region IDisposable Members
    public void Dispose()
    {
      if (m_Disposed)
        return;
      m_Disposed = true;
      m_Entities.Dispose();
      m_Entities = null;
    }
    #endregion

  }
}
