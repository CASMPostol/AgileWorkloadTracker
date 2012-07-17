using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.ITRDataAccess.SharePoint;

namespace CAS.ITRDataAccess
{
  class Program
  {
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "args" )]
    static void Main( string[] args )
    {
      using ( Importer _Importer = new Importer() )
        _Importer.ImportData();
    }
  }
}
