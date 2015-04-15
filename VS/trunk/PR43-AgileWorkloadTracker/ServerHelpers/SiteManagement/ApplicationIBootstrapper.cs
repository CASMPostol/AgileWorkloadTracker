//<summary>
//  Title   : ApplicationIBootstrapper
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

using CAS.AgileWorkloadTracker.SiteManagement.Infrastructure.Behaviors;
using Microsoft.Practices.Prism.MefExtensions;
using Microsoft.Practices.Prism.Regions;
using System.ComponentModel.Composition.Hosting;
using System.Windows;

namespace CAS.AgileWorkloadTracker.SiteManagement
{
  internal partial class ApplicationIBootstrapper : MefBootstrapper
  {

    protected override void ConfigureAggregateCatalog()
    {
      this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(ApplicationIBootstrapper).Assembly));
    }

    protected override void ConfigureContainer()
    {
      base.ConfigureContainer();
    }

    protected override void InitializeShell()
    {
      base.InitializeShell();
      Application.Current.MainWindow = (Shell)this.Shell;
      Application.Current.MainWindow.Show();
    }

    protected override Microsoft.Practices.Prism.Regions.IRegionBehaviorFactory ConfigureDefaultRegionBehaviors()
    {
      IRegionBehaviorFactory factory = base.ConfigureDefaultRegionBehaviors();
      factory.AddIfMissing("AutoPopulateExportedViewsBehavior", typeof(AutoPopulateExportedViewsBehavior));
      return factory;
    }

    protected override DependencyObject CreateShell()
    {
      return this.Container.GetExportedValue<Shell>();
    }
    public override void Run(bool runWithDefaultConfiguration)
    {
      base.Run(runWithDefaultConfiguration);
      ((Shell)Shell).ViewModel.RequestNavigate(Infrastructure.ViewNames.SetupStateName, null);
    }
  }
}
