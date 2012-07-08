using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace CAS.SharePoint.Web
{
  public static class ControlExtensions
  {
    /// <summary>
    /// Selects the specified _DDL.
    /// </summary>
    /// <param name="_ddl">The _DDL.</param>
    /// <param name="_row">The _row.</param>
    public static void Select(this DropDownList _ddl, int _row)
    {
      _ddl.SelectedIndex = -1;
      ListItem _li = _ddl.Items.FindByValue(_row.ToString());
      if (_li == null)
        throw new ApplicationException(String.Format("DropDownList does not contain ListItem with Value = {0}.", _row)) { Source = "ControlExtensions.Select" };
      _li.Selected = true;
    }
  }
}
