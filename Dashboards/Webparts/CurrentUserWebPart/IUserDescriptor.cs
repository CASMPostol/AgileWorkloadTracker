using System;
using Microsoft.SharePoint;
namespace CAS.AgileWorkloadTracker.Dashboards.CurrentUserWebPart
{
  interface IUserDescriptor
  {
    string Company { get; }
    string User { get; }
  }
}
