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
    public void ForceMakeInactive(Milestone target)
    {
      if (target == null)
        throw new ArgumentNullException("target");
      Dictionary<int, DataModel.Linq.Requirements> m_Requirements = new Dictionary<int, Requirements>();
      foreach (DataModel.Linq.Tasks _tskx in Tasks0)
      {
        Requirements _target = null;
        if (m_Requirements.ContainsKey(_tskx.Task2RequirementsTitle.Identyfikator.Value))
          _target = m_Requirements[_tskx.Task2RequirementsTitle.Identyfikator.Value];
        else
        {
          _target = _tskx.Task2RequirementsTitle.MakeCopy(target);
          m_Requirements.Add(_tskx.Task2RequirementsTitle.Identyfikator.Value, _target);
        }
        _tskx.MoveToTarget(_target);
      }
    }
  }
}
