using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
//<summary>
//  Title   : partial class App : Application
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
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System;
using System.Windows;

namespace CAS.AgileWorkloadTracker.SiteManagement
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);
#if (DEBUG)
      RunInDebugMode();
#else
            RunInReleaseMode();
#endif
      this.ShutdownMode = ShutdownMode.OnMainWindowClose;
    }
    private static void RunInDebugMode()
    {
      ApplicationIBootstrapper bootstrapper = new ApplicationIBootstrapper();
      bootstrapper.Run();
    }
    private static void RunInReleaseMode()
    {
      AppDomain.CurrentDomain.UnhandledException += AppDomainUnhandledException;
      try
      {
        ApplicationIBootstrapper bootstrapper = new ApplicationIBootstrapper();
        bootstrapper.Run();
      }
      catch (Exception ex)
      {
        HandleException(ex);
      }
    }
    private static void AppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
      HandleException(e.ExceptionObject as Exception);
    }
    private static void HandleException(Exception ex)
    {
      if (ex == null)
        return;
      ExceptionPolicy.HandleException(ex, "Default Policy");
      MessageBox.Show(CAS.AgileWorkloadTracker.SiteManagement.Properties.Resources.UnhandledException, "Application Error", MessageBoxButton.OK, MessageBoxImage.Stop);
      Environment.Exit(1);
    }
  }
}
