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
      
      m_projectTableAdapter.Fill( m_BugNETDataSet.Project );
      m_priorityTableAdapter.Fill( m_BugNETDataSet.Priority );
      m_versionTableAdapter.Fill( m_BugNETDataSet.Version );
      m_projectCustomFieldTypeTableAdapter.Fill( m_BugNETDataSet.ProjectCustomFieldType );
      m_projectCustomFieldsTableAdapter.Fill( m_BugNETDataSet.ProjectCustomFields );
      m_projectCustomFieldSelectionTableAdapter.Fill( m_BugNETDataSet.ProjectCustomFieldSelection );
      m_projectMailBoxTableAdapter.Fill( m_BugNETDataSet.ProjectMailBox );
      //TODO Roles if required.
      m_resolutionTableAdapter.Fill( m_BugNETDataSet.Resolution );
      m_statusTableAdapter.Fill( m_BugNETDataSet.Status );
      m_typeTableAdapter.Fill( m_BugNETDataSet.Type );
      m_BugTableAdapter.Fill( m_BugNETDataSet.Bug );
      m_bugAttachmentTableAdapter.Fill( m_BugNETDataSet.BugAttachment );
      m_bugCommentTableAdapter.Fill( m_BugNETDataSet.BugComment );
      m_bugHistoryTableAdapter.Fill( m_BugNETDataSet.BugHistory );
      m_bugNotificationTableAdapter.Fill( m_BugNETDataSet.BugNotification );
      m_bugTimeEntryTableAdapter.Fill( m_BugNETDataSet.BugTimeEntry );
      m_projectCustomFieldValuesTableAdapter.Fill( m_BugNETDataSet.ProjectCustomFieldValues );
      m_SharePointImporter.Import( m_BugNETDataSet );
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
