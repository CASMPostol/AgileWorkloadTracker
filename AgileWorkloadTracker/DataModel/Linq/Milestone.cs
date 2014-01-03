//<summary>
//  Title   : Entity partial class Milestone
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
  /// Entity partial class Milestone
  /// </summary>
  partial class Milestone
  {
    internal void Adjust(Entities edc)
    {
      List<Tasks> _danglingTasks = (from _tix in this.Tasks0 where _tix.Task2RequirementsTitle == null select _tix).ToList<Tasks>();
      if (_danglingTasks.Count > 0)
      {
        string _defMilestoneTitle = "DanglingTasks";
        Requirements _defR = (from _ix in this.Requirements where _ix.Title.Contains(_defMilestoneTitle) select _ix).FirstOrDefault<Requirements>();
        if (_defR == null)
        {
          _defR = new Requirements()
          {
            Body = _defMilestoneTitle,
            Requirements2MilestoneTitle = this,
            Requirements2ProjectsTitle = this.Milestone2ProjectTitle,
            RequirementsType = RequirementsType.Invalid,
            Title = _defMilestoneTitle,
          };
          edc.Requirements.InsertOnSubmit(_defR);
        }
        foreach (Tasks _task in _danglingTasks)
          _task.Task2RequirementsTitle = _defR;
        edc.SubmitChanges();
      }
      foreach (var _rix in this.Requirements)
      {
        if (_rix.Requirements2ProjectsTitle != this.Milestone2ProjectTitle)
          _rix.Requirements2ProjectsTitle = this.Milestone2ProjectTitle;
        _rix.Adjust(edc);
      }
    }
    /// <summary>
    /// Calculates the workload.
    /// </summary>
    public void CalculateWorkload()
    {
      double _hours = 0;
      DateTime _start = DateTime.MaxValue;
      DateTime _end = DateTime.MinValue;
      foreach (Requirements _wx in this.Requirements)
        _wx.Update(ref _hours, ref _start, ref _end);
      this.MilestoneStart = _start;
      this.MilestoneEnd = _end;
      this.MilestoneHours = _hours;
    }
    /// <summary>
    /// Makes the instance inactive if possible.
    /// </summary>
    public void MakeInactive()
    {
      CalculateWorkload();
      Inactive = true;
    }
    /// <summary>
    /// Forces the make inactive.
    /// </summary>
    /// <param name="edc">The object of <see cref="Entities"/>.</param>
    /// <param name="target">The target.</param>
    /// <exception cref="System.ArgumentNullException">target</exception>
    public void ForceMakeInactive(Entities edc, Milestone target)
    {
      if (target == null)
        throw new ArgumentNullException("target");
      Dictionary<int, DataModel.Linq.Requirements> _Requirements = new Dictionary<int, Requirements>();
      List<Tasks> _tsks = Tasks0.Where(x => x.Active.GetValueOrDefault(false)).ToList();
      foreach (DataModel.Linq.Tasks _tskx in _tsks)
      {
        Requirements _targetRequirement = null;
        if (_Requirements.ContainsKey(_tskx.Task2RequirementsTitle.Identyfikator.Value))
          _targetRequirement = _Requirements[_tskx.Task2RequirementsTitle.Identyfikator.Value];
        else
        {
          _targetRequirement = _tskx.Task2RequirementsTitle.MakeCopy(edc, target);
          _Requirements.Add(_tskx.Task2RequirementsTitle.Identyfikator.Value, _targetRequirement);
        }
        _tskx.MoveToTarget(edc, _targetRequirement);
      }
      CalculateWorkload();
      Inactive = true;
    }
    private bool Inactive
    {
      get
      {
        return !this.Active.GetValueOrDefault(false);
      }
      set
      {
        if (value)
          this.Default = false;
        this.Active = !value;
      }
    }

  }
}
