//<summary>
//  Title   : Entity partial class Requirements
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
using System.Linq;

namespace CAS.AgileWorkloadTracker.DataModel.Linq
{
  /// <summary>
  /// Entity partial class Requirements
  /// </summary>
  partial class Requirements
  {
    internal void Adjust(Entities edc)
    {
      foreach (Tasks _tix in this.Tasks)
      {
        if (_tix.Task2MilestoneResolvedInTitle != this.Requirements2MilestoneTitle)
          _tix.Task2MilestoneResolvedInTitle = this.Requirements2MilestoneTitle;
        if (_tix.Task2ProjectTitle != this.Requirements2ProjectsTitle)
          _tix.Task2ProjectTitle = this.Requirements2ProjectsTitle;
        _tix.Adjust(edc);
      };
    }
    internal double CalculatedHours
    {
      get
      {
        if (this.Requirements2ProjectsTitle == null || this.Requirements2ProjectsTitle != this.Requirements2MilestoneTitle.Milestone2ProjectTitle)
          throw new ArgumentOutOfRangeException("Requirements2ProjectsTitle", this.Title);
        Hours = this.Tasks.Sum<Tasks>(_Tasks => _Tasks.CalculatedHours);
        return Hours.Value;
      }
    }
    internal void CalculateWorkload()
    {
      if (Requirements2MilestoneTitle == null)
        return;
      Requirements2MilestoneTitle.CalculateWorkload();
    }
    internal Requirements MakeCopy(Entities edc, Milestone target)
    {
      Requirements _ret = new Requirements()
      {
        Body = Body + String.Format("<div><p>copy from milestone {0}.</p></div>", this.Requirements2MilestoneTitle.Title),
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
    private void AdjustProjectLookup()
    {
      if (this.Requirements2ProjectsTitle == null || this.Requirements2ProjectsTitle != this.Requirements2MilestoneTitle.Milestone2ProjectTitle)
        this.Requirements2ProjectsTitle = this.Requirements2MilestoneTitle.Milestone2ProjectTitle;
    }

  }
}
