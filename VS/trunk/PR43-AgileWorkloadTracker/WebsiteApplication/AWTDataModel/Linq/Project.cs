//<summary>
//  Title   : partial class Projects
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
using System.Linq;

namespace CAS.AgileWorkloadTracker.DataModel.Linq
{
  /// <summary>
  /// Entity partial class Projects
  /// </summary>
  partial class Projects
  {
    /// <summary>
    /// Adjusts the project.
    /// </summary>
    /// <param name="edc">The edc.</param>
    public void Adjust(Entities edc)
    {
      List<Tasks> _danglingTasks = (from _tix in this.Tasks where _tix.Task2MilestoneResolvedInTitle == null select _tix).ToList<Tasks>();
      if (_danglingTasks.Count > 0)
      {
        string _defMilestoneTitle = "DanglingTasks";
        Milestone _defM = (from _ix in this.Milestone where _ix.Title.Contains(_defMilestoneTitle) select _ix).FirstOrDefault<Milestone>();
        if (_defM == null)
        {
          _defM = new Milestone()
          {
            Active = true,
            BaselineEnd = DateTime.Today,
            BaselineStart = DateTime.Today,
            Default = false,
            Milestone2ProjectTitle = this,
            MilestoneEnd = DateTime.Today,
            MilestoneHours = 0,
            MilestoneStart = DateTime.Today,
            Title = _defMilestoneTitle,
          };
          edc.Milestone.InsertOnSubmit(_defM);
        }
        foreach (Tasks _task in _danglingTasks)
          _task.Task2MilestoneResolvedInTitle = _defM;
      }
      edc.SubmitChanges();
      foreach (Milestone _mix in this.Milestone)
        _mix.Adjust(edc);
    }
    internal Category FindCategory(Entities edc, string title)
    {
      Category _ret = (from _cx in this.Category where _cx.Title == title select _cx).FirstOrDefault<Linq.Category>();
      if (_ret == null)
      {
        _ret = new Category()
        {
          Category2ProjectsTitle = this,
          Title = title
        };
        edc.Category.InsertOnSubmit(_ret);
      }
      return _ret;
    }

  }
}
