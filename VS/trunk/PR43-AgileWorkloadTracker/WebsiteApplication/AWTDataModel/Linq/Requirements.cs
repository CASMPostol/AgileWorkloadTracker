//<summary>
//  Title   : Entity partial class Requirements
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
using System.Linq;

namespace CAS.AgileWorkloadTracker.DataModel.Linq
{
  /// <summary>
  /// Entity partial class Requirements
  /// </summary>
  partial class Requirements
  {

    #region API
    internal void Adjust(Entities edc)
    {
      foreach (Tasks _tix in this.Tasks)
      {
        _tix.MakeConsistent(edc);
      };
    }
    /// <summary>
    /// Moves this instance of <see cref="Requirements"/> to specified <paramref name="target"/>.
    /// </summary>
    /// <param name="edc">The instance of <see cref="Entities"/>.</param>
    /// <param name="target">The target <see cref="Milestone"/>.</param>
    internal void Move(Entities edc, Milestone target)
    {
      this.Requirements2MilestoneTitle = target;
      MakeConsistent(edc);
    }
    /// <summary>
    /// Makes the consistent this instance and all <see cref="Tasks"/> beneath.
    /// </summary>
    /// <param name="edc">The instance of <see cref="Entities"/>.</param>
    internal void MakeConsistent(Entities edc)
    {
      Requirements2ProjectsTitle = Requirements2MilestoneTitle.Milestone2ProjectTitle;
      foreach (Tasks _tskx in this.Tasks)
        _tskx.MakeConsistent(edc);
    }
    internal void Update(ref double hours, ref DateTime start, ref DateTime end, ref bool allInactive)
    {
      double _hours = 0;
      DateTime _start = DateTime.MaxValue;
      DateTime _end = DateTime.MinValue;
      foreach (Tasks _tx in this.Tasks)
        _tx.Update(ref _hours, ref _start, ref _end, ref allInactive);
      DataModelExtensions.UpdateWorkload(ref hours, ref start, ref end, _hours, _start, _end);
      this.Hours = _hours;
    }
    internal void CalculateWorkload()
    {
      if (Requirements2MilestoneTitle == null)
        return;
      Requirements2MilestoneTitle.Update();
    }
    /// <summary>
    /// Previous Hours: updated lookups Projects and Milestone and sums workload reported for all related tasks. 
    /// </summary>
    /// <value>The calculate hours.</value>
    internal double CalculateHours
    {
      get
      {
        if (this.Requirements2ProjectsTitle == null || this.Requirements2ProjectsTitle != this.Requirements2MilestoneTitle.Milestone2ProjectTitle)
          this.Requirements2ProjectsTitle = this.Requirements2MilestoneTitle.Milestone2ProjectTitle;
        double _hours = this.Tasks.Sum<Tasks>(a => a.Hours.GetValueOrDefault(0));
        RequirementPriority = Convert.ToInt32(Math.Round(_hours));
        return _hours;
      }
    }
    internal Requirements MakeCopy(Entities edc, Milestone target)
    {
      Requirements _ret = new Requirements()
      {
        Body = Body + String.Format("<div><p>copy from milestone {0}[{1}].</p></div>", this.Requirements2MilestoneTitle.Title, this.Requirements2MilestoneTitle.Id.Value),
        EstimatedHours = Math.Max(0, this.EstimatedHours.GetValueOrDefault(0) - this.Hours.GetValueOrDefault(0)),
        Hours = 0,
        RequirementPriority = this.RequirementPriority,
        Requirements2MilestoneTitle = target,
        Requirements2ProjectsTitle = target.Milestone2ProjectTitle,
        RequirementsType = this.RequirementsType,
        Title = String.Format("{0}-{1}", this.Title, target.Title)
      };
      edc.Requirements.InsertOnSubmit(_ret);
      return _ret;
    }
    internal static Requirements CreateDefault(Entities edc, Milestone milestone)
    {
      string _defMilestoneTitle = String.Format("Dangling Tasks for {0}", milestone.Title);
      Requirements _defR = new Requirements()
      {
        Body = String.Format("<div><p>Dangling Tasks for milestone {0}[{1}].</p></div>", milestone.Title, milestone.Id.Value),
        Requirements2MilestoneTitle = milestone,
        Requirements2ProjectsTitle = milestone.Milestone2ProjectTitle,
        RequirementsType = new Nullable<RequirementsType>(),
        Title = _defMilestoneTitle,
      };
      edc.Requirements.InsertOnSubmit(_defR);
      return _defR;
    }
    #endregion

    #region private
    private void AdjustProjectLookup()
    {
      if (this.Requirements2ProjectsTitle == null || this.Requirements2ProjectsTitle != this.Requirements2MilestoneTitle.Milestone2ProjectTitle)
        this.Requirements2ProjectsTitle = this.Requirements2MilestoneTitle.Milestone2ProjectTitle;
    }
    #endregion

  }
}
