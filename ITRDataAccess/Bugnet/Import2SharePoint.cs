using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CAS.ITRDataAccess.Bugnet
{
  internal partial class Import2SharePoint: Component
  {
    public Import2SharePoint()
    {
      InitializeComponent();
    }
    internal void GetDataFromDatabase()
    {
      m_BugNETDataSet.GetData();
    }
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode" )]
    public Import2SharePoint( IContainer container )
      : this()
    {
      if ( container == null )
        throw new ArgumentException( "The container parameter cant be nul" );
      container.Add( this );
    }
  }
}
