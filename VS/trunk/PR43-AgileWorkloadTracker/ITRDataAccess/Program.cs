using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.ITRDataAccess
{
  class Program
  {
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "args" )]
    static void Main( string[] args )
    {
      using ( Bugnet.Import2SharePoint _BugNET2SharePoint = new Bugnet.Import2SharePoint() )
      {
        //_BugNET2SharePoint.GetDataFromDatabase();
      }
      using (TimeTracking.ImportFromTimeTracking _importFromTimeTracking = new TimeTracking.ImportFromTimeTracking())
      {
        _importFromTimeTracking.GetDataFromDatabase();
      }
    }
  }
}
