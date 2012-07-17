using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.ITRDataAccess.Bugnet;
using CAS.ITRDataAccess.Properties;
using Microsoft.SharePoint.Linq;
using System.Globalization;

namespace CAS.AgileWorkloadTracker.Linq
{
  internal partial class Entities
  {
    internal static Entities GetEntitity()
    {
      return new Entities( Settings.Default.RequestUrl );
    }
  }
}
