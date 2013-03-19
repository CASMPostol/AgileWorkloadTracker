using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;

namespace CAS.AgileWorkloadTracker.Website0110.Features.Upgrade
{
  /// <summary>
  /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
  /// </summary>
  /// <remarks>
  /// The GUID attached to this class may be used during packaging and should not be modified.
  /// </remarks>

  [Guid( "2e31ec30-d4b5-443f-bd1a-e6a376023fa8" )]
  public class UpgradeEventReceiver: SPFeatureReceiver
  {
    // Uncomment the method below to handle the event raised after a feature has been activated.

    public override void FeatureActivated( SPFeatureReceiverProperties properties )
    {
      SPSite site = properties.Feature.Parent as SPSite;
      if ( site != null )
      {
        SPWeb web = site.RootWeb;

        // Check to see if the list already exists
        //try
        //{
          SPContentType _ct = web.ContentTypes[ "Contact" ];
          SPField _fld = web.Fields[ new Guid( "7662cd2c-f069-4dba-9e35-082cf976e170" ) ];
          _ct.Fields.Add( _fld );
          // SPList targetList = web.Lists[ "Sample List" ];
        //}
        //catch ( ArgumentException )
        //{
        //  // The list does not exist, thus you can create it
        //  Guid listId = web.Lists.Add( "Sample List", "Sample List for SampleWebPart", SPListTemplateType.Events );
        //  SPList list = web.Lists[ listId ];
        //  list.OnQuickLaunch = true;
        //  list.Update();
        //}
      }

    }


    // Uncomment the method below to handle the event raised before a feature is deactivated.

    //public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
    //{
    //}


    // Uncomment the method below to handle the event raised after a feature has been installed.

    //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
    //{
    //}


    // Uncomment the method below to handle the event raised before a feature is uninstalled.

    //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
    //{
    //}

    // Uncomment the method below to handle the event raised when a feature is upgrading.

    //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
    //{
    //}
  }
}
