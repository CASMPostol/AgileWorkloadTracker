using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.AgileWorkloadTracker.DataModel.Linq
{
  partial class Projects
  {
    /// <summary>
    /// Adjusts the project.
    /// </summary>
    /// <param name="edc">The edc.</param>
    public void Adjust( Entities edc )
    {
      List<Tasks> _danglingTasks = ( from _tix in this.Tasks where _tix.Task2MilestoneResolvedInTitle == null select _tix ).ToList<Tasks>();
      if ( _danglingTasks.Count > 0 )
      {
        string _defMilestoneTitle = "DanglingTasks";
        Milestone _defM = ( from _ix in this.Milestone where _ix.Title.Contains( _defMilestoneTitle ) select _ix ).FirstOrDefault<Milestone>();
        if ( _defM == null )
        {
          _defM = new Milestone()
          {
            Active = true,
            BaselineEnd = DateTime.Today,
            BaselineStart = DateTime.Today,
            Default = false,
            Milestone2ProjectTitle = this,
            MilestoneEnd = DateTime.Today,
            MilestoneHours = 0,
            MilestoneStart = DateTime.Today,
            Title = _defMilestoneTitle,
          };
          edc.Milestone.InsertOnSubmit( _defM );
        }
        foreach ( Tasks _task in _danglingTasks )
          _task.Task2MilestoneResolvedInTitle = _defM;
      }
      edc.SubmitChanges();
      foreach ( Milestone _mix in this.Milestone )
        _mix.Adjust( edc );
    }
  }
}
