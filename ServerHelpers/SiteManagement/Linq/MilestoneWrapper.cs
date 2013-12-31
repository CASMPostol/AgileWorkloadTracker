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
      _description.AppendFormat("Accepted Hours={0:f0}, Estimated Hours={1:f0}, Hours {2:f2}", milestone.AcceptedHours.GetValueOrDefault(0), milestone.EstimatedHours.GetValueOrDefault(0), _hours);
      _description.AppendLine();
      _description.AppendLine("Requirements:");
      NotInProgress = true;
      foreach (var _rqx in milestone.Requirements)
      {
        _description.AppendLine(_rqx.Title);
        foreach (var _tsx in from _tsx in _rqx.Tasks where _tsx.Active.GetValueOrDefault(false) orderby _tsx.Title select _tsx)
        {
          _description.AppendFormat("    Task:{0} hours: {1:f2}", _tsx.Title, _tsx.Workload.Sum(x => x.Hours.GetValueOrDefault(0)));
          _description.AppendLine();
          NotInProgress = false;
        }
      }
      _description.AppendLine();
      _description.AppendLine(NotInProgress ? "The milestone may be made inactive." : "The milestone cannot be made inactive");
      b_Description = _description.ToString();
    }
    /// <summary>
    /// Gets or sets a value indicating whether the Milestone is not in progress - may be made inactive.
    /// </summary>
    /// <value>
    ///   <c>true</c> if [not in progress]; otherwise, <c>false</c>.
    /// </value>
    public bool NotInProgress
    {
      get
      {
        return b_NotInProgress;
      }
      set
      {
        RaiseHandler<bool>(value, ref b_NotInProgress, "NotInProgress", this);
      }
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
    private bool b_NotInProgress;
    #endregion


    internal void MakeInactive()
    {
      if (base.Element == null)
        throw new ArgumentNullException("Element");
      if (!b_NotInProgress)
        throw new ApplicationException("MakeInactive - you try to inactivate still in progress milestone");
      Element.Active = null;
    }
  }
}
