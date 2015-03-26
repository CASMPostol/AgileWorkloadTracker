using System;
using System.Web.UI;

namespace CAS.AgileWorkloadTracker.Dashboards.Webparts.CurrentUserWebPart
{
  public partial class CurrentUserWebPartUserControl : UserControl
  {
    protected void Page_Load(object sender, EventArgs e)
    {}
    internal void DisplayUserName(IUserDescriptor _UserDescriptor)
    {
      if (_UserDescriptor != null)
        this.m_UserNameLiteral.Text = String.Format("Welcome {0} from {1} !", _UserDescriptor.User, _UserDescriptor.Company);
      else
        this.m_UserNameLiteral.Text = "Information about user is unavailable";
    }
  }
}
