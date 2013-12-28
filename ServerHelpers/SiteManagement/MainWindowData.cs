//<summary>
//  Title   : class MainWindowData
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate:$
//  $Rev:$
//  $LastChangedBy:$
//  $URL:$
//  $Id:$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>
      
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAS.AgileWorkloadTracker.SiteManagement
{
  /// <summary>
  /// class MainWindowData
  /// </summary>
  internal class MainWindowData: System.ComponentModel.INotifyPropertyChanged
  {

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
    public System.Collections.ObjectModel.ObservableCollection<Linq.ElementWrapper<CAS.AgileWorkloadTracker.DataModel.Linq.Milestone>> MilestoneCollection
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

    #region INotifyPropertyChanged Members
    public event PropertyChangedEventHandler PropertyChanged;
    #endregion

    #region private
    private System.Collections.ObjectModel.ObservableCollection<Linq.ElementWrapper<CAS.AgileWorkloadTracker.DataModel.Linq.Milestone>> b_MilestoneCollection;
    private string b_SiteURL;
    #endregion

  }
}
