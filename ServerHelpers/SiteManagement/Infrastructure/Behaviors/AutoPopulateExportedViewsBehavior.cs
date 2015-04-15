//<summary>
//  Title   : AutoPopulateExportedViewsBehavior
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

using Microsoft.Practices.Prism.Regions;
using System;
using System.ComponentModel.Composition;

namespace CAS.AgileWorkloadTracker.SiteManagement.Infrastructure.Behaviors
{
  [Export(typeof(AutoPopulateExportedViewsBehavior))]
  [PartCreationPolicy(CreationPolicy.NonShared)]
  public class AutoPopulateExportedViewsBehavior : RegionBehavior, IPartImportsSatisfiedNotification
  {
    protected override void OnAttach()
    {
      AddRegisteredViews();
    }

    public void OnImportsSatisfied()
    {
      AddRegisteredViews();
    }

    private void AddRegisteredViews()
    {
      if (this.Region != null)
      {
        foreach (Lazy<object, IViewRegionRegistration> viewEntry in this.RegisteredViews)
        {
          if (viewEntry.Metadata.RegionName == this.Region.Name)
          {
            var view = viewEntry.Value;

            if (!this.Region.Views.Contains(view))
            {
              this.Region.Add(view);
            }
          }
        }
      }
    }

    /// <summary>
    /// Gets or sets the registered views.
    /// </summary>
    /// <value>
    /// The registered views.
    /// </value>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "MEF injected values")]
    [ImportMany(AllowRecomposition = true)]
    public Lazy<object, IViewRegionRegistration>[] RegisteredViews { get; set; }
  }
}
