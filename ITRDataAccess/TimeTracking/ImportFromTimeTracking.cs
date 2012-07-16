using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CAS.ITRDataAccess.TimeTracking
{
  public partial class ImportFromTimeTracking : Component
  {
    public ImportFromTimeTracking()
    {
      InitializeComponent();
    }
    public ImportFromTimeTracking(IContainer container)
    {
      if (container == null)
        throw new ArgumentNullException("container cannot be null");
      container.Add(this);
      InitializeComponent();
    }
    internal void GetDataFromDatabase()
    {
      GetData();
      //m_SharePointImporter.Import(m_timeTrackingDataSet);
      Console.WriteLine("Data read from Godziny databaze");
    }
    private void GetData()
    {
      m_rodzajpracyTableAdapter.Fill( m_timeTrackingDataSet.RODZAJPRACY );
      m_statusyTableAdapter.Fill( m_timeTrackingDataSet.STATUSY );
      m_kategorieTableAdapter.Fill( m_timeTrackingDataSet.KATEGORIE );
      m_podkategorieTableAdapter.Fill( m_timeTrackingDataSet.PODKATEGORIE );
      m_pracownicyTableAdapter.Fill( m_timeTrackingDataSet.PRACOWNICY );
      m_kontrahenciTableAdapter.Fill( m_timeTrackingDataSet.KONTRAHENCI );
      m_umowyTableAdapter.Fill( m_timeTrackingDataSet.UMOWY );
      m_platnosciTableAdapter.Fill( m_timeTrackingDataSet.PLATNOSCI );
      m_polisyTableAdapter.Fill( m_timeTrackingDataSet.POLISY );
      m_projektyTableAdapter.Fill( m_timeTrackingDataSet.PROJEKTY );
      m_godzinyTableAdapter.Fill( m_timeTrackingDataSet.GODZINY );
      m_planTableAdapter.Fill( m_timeTrackingDataSet.PLAN );
    }
  }
}
