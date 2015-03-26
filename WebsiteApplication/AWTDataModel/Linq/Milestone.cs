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
  partial class Milestone : IComparable<Milestone>
  {

    #region public API
    /// <summary>
    /// Adjusts the references of this instance and all related objects beneath. I creates a default <see cref="Requirements"/> object.
    /// </summary>
    /// <param name="edc">The current instance of <see cref="Entities"/></param>
    internal void Adjust(Entities edc)
    {
      List<Tasks> _danglingTasks = (from _tix in this.Tasks0 where _tix.Task2RequirementsTitle == null select _tix).ToList<Tasks>();
      if (_danglingTasks.Count > 0)
      {
        string _defMilestoneTitle = "DanglingTasks of " + this.Title;
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
    public void Update()
    {
      if (this.Milestone2ProjectTitle == null)
        throw new ArgumentNullException("Milestone2ProjectTitle");
      double _hours = 0;
      DateTime _start = DateTime.MaxValue;
      DateTime _end = DateTime.MinValue;
      bool _AllInactive = true;
      foreach (Requirements _rqrx in this.Requirements)
        _rqrx.Update(ref _hours, ref _start, ref _end, ref _AllInactive);
      this.MilestoneHours = _hours;
      if (_start != DateTime.MaxValue)
        this.MilestoneStart = _start;
      if (_end != DateTime.MinValue)
        this.MilestoneEnd = _end;
    }
    /// <summary>
    /// Calculates the workload.
    /// </summary>
    public void Update(Entities edc)
    {
      if (this.Milestone2ProjectTitle == null)
        throw new ArgumentNullException("Milestone2ProjectTitle");
      double _hours = 0;
      DateTime _start = DateTime.MaxValue;
      DateTime _end = DateTime.MinValue;
      bool _AllInactive = true;
      b_DefaulktRequiremenr = null;
      foreach (Tasks _tsx in Tasks0)
        _tsx.MakeConsistent(edc, CreateDefault);
      foreach (Requirements _rqrx in this.Requirements)
        _rqrx.Update(ref _hours, ref _start, ref _end, ref _AllInactive);
      this.MilestoneHours = _hours;
      if (_start != DateTime.MaxValue)
        this.MilestoneStart = _start;
      if (_end != DateTime.MinValue)
        this.MilestoneEnd = _end;
      if (this.Milestone2StageTitle == null)
        this.Milestone2StageTitle = this.Milestone2ProjectTitle.Project2StageTitle;
      if (_AllInactive)
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
      Update(edc);
    }
    /// <summary>
    /// Moves this instance of <see cref="Milestone"/> to a new project before the selected milestone <paramref name="target"/>.
    /// </summary>
    /// <param name="edc">The instance of <see cref="Entities"/>.</param>
    /// <param name="target">The target.</param>
    /// <exception cref="System.ArgumentNullException">target</exception>
    public void Move(Entities edc, Milestone target)
    {
      if (target == null)
        throw new ArgumentNullException("target", "Milestone.Move - the argument cannot be null.");
      this.SortOrder = target.SortOrder.GetValueOrDefault(-99999) - 50;
      if (this.Milestone2ProjectTitle == target.Milestone2ProjectTitle)
        return;
      this.Milestone2ProjectTitle = target.Milestone2ProjectTitle;
      this.Milestone2StageTitle = this.Milestone2ProjectTitle.Project2StageTitle;
      foreach (DataModel.Linq.Requirements _rqrx in this.Requirements)
        _rqrx.MakeConsistent(edc);
      Update(edc);
    }
    #endregion

    #region private
    private Requirements b_DefaulktRequiremenr = null;
    private Requirements CreateDefault(Entities edc)
    {
      if (b_DefaulktRequiremenr == null)
        b_DefaulktRequiremenr = Linq.Requirements.CreateDefault(edc, this);
      return b_DefaulktRequiremenr;
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
    #endregion

    #region IComparable<Milestone> Members
    /// <summary>
    /// Compares the current object with another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    /// A value that indicates the relative order of the objects being compared. The return value has the following meanings: 
    /// Value Meaning Less than zero This object is less than the <paramref name="other" /> parameter.
    /// Zero This object is equal to <paramref name="other" />. 
    /// Greater than zero This object is greater than <paramref name="other" />.
    /// </returns>
    public int CompareTo(Milestone other)
    {
      string _xProjectTitle = this.Milestone2ProjectTitle == null ? " " : this.Milestone2ProjectTitle.Title;
      string _yProjectTitle = other.Milestone2ProjectTitle == null ? " " : other.Milestone2ProjectTitle.Title;
      int _result = _xProjectTitle.CompareTo(_yProjectTitle);
      if (_result == 0)
        _result = this.SortOrder.GetValueOrDefault(-99999).CompareTo(other.SortOrder.GetValueOrDefault(-99999));
      if (_result == 0)
        _result = this.Title.CompareTo(other.Title);
      return _result;
    }
    #endregion
  }
}
