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
    internal void Adjust( DateTime dateTime )
    {
      if ( !TaskStart.HasValue || TaskStart > dateTime )
      {
        TaskStart = dateTime;
        if ( this.Task2MilestoneDefinedInTitle != null )
          this.Task2MilestoneDefinedInTitle.Adjust( dateTime );
      }
      if ( !TaskEnd.HasValue || TaskEnd < dateTime )
      {
        TaskEnd = dateTime;
        if ( this.Task2MilestoneResolvedInTitle != null )
          this.Task2MilestoneResolvedInTitle.Adjust( dateTime );
      }
    }
  }
  internal partial class Milestone
  {
    internal void Adjust( DateTime dateTime )
    {
      if ( !MilestoneStart.HasValue || MilestoneStart > dateTime )
      {
        MilestoneStart = dateTime;
        if ( this.Milestone2ProjectTitle == null )
          this.Milestone2ProjectTitle.Adjust( dateTime );
      }
      if ( !MilestoneEnd.HasValue || MilestoneEnd < dateTime )
      {
        MilestoneEnd = dateTime;
        if ( this.Milestone2ProjectTitle != null )
          this.Milestone2ProjectTitle.Adjust( dateTime );
      }
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
