using System;
using System.ComponentModel;
using System.Web.UI;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;
using CAS.AgileWorkloadTracker.Dashboards.Webparts.CurrentUserWebPart;
using System.Globalization;

namespace CAS.AgileWorkloadTracker.Dashboards.Webparts.CurrentUserWebPart
{
  [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces" ), ToolboxItemAttribute( false )]
  [CLSCompliant(false)]
  public class CurrentUserWebPart : WebPart
  {

    #region private
    // Visual Studio might automatically update this path when you change the Visual Web Part project item.
    private const string _ascxPath = @"~/_CONTROLTEMPLATES/CAS.AgileWorkloadTracker.Dashboards/CurrentUserWebPart/CurrentUserWebPartUserControl.ascx";
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes" )]
    protected override void CreateChildControls()
    {
      string _phase = "Starting";
      try
      {
        Control _ctrl = Page.LoadControl(_ascxPath);
        _phase = "After Page.Load";
        m_Control = (CurrentUserWebPartUserControl)_ctrl;
        _phase = "After cast";
        _phase = "After DisplayUserName";
        Controls.Add(m_Control);
        _phase = "Finishing";
      }
      catch (Exception ex)
      {
        string _frmt = "Cannot lod the user control at: {0} because : {1}";
        Controls.Add(new LiteralControl(String.Format(CultureInfo.InvariantCulture, _frmt, _phase, ex.Message)));
      }
    }
    /// <summary>
    /// The event handler for the System.Web.UI.Control.PreRender event that occurs immediately before the Web Part is rendered to the Web Part Page it is contained on.
    /// </summary>
    /// <param name="e">A System.EventArgs that contains the event data.</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes" )]
    protected override void OnPreRender(EventArgs e)
    {
      //if (m_UserDescriptor == null)
      try
      {
        m_Control.DisplayUserName(GetUserDescriptor());
      }
      catch (Exception ex)
      {
        string _frmt = "Cannot execute OnPreRender because: {0}";
        Controls.Add( new LiteralControl( String.Format( CultureInfo.InvariantCulture, _frmt, ex.Message ) ) );
      } 
      base.OnLoad(e);
    }
    private CurrentUserWebPartUserControl m_Control;
    private static UserDescriptor GetUserDescriptor()
    {
      //!!!the using does not work in this context.
      //using (SPWeb _sw = SPContext.Current.Web)
      //{ }
      return new UserDescriptor(SPContext.Current.Web.CurrentUser);
    }
    #endregion

    #region public
    [System.Web.UI.WebControls.WebParts.ConnectionProvider("Current User", "CurrentUserProviderPoint", AllowsMultipleConnections = true)]
    public System.Web.UI.WebControls.WebParts.IWebPartRow GetConnectionInterface()
    {
      return GetUserDescriptor();
    }
    public CurrentUserWebPart()
      : base()
    {
      //m_UserDescriptor = GetUserDescriptor();
    }
    internal static string CurrentUserProviderPoint = "CurrentUserProviderPoint";
    #endregion
  }
}
