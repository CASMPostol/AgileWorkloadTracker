//<summary>
//  Title   : static class DataModelExtensions
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
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
using System.Text;

namespace CAS.AgileWorkloadTracker.DataModel.Linq
{
  /// <summary>
  /// static class DataModelExtensions
  /// </summary>
  internal static class DataModelExtensions
  {
    internal static void UpdateWorkload(ref double hours, ref DateTime start, ref DateTime end, double myHours, DateTime myStart, DateTime myEnd)
    {
      hours += myHours;
      if (start > myStart)
        start = myStart;
      if (end < myEnd)
        end = myEnd;
    }
  }
}
