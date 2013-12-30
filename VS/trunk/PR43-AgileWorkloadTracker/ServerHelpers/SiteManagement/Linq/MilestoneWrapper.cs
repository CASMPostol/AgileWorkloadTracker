//<summary>
//  Title   : class MilestoneWrapper 
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
      List<DataModel.Linq.Tasks> _activeTasksList = (from _tsx in Element.Tasks0
                                                     let _rt = _tsx.Task2RequirementsTitle.Title
                                                     where _tsx.Active.GetValueOrDefault(false)
                                                     orderby _rt
                                                     select _tsx).ToList();
      b_ActiveTasks = _activeTasksList.Count;
      StringBuilder _description = new StringBuilder();
      double _hours = 0;
      foreach (var _at in _activeTasksList)
        _hours += _at.Workload.Sum(x => x.Hours.GetValueOrDefault(0));
      _description.AppendFormat("{0}: Accepted Hours={1}, Estimated Hours={2}, Hours {3}", milestone.Title, milestone.AcceptedHours.GetValueOrDefault(0), milestone.EstimatedHours.GetValueOrDefault(0), _hours);
      foreach (var _rqx in milestone.Requirements)
      {
        _description.AppendLine(_rqx.Title);
      }
      b_Description = _description.ToString();
    }
    public string Description
    {
      get
      {
        return b_Description;
      }
      set
      {
        RaiseHandler<string>(value, ref b_Description, "Description", this);
      }
    }
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

    #region private
    private string b_Description;
    private int b_ActiveTasks;
    #endregion

  }
}
