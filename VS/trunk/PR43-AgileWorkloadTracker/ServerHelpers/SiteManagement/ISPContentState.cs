//<summary>
//  Title   : ISPContentState
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

namespace CAS.SmartFactory.Shepherd.Client.Management
{

  /// <summary>
  /// Interface ISPContentState - contains information about accessibility of the SharePoint website. 
  /// </summary>
  interface ISPContentState
  {

    /// <summary>
    /// Gets the URL of the SharePoint application website.
    /// </summary>
    /// <value>The URL.</value>
    string SharePointWebsiteURL { get; }
    /// <summary>
    /// Gets or sets a value indicating whether SharePoint could be connected using the <see cref="ISPContentState.SharePointWebsiteURL"/>
    /// </summary>
    /// <value><c>true</c> if [sp connected]; otherwise, <c>false</c>.</value>
    bool SPConnected { get; set; }

  }
}
