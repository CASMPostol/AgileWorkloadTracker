using System;
using Microsoft.SharePoint;
namespace CAS.AgileWorkloadTracker.Dashboards.Webparts.CurrentUserWebPart
{
  interface IUserDescriptor
  {
    string Company { get; }
    string User { get; }
  }
}
