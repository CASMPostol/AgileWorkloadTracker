using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Collections.Generic;

namespace CAS.AgileWorkloadTracker.Dashboards.Webparts.TaskManagement
{
  [ToolboxItemAttribute( false )]
  public class TaskManagement: WebPart
  {
    // Visual Studio might automatically update this path when you change the Visual Web Part project item.
    private const string _ascxPath = @"~/_CONTROLTEMPLATES/CAS.AgileWorkloadTracker.Dashboards.Webparts/TaskManagement/TaskManagementUserControl.ascx";
    private TaskManagementUserControl m_Control;
    protected override void CreateChildControls()
    {
      m_Control = Page.LoadControl( _ascxPath ) as TaskManagementUserControl;
      Controls.Add( m_Control );
    }
    protected override void OnPreRender( EventArgs e )
    {
      m_Control.SetInterconnectionData( m_ProvidersDictionary );
      base.OnPreRender( e );
    }
    #region Interconnections Providers
    /// <summary>
    /// Sets the BatchInterconnection provider.
    /// </summary>
    /// <param name="_provider">The provider interface.</param>
    [ConnectionConsumer( "Project list interconnection", "ProjectInterconnection", AllowsMultipleConnections = false )]
    public void SetProjectProvider( IWebPartRow _provider )
    {
      m_ProvidersDictionary.Add( ConnectionSelector.ProjectInterconnection, _provider );
    }
    /// <summary>
    /// Sets the InvoiceContentInterconnection provider.
    /// </summary>
    /// <param name="_provider">The provider interface.</param>
    [ConnectionConsumer( "Task list interconnection", "TaskInterconnection", AllowsMultipleConnections = false )]
    public void SetInvoiceContentProvider( IWebPartRow _provider )
    {
      m_ProvidersDictionary.Add( ConnectionSelector.TaskInterconnection, _provider );
    }
    private Dictionary<ConnectionSelector, IWebPartRow> m_ProvidersDictionary = new Dictionary<ConnectionSelector, IWebPartRow>();
    #endregion
  }
}
