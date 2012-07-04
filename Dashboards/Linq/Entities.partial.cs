using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace CAS.AgileWorkloadTracker.Linq
{
  internal partial class Resources
  {
    internal static Resources FindForUser(Entities edc, SPUser _user)
    {
      if (edc.Resources == null)
        return null;
      return edc.Resources.FirstOrDefault(idx => String.IsNullOrEmpty(idx.EmployeeADAccount.Tytuł) ? false : idx.EmployeeADAccount.Tytuł.Contains(_user.Name));
    }
  }
}
