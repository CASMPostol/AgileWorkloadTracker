//<summary>
//  Title   : ShellViewModel
//  System  : Microsoft VisualStudio 2013 / C#
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

using CAS.AgileWorkloadTracker.SiteManagement.Infrastructure;
using CAS.Common.ViewModel.Wizard;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Reflection;

namespace CAS.AgileWorkloadTracker.SiteManagement
{
  /// <summary>
  /// Class ShellViewModel.
  /// </summary>
  [Export]
  [PartCreationPolicy(CreationPolicy.Shared)]
  public class ShellViewModel : StateMachineContext
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ShellViewModel"/> class.
    /// </summary>
    /// <param name="regionManager">The region manager service.</param>
    /// <param name="eventAggregator">The event aggregator service.</param>
    /// <exception cref="System.ArgumentNullException">
    /// watchListService
    /// or
    /// eventAggregator
    /// </exception>
    [ImportingConstructor]
    public ShellViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, ILoggerFacade loggingService)
    {
      AssemblyName _name = Assembly.GetExecutingAssembly().GetName();
      this.Title = CAS.AgileWorkloadTracker.SiteManagement.Properties.Resources.ApplicationName + " Rel " + _name.Version.ToString(3);
      if (regionManager == null)
      {
        m_LoggingService.Log(String.Format("ShellViewModel: exception {0}", "ArgumentNullException(\"watchListService\")"), Category.Debug, Priority.Low);
        throw new ArgumentNullException("watchListService");
      }
      m_RegionManager = regionManager;
      if (eventAggregator == null)
      {
        m_LoggingService.Log(String.Format("ShellViewModel: exception {0}", "ArgumentNullException(\"eventAggregator\")"), Category.Debug, Priority.Low);
        throw new ArgumentNullException("eventAggregator");
      }
      m_EventAggregator = eventAggregator;
      m_LoggingService = loggingService;
      m_LoggingService.Log("Entered ShellViewModel state machine context", Category.Info, Priority.Low);
    }
    public string Title
    {
      get
      {
        return b_Title;
      }
      set
      {
        RaiseHandler<string>(value, ref b_Title, "Title", this);
      }
    }
    /// <summary>
    /// Is called by the event handler of the <see cref="BackgroundWorker.ProgressChanged" />.
    /// </summary>
    /// <param name="activationMachine">The activation machine.</param>
    /// <param name="entitiesState">The <see cref="ProgressChangedEventArgs" /> instance containing the event data.</param>
    public override void ProgressChang(IAbstractMachineState activationMachine, ProgressChangedEventArgs entitiesState)
    {
      base.ProgressChang(activationMachine, entitiesState);
      m_EventAggregator.GetEvent<ProgressChangeEvent>().Publish(entitiesState);
    }
    /// <summary>
    ///  Navigates the specified region manager.
    /// </summary>
    /// <param name="url">The URL of the content to display.</param>
    /// <param name="parameter">An instance of <see cref="NavigationParameters"/>, which holds a collection of object parameters.</param>
    internal void RequestNavigate(string url, NavigationParameters parameter)
    {
      m_LoggingService.Log(String.Format("RequestNavigate to {0}/{1}", url, parameter == null ? String.Empty : parameter.ToString()), Category.Debug, Priority.Low);
      if (parameter == null)
        m_RegionManager.RequestNavigate(RegionNames.ActionRegion, new Uri(url, UriKind.Relative));
      else
        m_RegionManager.RequestNavigate(RegionNames.ActionRegion, new Uri(url, UriKind.Relative), parameter);
    }
    /// <summary>
    /// Reports state name change.
    /// </summary>
    /// <param name="machineStateName">Current name of the machine state.</param>
    protected override void StateNameProgressChang(string machineStateName)
    {
      m_LoggingService.Log(String.Format("StateNameProgressChang: new machine state {0}", machineStateName), Category.Debug, Priority.Low);
      m_EventAggregator.GetEvent<MachineStateNameEvent>().Publish(machineStateName);
    }
    private IRegionManager m_RegionManager = null;
    private IEventAggregator m_EventAggregator = null;
    private ILoggerFacade m_LoggingService;
    private string b_Title;   
  }
}
