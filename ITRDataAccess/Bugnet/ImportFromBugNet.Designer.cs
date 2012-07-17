namespace CAS.ITRDataAccess.Bugnet
{
  internal partial class ImportFromBugNet
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose( bool disposing )
    {
      if ( disposing && ( components != null ) )
      {
        components.Dispose();
      }
      base.Dispose( disposing );
    }

    #region Component Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.m_BugNETDataSet = new CAS.ITRDataAccess.Bugnet.DatabaseContentDataSet();
      this.m_BugTableAdapter = new CAS.ITRDataAccess.Bugnet.DatabaseContentDataSetTableAdapters.BugTableAdapter();
      this.m_priorityTableAdapter = new CAS.ITRDataAccess.Bugnet.DatabaseContentDataSetTableAdapters.PriorityTableAdapter();
      this.m_projectTableAdapter = new CAS.ITRDataAccess.Bugnet.DatabaseContentDataSetTableAdapters.ProjectTableAdapter();
      this.m_versionTableAdapter = new CAS.ITRDataAccess.Bugnet.DatabaseContentDataSetTableAdapters.VersionTableAdapter();
      this.m_projectCustomFieldTypeTableAdapter = new CAS.ITRDataAccess.Bugnet.DatabaseContentDataSetTableAdapters.ProjectCustomFieldTypeTableAdapter();
      this.m_projectCustomFieldsTableAdapter = new CAS.ITRDataAccess.Bugnet.DatabaseContentDataSetTableAdapters.ProjectCustomFieldsTableAdapter();
      this.m_projectCustomFieldSelectionTableAdapter = new CAS.ITRDataAccess.Bugnet.DatabaseContentDataSetTableAdapters.ProjectCustomFieldSelectionTableAdapter();
      this.m_projectMailBoxTableAdapter = new CAS.ITRDataAccess.Bugnet.DatabaseContentDataSetTableAdapters.ProjectMailBoxTableAdapter();
      this.m_resolutionTableAdapter = new CAS.ITRDataAccess.Bugnet.DatabaseContentDataSetTableAdapters.ResolutionTableAdapter();
      this.m_statusTableAdapter = new CAS.ITRDataAccess.Bugnet.DatabaseContentDataSetTableAdapters.StatusTableAdapter();
      this.m_typeTableAdapter = new CAS.ITRDataAccess.Bugnet.DatabaseContentDataSetTableAdapters.TypeTableAdapter();
      this.m_bugAttachmentTableAdapter = new CAS.ITRDataAccess.Bugnet.DatabaseContentDataSetTableAdapters.BugAttachmentTableAdapter();
      this.m_bugCommentTableAdapter = new CAS.ITRDataAccess.Bugnet.DatabaseContentDataSetTableAdapters.BugCommentTableAdapter();
      this.m_bugHistoryTableAdapter = new CAS.ITRDataAccess.Bugnet.DatabaseContentDataSetTableAdapters.BugHistoryTableAdapter();
      this.m_bugNotificationTableAdapter = new CAS.ITRDataAccess.Bugnet.DatabaseContentDataSetTableAdapters.BugNotificationTableAdapter();
      this.m_bugTimeEntryTableAdapter = new CAS.ITRDataAccess.Bugnet.DatabaseContentDataSetTableAdapters.BugTimeEntryTableAdapter();
      this.m_projectCustomFieldValuesTableAdapter = new CAS.ITRDataAccess.Bugnet.DatabaseContentDataSetTableAdapters.ProjectCustomFieldValuesTableAdapter();
      this.m_relatedBugTableAdapter = new CAS.ITRDataAccess.Bugnet.DatabaseContentDataSetTableAdapters.RelatedBugTableAdapter();
      this.m_aspnet_UsersTableAdapter = new CAS.ITRDataAccess.Bugnet.DatabaseContentDataSetTableAdapters.aspnet_UsersTableAdapter();
      ((System.ComponentModel.ISupportInitialize)(this.m_BugNETDataSet)).BeginInit();
      // 
      // m_BugNETDataSet
      // 
      this.m_BugNETDataSet.DataSetName = "DatabaseContent";
      this.m_BugNETDataSet.Locale = new System.Globalization.CultureInfo("");
      this.m_BugNETDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
      // 
      // m_BugTableAdapter
      // 
      this.m_BugTableAdapter.ClearBeforeFill = true;
      // 
      // m_priorityTableAdapter
      // 
      this.m_priorityTableAdapter.ClearBeforeFill = true;
      // 
      // m_projectTableAdapter
      // 
      this.m_projectTableAdapter.ClearBeforeFill = true;
      // 
      // m_versionTableAdapter
      // 
      this.m_versionTableAdapter.ClearBeforeFill = true;
      // 
      // m_projectCustomFieldTypeTableAdapter
      // 
      this.m_projectCustomFieldTypeTableAdapter.ClearBeforeFill = true;
      // 
      // m_projectCustomFieldsTableAdapter
      // 
      this.m_projectCustomFieldsTableAdapter.ClearBeforeFill = true;
      // 
      // m_projectCustomFieldSelectionTableAdapter
      // 
      this.m_projectCustomFieldSelectionTableAdapter.ClearBeforeFill = true;
      // 
      // m_projectMailBoxTableAdapter
      // 
      this.m_projectMailBoxTableAdapter.ClearBeforeFill = true;
      // 
      // m_resolutionTableAdapter
      // 
      this.m_resolutionTableAdapter.ClearBeforeFill = true;
      // 
      // m_statusTableAdapter
      // 
      this.m_statusTableAdapter.ClearBeforeFill = true;
      // 
      // m_typeTableAdapter
      // 
      this.m_typeTableAdapter.ClearBeforeFill = true;
      // 
      // m_bugAttachmentTableAdapter
      // 
      this.m_bugAttachmentTableAdapter.ClearBeforeFill = true;
      // 
      // m_bugCommentTableAdapter
      // 
      this.m_bugCommentTableAdapter.ClearBeforeFill = true;
      // 
      // m_bugHistoryTableAdapter
      // 
      this.m_bugHistoryTableAdapter.ClearBeforeFill = true;
      // 
      // m_bugNotificationTableAdapter
      // 
      this.m_bugNotificationTableAdapter.ClearBeforeFill = true;
      // 
      // m_bugTimeEntryTableAdapter
      // 
      this.m_bugTimeEntryTableAdapter.ClearBeforeFill = true;
      // 
      // m_projectCustomFieldValuesTableAdapter
      // 
      this.m_projectCustomFieldValuesTableAdapter.ClearBeforeFill = true;
      // 
      // m_relatedBugTableAdapter
      // 
      this.m_relatedBugTableAdapter.ClearBeforeFill = true;
      // 
      // m_aspnet_UsersTableAdapter
      // 
      this.m_aspnet_UsersTableAdapter.ClearBeforeFill = true;
      ((System.ComponentModel.ISupportInitialize)(this.m_BugNETDataSet)).EndInit();

    }

    #endregion

    private DatabaseContentDataSetTableAdapters.BugTableAdapter m_BugTableAdapter;
    private DatabaseContentDataSetTableAdapters.PriorityTableAdapter m_priorityTableAdapter;
    private DatabaseContentDataSetTableAdapters.ProjectTableAdapter m_projectTableAdapter;
    private DatabaseContentDataSetTableAdapters.VersionTableAdapter m_versionTableAdapter;
    private DatabaseContentDataSetTableAdapters.ProjectCustomFieldTypeTableAdapter m_projectCustomFieldTypeTableAdapter;
    private DatabaseContentDataSetTableAdapters.ProjectCustomFieldsTableAdapter m_projectCustomFieldsTableAdapter;
    private DatabaseContentDataSetTableAdapters.ProjectCustomFieldSelectionTableAdapter m_projectCustomFieldSelectionTableAdapter;
    private DatabaseContentDataSetTableAdapters.ProjectMailBoxTableAdapter m_projectMailBoxTableAdapter;
    private DatabaseContentDataSetTableAdapters.ResolutionTableAdapter m_resolutionTableAdapter;
    private DatabaseContentDataSetTableAdapters.StatusTableAdapter m_statusTableAdapter;
    private DatabaseContentDataSetTableAdapters.TypeTableAdapter m_typeTableAdapter;
    private DatabaseContentDataSetTableAdapters.BugAttachmentTableAdapter m_bugAttachmentTableAdapter;
    private DatabaseContentDataSetTableAdapters.BugCommentTableAdapter m_bugCommentTableAdapter;
    private DatabaseContentDataSetTableAdapters.BugHistoryTableAdapter m_bugHistoryTableAdapter;
    private DatabaseContentDataSetTableAdapters.BugNotificationTableAdapter m_bugNotificationTableAdapter;
    private DatabaseContentDataSetTableAdapters.BugTimeEntryTableAdapter m_bugTimeEntryTableAdapter;
    private DatabaseContentDataSetTableAdapters.ProjectCustomFieldValuesTableAdapter m_projectCustomFieldValuesTableAdapter;
    private DatabaseContentDataSetTableAdapters.RelatedBugTableAdapter m_relatedBugTableAdapter;
    private DatabaseContentDataSet m_BugNETDataSet;
    private DatabaseContentDataSetTableAdapters.aspnet_UsersTableAdapter m_aspnet_UsersTableAdapter;
  }
}
