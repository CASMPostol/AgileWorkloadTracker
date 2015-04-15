//<summary>
//  Title   : ViewExportAttribute
//  System  : Microsoft VisulaStudio 2013 / C#
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.ComponentModel.Composition;

namespace CAS.AgileWorkloadTracker.SiteManagement.Infrastructure.Behaviors
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  [MetadataAttribute]
  public sealed class ViewExportAttribute : ExportAttribute, IViewRegionRegistration
  {
    public ViewExportAttribute()
      : base(typeof(object))
    { }

    public ViewExportAttribute(string viewName)
      : base(viewName, typeof(object))
    { }

    public string ViewName { get { return base.ContractName; } }

    public string RegionName { get; set; }
  }
}
