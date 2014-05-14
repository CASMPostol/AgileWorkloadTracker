//<summary>
//  Title   : partial class Tasks
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
  /// Entity partial class Tasks
  /// </summary>
  partial class Tasks : IComparable<Tasks>
  {
    #region public API
    internal void MakeConsistent(Entities edc)
    {
      if (this.Task2RequirementsTitle == null)
        throw new ArgumentNullException("Task2RequirementsTitle", this.Title);
      AdjustActive();
      Connecr2Target(edc, this.Task2RequirementsTitle);
      foreach (Workload _wx in Workload)
        _wx.MakeConsistent();
    }
    internal void MakeConsistent(Entities edc, Func<Entities, Requirements> createDefault)
    {
      if (this.Task2RequirementsTitle == null)
        this.Task2RequirementsTitle = createDefault(edc);
      MakeConsistent(edc);
    }
    internal void CalculateWorkload()
    {
      if (this.Task2RequirementsTitle == null)
        return;
      this.Task2RequirementsTitle.CalculateWorkload();
    }
    internal void Update(ref double hours, ref DateTime start, ref DateTime end, ref bool allInactive)
    {
      if (this.Task2RequirementsTitle == null)
        throw new ArgumentNullException("Task2RequirementsTitle", this.Title);
      if (this.Task2MilestoneResolvedInTitle == null || this.Task2MilestoneResolvedInTitle != this.Task2RequirementsTitle.Requirements2MilestoneTitle)
        throw new ArgumentOutOfRangeException("Task2MilestoneResolvedInTitle", this.Task2RequirementsTitle.Title);
      if (this.Task2ProjectTitle == null || this.Task2ProjectTitle != this.Task2RequirementsTitle.Requirements2ProjectsTitle)
        throw new ArgumentOutOfRangeException("Task2ProjectTitle", this.Title);
      AdjustActive(ref allInactive);
      double _hours = 0;
      DateTime _start = DateTime.MaxValue;
      DateTime _end = DateTime.MinValue;
      foreach (Workload _wx in Workload)
        _wx.Update(ref _hours, ref _start, ref _end);
      if (_start != DateTime.MaxValue)
        this.TaskStart = _start;
      if (_end != DateTime.MinValue)
        this.TaskEnd = _end;
      DataModelExtensions.UpdateWorkload(ref hours, ref start, ref end, _hours, _start, _end);
    }
    internal void MoveToTarget(Entities edc, Requirements target)
    {
      AdjustActive();
      if (!this.Active.GetValueOrDefault(false))
        throw new ArgumentOutOfRangeException("Active", String.Format("Active cannot be {0} while moving the task to new requirement.", this.Active));
      Tasks _tsk = this;
      if (this.Workload.Any())
        _tsk = MakeCopy(edc);
      _tsk.Connecr2Target(edc, target);
    }
    #endregion

    #region private
    private int c_ResolutionFixed = 2;
    private int c_ResolutionFixLater = 5;
    private void Connecr2Target(Entities edc, Requirements target)
    {
      this.Task2RequirementsTitle = target;
      this.Task2MilestoneResolvedInTitle = this.Task2RequirementsTitle.Requirements2MilestoneTitle;
      this.Task2ProjectTitle = this.Task2RequirementsTitle.Requirements2MilestoneTitle.Milestone2ProjectTitle;
      if ((this.Task2CategoryTitle != null) && (Task2ProjectTitle != this.Task2CategoryTitle.Category2ProjectsTitle))
        this.Task2CategoryTitle = Task2ProjectTitle.FindCategory(edc, this.Task2CategoryTitle.Title);
    }
    private Tasks MakeCopy(Entities edc)
    {
      string _Cmnts = String.Empty;
      int _idx = 0;
      foreach (Linq.TaskComments _cx in TaskComments)
      {
        _Cmnts += _cx.Body;
        if (_idx++ > 3)
          break;
      }
      Status _newStae = Status.GetStatus(edc, Status.StatusValues.New);
      Tasks _newTask = new Tasks()
      {
        Active = _newStae.Active,
        BaselineEnd = this.BaselineEnd,
        BaselineStart = this.BaselineStart,
        Body = Body + String.Format("<div><p>copy from milestone {0}.</p></div><div>{1}</div>", this.Task2MilestoneResolvedInTitle.Title, _Cmnts),
        Task2CategoryTitle = this.Task2CategoryTitle,
        Task2MilestoneDefinedInTitle = this.Task2MilestoneDefinedInTitle,
        Task2MilestoneResolvedInTitle = null, // assigned in Connecr2Target
        Task2ProjectTitle = null, // assigned in Connecr2Target
        Task2RequirementsTitle = null, // assigned in Connecr2Target
        Task2ResourcesTitle = this.Task2ResourcesTitle,
        Task2SPriorityTitle = this.Task2SPriorityTitle,
        Task2SResolutionTitle = this.Task2SResolutionTitle,
        Task2StatusTitle = _newStae,
        Task2TypeTitle = this.Task2TypeTitle,
        TaskEnd = DateTime.Today,
        TaskStart = DateTime.Today,
        Title = this.Title
      };
      edc.Task.InsertOnSubmit(_newTask);
      this.Task2SResolutionTitle = Element.GetAtIndex<Resolution>(edc.Resolution, c_ResolutionFixLater);
      this.Task2StatusTitle = Status.GetStatus(edc, Status.StatusValues.Resolved);
      this.Active = this.Task2StatusTitle.Active.Value;
      return _newTask;
    }
    //TODO not used 
    //private void Adjust()
    //{
    //  if (this.Task2MilestoneResolvedInTitle == null || this.Task2MilestoneResolvedInTitle != this.Task2RequirementsTitle.Requirements2MilestoneTitle)
    //    this.Task2MilestoneResolvedInTitle = this.Task2RequirementsTitle.Requirements2MilestoneTitle;
    //  if (this.Task2ProjectTitle == null || this.Task2ProjectTitle != this.Task2RequirementsTitle.Requirements2ProjectsTitle)
    //    this.Task2ProjectTitle = this.Task2RequirementsTitle.Requirements2ProjectsTitle;
    //  AdjustActive();
    //}
    private void AdjustActive(ref bool allInactive)
    {
      AdjustActive();
      if (Active.Value)
        allInactive = false;
    }
    private void AdjustActive()
    {
      if (this.Task2StatusTitle == null)
        throw new ArgumentNullException("Task2StatusTitle", "The task has to have assigned status.");
      if (!this.Task2StatusTitle.Active.HasValue)
        throw new ArgumentOutOfRangeException("Task2StatusTitle.Active", "The status has to have assigned active bit.");
      if (Active.HasValue && this.Task2StatusTitle.Active.Value == Active.Value)
        return;
      Active = this.Task2StatusTitle.Active.Value;
    }
    #endregion


    #region IComparable<Tasks> Members

    /// <summary>
    /// Compares the current object with another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    /// A value that indicates the relative order of the objects being compared. The return value has the following meanings: 
    /// Value Meaning:
    /// Less than zero This object is less than the <paramref name="other" /> parameter.
    /// Zero This object is equal to <paramref name="other" />. 
    /// Greater than zero This object is greater than <paramref name="other" />.
    /// </returns>
    public int CompareTo(Tasks other)
    {
      string _xRequirementTitle = this.Task2RequirementsTitle == null ? " " : this.Task2RequirementsTitle.Title;
      string _yRequirementTitle = other.Task2RequirementsTitle == null ? " " : other.Task2RequirementsTitle.Title;
      int _result = _xRequirementTitle.CompareTo(_yRequirementTitle);
      if (_result == 0)
        _result = this.Title.CompareTo(other.Title);
      return _result;
    }

    #endregion
  }
}
