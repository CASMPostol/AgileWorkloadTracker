//<summary>
//  Title   : ApplicationIBootstrapper - adopted from StockTraderRI
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

using CAS.AgileWorkloadTracker.SiteManagement.Services;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.MefExtensions;

namespace CAS.AgileWorkloadTracker.SiteManagement
{
  /// <summary>
  /// ApplicationIBootstrapper part that defines the logger 
  /// </summary>
  internal partial class ApplicationIBootstrapper
  {

    /// <summary>
    /// Create the <see cref="ILoggerFacade" /> used by the bootstrapper.
    /// </summary>
    /// <returns>ILoggerFacade.</returns>
    /// <remarks>The base implementation returns a new TextLogger.</remarks>
    protected override ILoggerFacade CreateLogger()
    {
      return NamedTraceLogger.Logger;
    }

  }
}
