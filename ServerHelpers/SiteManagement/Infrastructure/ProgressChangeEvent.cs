//<summary>
//  Title   : ProgressChangeEvent
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
      
using Microsoft.Practices.Prism.PubSubEvents;
using System.ComponentModel;

/// <summary>
/// The Infrastructure namespace.
/// </summary>
namespace CAS.AgileWorkloadTracker.SiteManagement.Infrastructure
{
  /// <summary>
  /// Class ProgressChangeEvent <see cref="PubSubEvent"/> that allows to distribute <see cref="PropertyChangedEventArgs"/>.
  /// </summary>
  internal class ProgressChangeEvent : PubSubEvent<ProgressChangedEventArgs>
  {
  }
}
