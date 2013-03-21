using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.AgileWorkloadTracker.DataModel.Linq
{
  partial class Requirements
  {
    internal void Adjust( Entities edc )
    {
      foreach ( Tasks _tix in this.Tasks )
      {
        if ( _tix.Task2MilestoneResolvedInTitle != this.Requirements2MilestoneTitle )
          _tix.Task2MilestoneResolvedInTitle = this.Requirements2MilestoneTitle;
        if ( _tix.Task2ProjectTitle != this.Requirements2ProjectsTitle )
          _tix.Task2ProjectTitle = this.Requirements2ProjectsTitle;
        _tix.Adjust( edc );
      };
    }

  }
}
