using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.AgileWorkloadTracker.DataModel.Linq
{
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
        Body = this.Body,
        EstimatedHours = Math.Max(0, this.EstimatedHours.GetValueOrDefault(0) - this.Hours.GetValueOrDefault(0)),
        Hours = 0,
        RequirementPriority = this.RequirementPriority,
        Requirements2MilestoneTitle = target,
        Requirements2ProjectsTitle = target.Milestone2ProjectTitle,
        RequirementsType = this.RequirementsType,
        Title = String.Format("{0} - copy from: {0}", this.Title, this.Requirements2MilestoneTitle.Title)
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
