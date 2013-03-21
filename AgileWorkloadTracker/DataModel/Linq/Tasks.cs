using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.AgileWorkloadTracker.DataModel.Linq
{
  partial class Tasks
  {
    internal void Adjust( Entities edc )
    {
      foreach ( Workload _wix in from _ix in this.Workload where _ix.Workload2ProjectTitle.Identyfikator != this.Task2ProjectTitle.Identyfikator select _ix )
        _wix.Workload2ProjectTitle = this.Task2ProjectTitle;
    }
  }
}
