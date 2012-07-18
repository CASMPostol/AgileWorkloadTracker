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

      if ( Start > dateTime )
      {
        Start = dateTime;
        this.Task2MilestoneDefinedInTitle.Adjust( dateTime );
      }
      if ( Finish < dateTime )
      {
        Finish = dateTime;
        this.Task2MilestoneResolvedInTitle.Adjust( dateTime );
      }
    }
    //TODO  [AWT-3523] Add columns to the Milestone and Task 
    public DateTime Start { get; set; }
    public DateTime Finish { get; set; }
  }
  internal partial class Milestone: Element
  {
    //TODO  [AWT-3523] Add columns to the Milestone and Task 
    public DateTime Start { get; set; }
    public DateTime Finish { get; set; }
    internal void Adjust( DateTime dateTime )
    {
      if ( Start > dateTime )
        Start = dateTime;
      if ( Finish < dateTime )
        Finish = dateTime;
    }
  }
}
