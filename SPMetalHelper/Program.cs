using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.SharePoint.SPMetalParameters;
using Microsoft.SharePoint;
using System.IO;
using System.Windows.Forms;

namespace CAS.AgileWorkloadTracker.SPMetalHelper
{
  class Program
  {
    static void Main(string[] args)
    {
      try
      {
        PRWeb _web = PRWeb.ImportDataModel(Properties.Settings.Default.URL);
        using (Stream _str = new FileInfo(Properties.Settings.Default.FileName).Create())
          PRWeb.ImportDocument(_str, _web);
        string _endText = String.Format("Operation finisched, the file is located at: {0}", Environment.CurrentDirectory);
        MessageBox.Show(_endText, "Operation succeeded", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
      catch (Exception ex)
      {
        string _errorText = String.Format("Operation failed with the error: {0}", ex.Message);
        MessageBox.Show(_errorText, "Operation failed",  MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
  }
}
