using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.AgileWorkloadTracker.Linq;

namespace CAS.AgileWorkloadTracker.Dashboards.Webparts
{
  internal enum ConnectionSelector
  {
    ProjectInterconnection,
    TaskInterconnection,
  }
  internal abstract class InterconnectionData<DerivedType>: CAS.SharePoint.Web.InterconnectionData<DerivedType>
      where DerivedType: InterconnectionData<DerivedType>
  {
    internal string ID { get { return GetFieldValue( Element.IDColunmName ); } }
    internal string Title { get { return GetFieldValue( Element.TitleColunmName ); } }
  }
  internal class ProjectInterconnectionData: InterconnectionData<ProjectInterconnectionData>
  {
    internal ProjectInterconnectionData()
      : base()
    { }
  }
  internal class TaskInterconnectionData: InterconnectionData<TaskInterconnectionData>
  {
    internal TaskInterconnectionData()
      : base()
    { }
  }
}
