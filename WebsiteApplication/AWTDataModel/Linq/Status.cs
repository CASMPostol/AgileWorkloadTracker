//<summary>
//  Title   : partial class Status
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

using CAS.SharePoint.Linq;

namespace CAS.AgileWorkloadTracker.DataModel.Linq
{
  /// <summary>
  /// Entity partial class Status
  /// </summary>
  public partial class Status
  {
    internal enum StatusValues : int
    {
      Confirmed = 1,
      InProgress = 2,
      ReOpened = 3,
      Resolved = 4,
      Closed = 5,
      InWaiting = 6,
      New = 7
    }
    internal static Status GetStatus(Entities edc, StatusValues value)
    {
      return edc.Status.GetAtIndex<Status>((int)value);
    }
  }
}
