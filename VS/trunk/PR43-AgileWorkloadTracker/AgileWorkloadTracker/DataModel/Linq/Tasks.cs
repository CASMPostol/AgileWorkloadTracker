using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.AgileWorkloadTracker.DataModel.Linq
{
  partial class Tasks
  {
    #region public API
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
    internal void MoveToTarget(Entities edc, Requirements target)
    {
      if (!this.Active.GetValueOrDefault(false))
        throw new ArgumentOutOfRangeException("Active", String.Format("Active cannot be {0} while moving the task to new requirement.", this.Active));
      Tasks _tsk = this;
      if (this.Workload.Any())
        _tsk = MakeCopy(edc, target);
      _tsk.Connecr2Target(edc, target, this.Task2CategoryTitle);
    }
    #endregion

    #region private
    private int c_ResolutionFixed = 2;
    private int c_ResolutionFixLater = 5;
    private int c_StatusResolved = 4;
    private void Connecr2Target(Entities edc, Requirements target, Category categoty)
    {
      this.Task2MilestoneResolvedInTitle = target.Requirements2MilestoneTitle;
      this.Task2ProjectTitle = target.Requirements2MilestoneTitle.Milestone2ProjectTitle;
      this.Task2RequirementsTitle = target;
      Task2CategoryTitle = Task2ProjectTitle == categoty.Category2ProjectsTitle ? categoty : Task2ProjectTitle.FindCategory(edc, this.Task2CategoryTitle.Title);
    }
    private Tasks MakeCopy(Entities edc, Requirements targetRequirements)
    {
      string _Cmnts = String.Empty;
      int _idx = 0;
      foreach (Linq.TaskComments _cx in TaskComments)
      {
        _Cmnts += _cx.Body;
        if (_idx++ > 3)
          break;
      }
      Tasks _newTask = new Tasks()
      {
        Active = this.Active,
        BaselineEnd = this.BaselineEnd,
        BaselineStart = this.BaselineStart,
        Body = Body + String.Format("<div><p>copy from milestone {0}.</p></div><div>{1}</div>", targetRequirements.Requirements2MilestoneTitle.Title, _Cmnts),
        Task2CategoryTitle = null, // assigned in Connecr2Target 
        Task2MilestoneDefinedInTitle = this.Task2MilestoneDefinedInTitle,
        Task2MilestoneResolvedInTitle = null, // assigned in Connecr2Target
        Task2ProjectTitle = null, // assigned in Connecr2Target
        Task2RequirementsTitle = targetRequirements,
        Task2ResourcesTitle = this.Task2ResourcesTitle,
        Task2SPriorityTitle = this.Task2SPriorityTitle,
        Task2SResolutionTitle = this.Task2SResolutionTitle,
        Task2StatusTitle = this.Task2StatusTitle,
        Task2TypeTitle = this.Task2TypeTitle,
        TaskEnd = DateTime.Today,
        TaskStart = DateTime.Today,
        Title = this.Title
      };
      edc.Task.InsertOnSubmit(_newTask);
      this.Task2SResolutionTitle = Element.GetAtIndex<Resolution>(edc.Resolution, c_ResolutionFixLater);
      this.Task2StatusTitle = Element.GetAtIndex<Status>(edc.Status, c_StatusResolved);
      this.Active = this.Task2StatusTitle.Active.Value;
      return _newTask;
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
    #endregion

  }
}
