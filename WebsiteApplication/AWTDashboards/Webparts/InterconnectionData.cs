//_______________________________________________________________
//  Title   : ConnectionSelector
//  System  : Microsoft VisualStudio 2013 / C#
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL: $
//  $Id$
//
//  Copyright (C) 2015, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________

using CAS.SharePoint.Linq;

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
    internal string ID { get { return GetFieldValue( SPLinqExtensions.IDColumnName ); } }
    internal string Title { get { return GetFieldValue(SPLinqExtensions.TitleColumnName); } }
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
