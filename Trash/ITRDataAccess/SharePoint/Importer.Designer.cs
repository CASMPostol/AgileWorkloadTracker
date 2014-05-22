namespace CAS.ITRDataAccess.SharePoint
{
  partial class Importer
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;


    #region Component Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      this.m_importFromTimeTracking = new CAS.ITRDataAccess.TimeTracking.ImportFromTimeTracking(this.components);
      this.m_importFromBugNet = new CAS.ITRDataAccess.Bugnet.ImportFromBugNet(this.components);

    }

    #endregion

    private TimeTracking.ImportFromTimeTracking m_importFromTimeTracking;
    private Bugnet.ImportFromBugNet m_importFromBugNet;

  }
}
