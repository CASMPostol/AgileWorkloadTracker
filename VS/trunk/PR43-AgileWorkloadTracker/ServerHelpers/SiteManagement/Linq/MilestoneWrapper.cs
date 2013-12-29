//<summary>
//  Title   : class MilestoneWrapper 
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
using System.Linq;
using CAS.AgileWorkloadTracker.SiteManagement;

namespace CAS.AgileWorkloadTracker.SiteManagement.Linq
{
  internal class MilestoneWrapper : ElementWrapper<DataModel.Linq.Milestone>
  {

    public MilestoneWrapper(DataModel.Linq.Milestone milestone)
      : base(milestone)
    {
      if (milestone == null)
        return;
      b_ActiveTasks = Element.Tasks0.Where<DataModel.Linq.Tasks>(x => x.Active.GetValueOrDefault(false)).Count();
    }

    private int b_ActiveTasks;

    public int ActiveTasks
    {
      get
      {
        return b_ActiveTasks;
      }
      set
      {
        RaiseHandler<int>(value, ref b_ActiveTasks, "ActiveTasks", this);
      }
    }

    #region object
    public override string ToString()
    {
      if (Element == null)
        return base.ToString();
      return String.Format("{0} Active tasks {1}", Element.Title, ActiveTasks);
    }
    #endregion

  }
}
