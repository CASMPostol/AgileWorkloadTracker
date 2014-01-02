using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.AgileWorkloadTracker.DataModel.Linq
{
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
      this.MilestoneHours = this.Requirements.Sum<Requirements>(a => a.CalculatedHours);
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
    }
  }
}
