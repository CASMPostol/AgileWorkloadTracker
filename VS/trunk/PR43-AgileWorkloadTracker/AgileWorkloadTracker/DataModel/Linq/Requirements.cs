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

    private void AdjustProjectLookup()
    {
      if (this.Requirements2ProjectsTitle == null || this.Requirements2ProjectsTitle != this.Requirements2MilestoneTitle.Milestone2ProjectTitle)
        this.Requirements2ProjectsTitle = this.Requirements2MilestoneTitle.Milestone2ProjectTitle;
    }

  }
}
