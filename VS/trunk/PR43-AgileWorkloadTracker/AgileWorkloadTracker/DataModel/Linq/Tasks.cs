using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.AgileWorkloadTracker.DataModel.Linq
{
  partial class Tasks
  {
    internal void Adjust(Entities edc)
    {
      foreach (Workload _wix in from _ix in this.Workload where _ix.Workload2ProjectTitle.Identyfikator != this.Task2ProjectTitle.Identyfikator select _ix)
        _wix.Workload2ProjectTitle = this.Task2ProjectTitle;
    }
    internal void CalculateWorkload()
    {
      if (this.Task2RequirementsTitle == null)
        return;
      this.Task2RequirementsTitle.CalculateWorkload();
    }
    internal double CalculatedHours
    {
      get
      {
        if (this.Task2MilestoneResolvedInTitle == null || this.Task2MilestoneResolvedInTitle != this.Task2RequirementsTitle.Requirements2MilestoneTitle)
          throw new ArgumentOutOfRangeException("Task2MilestoneResolvedInTitle", this.Task2RequirementsTitle.Title);
        if (this.Task2ProjectTitle == null || this.Task2ProjectTitle != this.Task2RequirementsTitle.Requirements2ProjectsTitle)
          throw new ArgumentOutOfRangeException("Task2ProjectTitle", this.Title);
        if (!Active.HasValue || this.Task2StatusTitle.Active.Value != Active.Value)
          throw new ArgumentOutOfRangeException("Active", this.Title);
        return Workload.Sum<Workload>(_Workload => _Workload.Hours.GetValueOrDefault(0));
      }
    }
    private void Adjust()
    {
      if (this.Task2MilestoneResolvedInTitle == null || this.Task2MilestoneResolvedInTitle != this.Task2RequirementsTitle.Requirements2MilestoneTitle)
        this.Task2MilestoneResolvedInTitle = this.Task2RequirementsTitle.Requirements2MilestoneTitle;
      if (this.Task2ProjectTitle == null || this.Task2ProjectTitle != this.Task2RequirementsTitle.Requirements2ProjectsTitle)
        this.Task2ProjectTitle = this.Task2RequirementsTitle.Requirements2ProjectsTitle;
      if (!Active.HasValue || this.Task2StatusTitle.Active.Value != Active.Value)
        Active = this.Task2StatusTitle.Active.Value;
    }
  }
}
