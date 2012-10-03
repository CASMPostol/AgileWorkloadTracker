using System;
using CAS.ITRDataAccess.Properties;

namespace CAS.AgileWorkloadTracker.Linq
{
  internal partial class Entities
  {
    internal static Entities GetEntitity()
    {
      return new Entities( Settings.Default.RequestUrl );
    }
  }
  internal partial class Tasks
  {
    internal void Adjust()
    {
      if ( Task2MilestoneResolvedInTitle == null )
        return;
      Task2MilestoneResolvedInTitle.Adjust( this );
      Task2MilestoneDefinedInTitle.AdjustBaseline( this.BaselineStart.Value );
    }
    internal void Adjust( DateTime dateTime )
    {
      if ( !TaskStart.HasValue || TaskStart > dateTime )
        TaskStart = dateTime;
      if ( !TaskEnd.HasValue || TaskEnd < dateTime )
        TaskEnd = dateTime;
      Adjust();
    }
  }
  internal partial class Milestone
  {

    internal void AdjustBaseline( DateTime dateTime )
    {
      if ( !this.BaselineStart.HasValue || this.BaselineStart.Value > dateTime )
        this.BaselineStart = dateTime;
      if ( !this.BaselineEnd.HasValue || this.BaselineEnd.Value > dateTime )
        this.BaselineEnd = dateTime;
    }
    internal void Adjust( Tasks task )
    {
      if ( !MilestoneStart.HasValue || MilestoneStart.Value > task.TaskStart )
      {
        MilestoneStart = task.TaskStart;
        if ( this.Milestone2ProjectTitle != null )
          this.Milestone2ProjectTitle.Adjust( task.TaskStart.Value );
      }
      if ( !MilestoneEnd.HasValue || MilestoneEnd.Value < task.TaskEnd )
      {
        MilestoneEnd = task.TaskEnd;
        if ( this.Milestone2ProjectTitle != null )
          this.Milestone2ProjectTitle.Adjust( task.TaskEnd.Value );
      }
      if ( !this.BaselineStart.HasValue || this.BaselineStart.Value > task.BaselineStart )
        this.BaselineStart = task.BaselineStart;
      if ( !this.BaselineEnd.HasValue || this.BaselineEnd.Value > task.BaselineEnd )
        this.BaselineEnd = task.BaselineEnd;
    }
  }
  internal partial class Projects
  {
    internal void Adjust( DateTime dateTime )
    {
      if ( !ProjectStartDate.HasValue || ProjectStartDate.Value > dateTime )
        ProjectStartDate = dateTime;
      if ( !ProjectEndDate.HasValue || ProjectEndDate.Value < dateTime )
        ProjectEndDate = dateTime;
    }
  }
}
