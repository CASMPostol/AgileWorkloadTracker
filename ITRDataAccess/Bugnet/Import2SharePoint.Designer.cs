namespace CAS.ITRDataAccess.Bugnet
{
  internal partial class Import2SharePoint
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
      ((System.ComponentModel.ISupportInitialize)(this.m_BugNETDataSet)).BeginInit();
      // 
      // m_BugNETDataSet
      // 
      this.m_BugNETDataSet.DataSetName = "DatabaseContent";
      this.m_BugNETDataSet.Locale = new System.Globalization.CultureInfo("");
      this.m_BugNETDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
      ((System.ComponentModel.ISupportInitialize)(this.m_BugNETDataSet)).EndInit();

    }

    #endregion

    private DatabaseContentDataSet m_BugNETDataSet;
  }
}
