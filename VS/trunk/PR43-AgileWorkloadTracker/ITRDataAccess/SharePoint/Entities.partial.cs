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
      if ( !Start.HasValue || Start > dateTime )
      {
        Start = dateTime;
        if ( this.Task2MilestoneDefinedInTitle != null )
          this.Task2MilestoneDefinedInTitle.Adjust( dateTime );
      }
      if ( !Finish.HasValue || Finish < dateTime )
      {
        Finish = dateTime;
        if ( this.Task2MilestoneResolvedInTitle != null )
          this.Task2MilestoneResolvedInTitle.Adjust( dateTime );
      }
    }
    //TODO  [AWT-3523] Add columns to the Milestone and Task 
    public DateTime? Start { get; set; }
    public DateTime? Finish { get; set; }
  }
  internal partial class Milestone
  {
    //TODO  [AWT-3523] Add columns to the Milestone and Task 
    public DateTime? Start { get; set; }
    public DateTime? Finish { get; set; }
    internal void Adjust( DateTime dateTime )
    {
      if ( !Start.HasValue || Start > dateTime )
      {
        Start = dateTime;
        if ( this.Milestone2ProjectTitle == null )
          this.Milestone2ProjectTitle.Adjust( dateTime );
      }
      if ( !Finish.HasValue || Finish < dateTime )
      {
        Finish = dateTime;
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
