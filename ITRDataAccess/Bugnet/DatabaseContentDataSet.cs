using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.ITRDataAccess.Bugnet.DatabaseContentTableAdapters;
using System.Diagnostics;

namespace CAS.ITRDataAccess.Bugnet
{
  [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA2240:ImplementISerializableCorrectly" )]
  public partial class DatabaseContentDataSet
  {
    BugTableAdapter m_BugTableAdapter = new BugTableAdapter();
    internal void GetData()
    {
      this.EnforceConstraints = false;
      int _lenght = m_BugTableAdapter.Fill( this.Bug );
      Debug.Assert( _lenght > 0 );
    }
  }
}
