using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data;

namespace CAS.AgileWorkloadTracker.Dashboards.Webparts.WorkloadManagement
{
  public partial class WorkloadManagementUserControl : UserControl
  {
    //private const string m_keyCurrentYear = "CurrentYear";
    ////private Main main = new Main();
    //private int CurrentYear
    //{
    //  get
    //  {
    //    if (Session[m_keyCurrentYear] != null && Session[m_keyCurrentYear] is int)
    //      return ((int)Session[m_keyCurrentYear]);
    //    if (m_Calendar.SelectedDate.Year > 2000)
    //      return m_Calendar.SelectedDate.Year;
    //    return DateTime.Now.Year;
    //  }
    //  set
    //  {
    //    Session[m_keyCurrentYear] = value;
    //  }
    //}
    //protected string UserName;
    //protected decimal hours;
    //protected string txt;
    ///// <summary>
    ///// Method loading KATEGORIE Table rows to m_CategoryDropDownList
    ///// </summary>
    //protected void Kat_Load()
    //{
    //  //TODO get from list
    //  //foreach (GodzinySchema.KATEGORIERow row in main.godzinySchema.KATEGORIE)
    //  //  m_CategoryDropDownList.Items.Add(new Main.kategorieRowClass(row).ToString());
    //}
    ///// <summary>
    ///// Method loading first category row into m_CategoryDropDownList (default displayed value)
    ///// </summary>
    //protected void FirstKat_Load()
    //{
    //  GodzinySchema.KATEGORIERow katrow = main.godzinySchema.KATEGORIE.NewKATEGORIERow();
    //  katrow.NAZWA = Resources.Resources.text_sellect_category;
    //  katrow.ID = 0;
    //  katrow.OPIS = " ";
    //  this.main.godzinySchema.KATEGORIE.AddKATEGORIERow(katrow);
    //}
    ///// <summary>
    ///// Method loading PODKATEGORIE Table rows to DropDownList_SubCategory
    ///// </summary>
    //protected void Podkat_Load()
    //{
    //  foreach (GodzinySchema.PODKATEGORIERow row1 in main.godzinySchema.PODKATEGORIE)
    //    DropDownList_SubCategory.Items.Add(new Main.podkategorieRowClass(row1).ToString());
    //}
    ///// <summary>
    ///// Method loading first subcategory row into DropDownList_SubCategory (default displayed value)
    ///// </summary>
    //protected void FirstPodkat_Load()
    //{
    //  GodzinySchema.PODKATEGORIERow podkatrow = this.main.godzinySchema.PODKATEGORIE.NewPODKATEGORIERow();
    //  podkatrow.NAZWA = Resources.Resources.text_sellect_subcategory;
    //  podkatrow.ID = 0;
    //  podkatrow.OPIS = " ";
    //  podkatrow.ID_KATEGORII = 0;
    //  this.main.godzinySchema.PODKATEGORIE.AddPODKATEGORIERow(podkatrow);
    //}
    ///// <summary>
    ///// Method loading RODZAJPRACY Table rows to DropDownList_TypeOfWork
    ///// </summary>
    //protected void Rodz_Load()
    //{
    //  foreach (GodzinySchema.RODZAJPRACYRow row1 in main.godzinySchema.RODZAJPRACY)
    //    DropDownList_TypeOfWork.Items.Add(new Main.rodzajepracyRowClass(row1).ToString());
    //}
    ///// <summary>
    ///// Method loading first job row into DropDownList_TypeOfWork (default displayed value)
    ///// </summary>
    //protected void FirstRodz_Load()
    //{
    //  GodzinySchema.RODZAJPRACYRow rodzrow = this.main.godzinySchema.RODZAJPRACY.NewRODZAJPRACYRow();
    //  rodzrow.NAZWAPRACY = Resources.Resources.text_sellect_work_type;
    //  rodzrow.ID = 0;
    //  rodzrow.OPIS = " ";
    //  this.main.godzinySchema.RODZAJPRACY.AddRODZAJPRACYRow(rodzrow);
    //}
    ///// <summary>
    ///// Method loading PROJEKTY Table rows to DropDownList_Project
    ///// </summary>
    //protected void Proj_Load()
    //{
    //  foreach (GodzinySchema.PROJEKTYRow row2 in main.godzinySchema.PROJEKTY)
    //    DropDownList_Project.Items.Add(new Main.projektyRowClass(row2).ToString());
    //}
    ///// <summary>
    ///// Method loading first project row into DropDownList_Project (default displayed value)
    ///// </summary>
    //protected void FirstProj_Load()
    //{
    //  GodzinySchema.PROJEKTYRow projektyrow = this.main.godzinySchema.PROJEKTY.NewPROJEKTYRow();
    //  projektyrow.NAZWA = Resources.Resources.text_sellect_project;
    //  projektyrow.NAZWA_KROTKA = Resources.Resources.text_sellect_project;
    //  projektyrow.ID = 0;
    //  projektyrow.ID_UMOWY = 0;
    //  projektyrow.ROK = DateTime.Now.Year;
    //  projektyrow.LICZBA_GODZIN = 0;
    //  projektyrow.DATA_START = DateTime.Now;
    //  projektyrow.DATA_KONIEC = DateTime.Now;
    //  projektyrow.DATA_GWARANCJA = DateTime.Now;
    //  projektyrow.ID_KATEGORII = 0;
    //  projektyrow.ID_STATUSU = 0;
    //  projektyrow.ID_PODKATEGORII = 0;
    //  this.main.godzinySchema.PROJEKTY.AddPROJEKTYRow(projektyrow);
    //}
    ///// <summary>
    ///// Method loading KATEGORIE, PODKATEGORIE and PROJEKTY Table rows into DropDownLists 
    ///// and selecting first indexes of these DropDownLists
    ///// </summary>
    //protected void DropDownList_Load()
    //{
    //  m_CategoryDropDownList.Items.Clear();
    //  FirstKat_Load();
    //  Kat_Load();
    //  int numberKat = m_CategoryDropDownList.Items.Count;
    //  if (numberKat > 0)
    //    m_CategoryDropDownList.SelectedIndex = numberKat - 1;
    //  DropDownList_SubCategory.Items.Clear();
    //  FirstPodkat_Load();
    //  Podkat_Load();
    //  int numberPodkat = DropDownList_SubCategory.Items.Count;
    //  if (numberPodkat > 0)
    //    DropDownList_SubCategory.SelectedIndex = numberPodkat - 1;
    //  DropDownList_Project.Items.Clear();
    //  FirstProj_Load();
    //  Proj_Load();
    //  int numberProj = DropDownList_Project.Items.Count;
    //  if (numberProj > 0)
    //    DropDownList_Project.SelectedIndex = numberProj - 1;
    //}
    ///// <summary>
    ///// Checking GodzinySchema DataSet changes and updating GODZINY Table if changes exist
    ///// </summary>
    //public void CheckChanges()
    //{
    //  if (main.godzinySchema.HasChanges())
    //    main.godzinyTableadapter.Update(main.godzinySchema.GODZINY);
    //}
    ///// <summary>
    ///// Accepting GodzinySchema changes
    ///// </summary>
    //public void RefreshGodziny()
    //{
    //  CheckChanges();
    //  main.godzinySchema.AcceptChanges();
    //}
    ///// <summary>
    ///// Page Load method
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    //protected void Page_Load(object sender, EventArgs e)
    //{
    //  UserName = UserIdentityHelper.GetCurrentUser(Session);
    //  Label1.Text = main.pracownik(UserName).NAZWISKO_IMIE;
    //  string proj = String.Empty;
    //  string date = String.Empty;
    //  ObjectDataSource.SelectParameters[0].ConvertEmptyStringToNull = true;
    //  GridView1.EmptyDataText = "Brak godzin";
    //  PlanDataSource.SelectParameters[0].ConvertEmptyStringToNull = true;
    //  GridView2.EmptyDataText = "Brak planu";
    //  txt = "Naruszono więzy unikatowe, proszę wybrać inny rodzaj pracy albo zmodyfikować już istniejący rekord.";
    //  Label8.ForeColor = Color.Red;
    //  Label8.Font.Size = FontUnit.Large;
    //  main.kategorieTableadapter.Fill(main.godzinySchema.KATEGORIE);
    //  main.podkategorieTableadapter.Fill(main.godzinySchema.PODKATEGORIE);
    //  main.projektyTableadapter.Fill(main.godzinySchema.PROJEKTY, Convert.ToDecimal(CurrentYear));
    //  main.statusyTableadapter.Fill(main.godzinySchema.STATUSY);
    //  main.rodzajpracyTableadapter.Fill(main.godzinySchema.RODZAJPRACY);
    //  if (this.IsPostBack == false)
    //  {
    //    if (Request.Params["Proj"] != null)
    //    {
    //      proj = Request.Params["Proj"].ToString();
    //      string[] temp = proj.Split('/');
    //      date = temp[1];
    //      ObjectDataSource.SelectParameters[0].DefaultValue = date;
    //      Label16.Text = String.Format("Aktualnie masz wpisanych {0} godzin", main.sumadaily(UserName, date));
    //      m_Calendar.SelectedDate = Convert.ToDateTime(date);
    //      m_Calendar.VisibleDate = Convert.ToDateTime(date);
    //      if (temp[2] == "1")
    //        Label8.Text = txt;
    //      else
    //        Label8.Text = String.Empty;
    //    }
    //    else
    //    {
    //      ObjectDataSource.SelectParameters[0].DefaultValue = DateTime.Now.Date.ToString();
    //      Label16.Text = String.Format("Aktualnie masz wpisanych {0} godzin", main.sumadaily(UserName, DateTime.Now.Date.ToString()));
    //      m_Calendar.SelectedDate = DateTime.Now.Date;
    //      m_Calendar.VisibleDate = DateTime.Now.Date;
    //    }
    //    FirstKat_Load();
    //    Kat_Load();
    //    int numberKat = m_CategoryDropDownList.Items.Count;
    //    if (numberKat > 0)
    //      m_CategoryDropDownList.SelectedIndex = numberKat - 1;
    //    FirstPodkat_Load();
    //    Podkat_Load();
    //    int numberPodkat = DropDownList_SubCategory.Items.Count;
    //    if (numberPodkat > 0)
    //      DropDownList_SubCategory.SelectedIndex = numberPodkat - 1;
    //    FirstProj_Load();
    //    Proj_Load();
    //    int numberProj = DropDownList_Project.Items.Count;
    //    if (Request.Params["Proj"] != null)
    //    {
    //      proj = Request.Params["Proj"].ToString();
    //      string[] temp = proj.Split('/');
    //      if (numberProj > 0)
    //        DropDownList_Project.SelectedIndex = numberProj - 1;
    //      if (temp[0] != "")
    //      {
    //        proj = temp[0];
    //        DropDownList_Project.SelectedValue = proj;
    //        PlanDataSource.SelectParameters[0].DefaultValue = main.projekty(DropDownList_Project.SelectedValue, Convert.ToDecimal(CurrentYear)).ID.ToString();
    //        Label12.Text = String.Format("Aktualnie masz {0} godzin w tym projekcie",
    //          main.suma(main.projekty(DropDownList_Project.SelectedValue,
    //          Convert.ToDecimal(CurrentYear)).ID, UserName));
    //        Label14.Text = String.Format("Aktualnie masz zaplanowane {0} godzin w kategorii {1}",
    //          main.plankatsum(UserName, main.projekty(DropDownList_Project.SelectedValue,
    //          Convert.ToDecimal(CurrentYear)).ID_KATEGORII,
    //          Convert.ToDecimal(CurrentYear)),
    //          main.kategorie(main.projekty(DropDownList_Project.SelectedValue, Convert.ToDecimal(CurrentYear)).ID_KATEGORII).NAZWA);
    //        Label14.BackColor = Color.White;
    //        Label15.Text = String.Format("Aktualnie masz przepracowane {0} godzin w kategorii {1}", main.godzinykatsum(UserName, main.projekty(DropDownList_Project.SelectedValue, Convert.ToDecimal(CurrentYear)).ID_KATEGORII, Convert.ToDecimal(CurrentYear)), main.kategorie(main.projekty(DropDownList_Project.SelectedValue, Convert.ToDecimal(CurrentYear)).ID_KATEGORII).NAZWA);
    //        Label15.BackColor = Color.White;
    //      }
    //    }
    //    else
    //    {
    //      if (numberProj > 0)
    //        DropDownList_Project.SelectedIndex = numberProj - 1;
    //    }
    //    foreach (GodzinySchema.STATUSYRow row3 in main.godzinySchema.STATUSY)
    //      DropDownList_State.Items.Add(new Main.statusyRowClass(row3).ToString());
    //    if (DropDownList_State.Items.Count > 0)
    //      DropDownList_State.SelectedIndex = 2;
    //    FirstRodz_Load();
    //    Rodz_Load();
    //    int numberRodz = DropDownList_TypeOfWork.Items.Count;
    //    if (numberRodz > 0)
    //      DropDownList_TypeOfWork.SelectedIndex = numberRodz - 1;
    //    RefreshGodziny();
    //  }
    //}
    ///// <summary>
    ///// Method filtering subcategory and projects in DropDownList_SubCategory 
    ///// and DropDownList_Project using category name parameter
    ///// </summary>
    ///// <param name="kategorieRow">Main.kategorieRowClass object</param>
    //protected void SelectProjekty(Main.kategorieRowClass kategorieRow)
    //{
    //  DropDownList_SubCategory.Visible = true;
    //  DropDownList_SubCategory.Items.Clear();
    //  DropDownList_Project.Visible = true;
    //  DropDownList_Project.Items.Clear();
    //  foreach (GodzinySchema.PODKATEGORIERow podkategorieRow in
    //      main.godzinySchema.PODKATEGORIE)
    //  {
    //    if (kategorieRow.ID == 0 ||
    //        podkategorieRow.ID_KATEGORII == kategorieRow.ID)
    //    {
    //      DropDownList_SubCategory.Items.Add(new Main.podkategorieRowClass(podkategorieRow).ToString());
    //    }
    //  }
    //  foreach (GodzinySchema.PROJEKTYRow projektyRow in main.godzinySchema.PROJEKTY)
    //  {
    //    if (kategorieRow.ID == 0 ||
    //        (projektyRow.ID_KATEGORII == kategorieRow.ID &&
    //        main.statusy(DropDownList_State.SelectedValue).ID == projektyRow.ID_STATUSU) && projektyRow.NAZWA_KROTKA != "GUS")
    //    {
    //      DropDownList_Project.Items.Add(new Main.projektyRowClass(projektyRow).ToString());
    //    }
    //  }
    //  GodzinySchema.PODKATEGORIERow podkatrow = this.main.godzinySchema.PODKATEGORIE.NewPODKATEGORIERow();
    //  podkatrow.NAZWA = Resources.Resources.text_sellect_subcategory;
    //  podkatrow.ID = 0;
    //  podkatrow.OPIS = " ";
    //  podkatrow.ID_KATEGORII = 0;
    //  GodzinySchema.PROJEKTYRow projektyrow = this.main.godzinySchema.PROJEKTY.NewPROJEKTYRow();
    //  projektyrow.NAZWA = Resources.Resources.text_sellect_project;
    //  projektyrow.NAZWA_KROTKA = Resources.Resources.text_sellect_project;
    //  projektyrow.ID = 0;
    //  projektyrow.ID_UMOWY = 0;
    //  projektyrow.ROK = DateTime.Now.Year;
    //  projektyrow.LICZBA_GODZIN = 0;
    //  projektyrow.DATA_START = DateTime.Now;
    //  projektyrow.DATA_KONIEC = DateTime.Now;
    //  projektyrow.DATA_GWARANCJA = DateTime.Now;
    //  projektyrow.ID_KATEGORII = 0;
    //  projektyrow.ID_STATUSU = 0;
    //  projektyrow.ID_PODKATEGORII = 0;
    //  int numberPodkat = DropDownList_SubCategory.Items.Count;
    //  int numberProj = DropDownList_Project.Items.Count;
    //  if (numberPodkat > 0 && numberProj > 0)
    //  {
    //    DropDownList_SubCategory.Items.Add(new Main.podkategorieRowClass(podkatrow).ToString());
    //    DropDownList_SubCategory.SelectedIndex = numberPodkat;
    //    DropDownList_Project.Items.Add(new Main.projektyRowClass(projektyrow).ToString());
    //    DropDownList_Project.SelectedIndex = numberProj;
    //  }
    //  else
    //  {
    //    DropDownList_SubCategory.Visible = false;
    //    DropDownList_Project.Visible = false;
    //  }
    //  DropDownList_SubCategory.DataBind();
    //  DropDownList_Project.DataBind();
    //}

    ///// <summary>
    ///// Method filtering projects in DropDownList_Project using subcategory name parameter
    ///// </summary>
    ///// <param name="podkategorieRow">Main.podkategorieRowClass object</param>
    //protected void SelectProjekty2(Main.podkategorieRowClass podkategorieRow)
    //{
    //  DropDownList_Project.Visible = true;
    //  DropDownList_Project.Items.Clear();
    //  foreach (GodzinySchema.PROJEKTYRow projektyRow in main.godzinySchema.PROJEKTY)
    //  {
    //    if (podkategorieRow.ID == 0 || (projektyRow.ID_PODKATEGORII == podkategorieRow.ID &&
    //        main.statusy(DropDownList_State.SelectedValue).ID == projektyRow.ID_STATUSU) && projektyRow.NAZWA_KROTKA != "GUS")
    //    {
    //      DropDownList_Project.Items.Add(new Main.projektyRowClass(projektyRow).ToString());
    //    }
    //  }
    //  GodzinySchema.PROJEKTYRow projektyrow = this.main.godzinySchema.PROJEKTY.NewPROJEKTYRow();
    //  projektyrow.NAZWA = Resources.Resources.text_sellect_project;
    //  projektyrow.NAZWA_KROTKA = Resources.Resources.text_sellect_project;
    //  projektyrow.ID = 0;
    //  projektyrow.ID_UMOWY = 0;
    //  projektyrow.ROK = DateTime.Now.Year;
    //  projektyrow.LICZBA_GODZIN = 0;
    //  projektyrow.DATA_START = DateTime.Now;
    //  projektyrow.DATA_KONIEC = DateTime.Now;
    //  projektyrow.DATA_GWARANCJA = DateTime.Now;
    //  projektyrow.ID_KATEGORII = 0;
    //  projektyrow.ID_STATUSU = 0;
    //  projektyrow.ID_PODKATEGORII = 0;
    //  int numberProj = DropDownList_Project.Items.Count;
    //  if (numberProj > 0)
    //  {
    //    DropDownList_Project.Items.Add(new Main.projektyRowClass(projektyrow).ToString());
    //    DropDownList_Project.SelectedIndex = numberProj;
    //  }
    //  else
    //  {
    //    DropDownList_Project.Visible = false;
    //  }
    //  DropDownList_Project.DataBind();
    //}
    ///// <summary>
    ///// Button_new Click method inserting new GODZINY Table row
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    //protected void Button_new_Click(object sender, EventArgs e)
    //{
    //  if (Page.IsValid)
    //  {
    //    string error;
    //    error = "0";
    //    hours = Convert.ToDecimal(TextBox1.Text);
    //    try
    //    {
    //      if (main.projekty(DropDownList_Project.SelectedValue,
    //          Convert.ToDecimal(CurrentYear)).ID_STATUSU != main.statusy("Koniec").ID)
    //      {
    //        main.godzinyTableadapter.InsertQuery(main.pracownik(UserName).ID,
    //            m_Calendar.SelectedDate.Date,
    //            hours + Convert.ToDecimal(DropDownList_minutes.SelectedValue) / 60,
    //            TextBox_work_description.Text, main.projekty(DropDownList_Project.SelectedValue,
    //            Convert.ToDecimal(CurrentYear)).ID,
    //            main.statusy(DropDownList_State.SelectedValue).ID,
    //            main.rodzaje(DropDownList_TypeOfWork.SelectedValue).ID);
    //        RefreshGodziny();
    //      }
    //      else
    //      {
    //        Label8.Text = "Projekt zakończony, proszę wybrać inny";
    //      }
    //    }
    //    catch (System.Data.SqlClient.SqlException exOra)
    //    {
    //      if (exOra.ErrorCode == -2146232008)
    //      {
    //        error = "1";
    //      }
    //    }
    //    catch (ConstraintException ex)
    //    {
    //      throw new Exception("Problem: " + ex.Message);
    //    }
    //    if (Label8.Text == String.Empty)
    //    {
    //      System.Text.StringBuilder redirecturl = new System.Text.StringBuilder();
    //      redirecturl.Append("GodzinyInsert.aspx?Proj=");
    //      redirecturl.Append(DropDownList_Project.SelectedValue.ToString());
    //      redirecturl.Append("/");
    //      redirecturl.Append(m_Calendar.SelectedDate.Date.Year.ToString());
    //      redirecturl.Append("-");
    //      redirecturl.Append(m_Calendar.SelectedDate.Date.Month.ToString());
    //      redirecturl.Append("-");
    //      redirecturl.Append(m_Calendar.SelectedDate.Date.Day.ToString());
    //      if (error == "1")
    //      {
    //        redirecturl.Append("/");
    //        redirecturl.Append("1");
    //      }
    //      else
    //      {
    //        redirecturl.Append("/");
    //        redirecturl.Append("0");
    //      }
    //      Response.Redirect(redirecturl.ToString());
    //    }
    //  }
    //}
    ///// <summary>
    ///// m_CategoryDropDownList Selected Index Changed method
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    //protected void DropDownList_Category_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  GodzinySchema.KATEGORIERow kategorieRow = this.main.godzinySchema.KATEGORIE.NewKATEGORIERow();
    //  Main.kategorieRowClass kategorieItem = new Main.kategorieRowClass(kategorieRow);
    //  if (m_CategoryDropDownList.SelectedIndex == m_CategoryDropDownList.Items.Count - 1)
    //  {
    //    DropDownList_Load();
    //  }
    //  else
    //  {
    //    kategorieItem.MyRow = main.kategorie(m_CategoryDropDownList.SelectedValue);
    //    SelectProjekty(kategorieItem);
    //  }
    //}
    ///// <summary>
    ///// DropDownList_SubCategory Selected Index Changed method
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    //protected void DropDownList_SubCategory_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  GodzinySchema.PODKATEGORIERow podkategorieRow = this.main.godzinySchema.PODKATEGORIE.NewPODKATEGORIERow();
    //  Main.podkategorieRowClass podkategorieItem = new Main.podkategorieRowClass(podkategorieRow);
    //  if (DropDownList_SubCategory.SelectedIndex == DropDownList_SubCategory.Items.Count - 1)
    //  {
    //    DropDownList_Load();
    //  }
    //  else
    //  {
    //    podkategorieItem.MyRow = main.podkategorie(DropDownList_SubCategory.SelectedValue);
    //    SelectProjekty2(podkategorieItem);
    //  }
    //}
    ///// <summary>
    ///// GridView1 Selected Index Changed method
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    //protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  int id;
    //  id = Convert.ToInt32(GridView1.SelectedValue);
    //  DropDownList_Project.SelectedValue = main.projekty(main.godziny(id).ID_PROJEKTU).ToString();
    //  DropDownList_State.SelectedValue = main.statusy(main.godziny(id).ID_STATUSU).NAZWA;
    //  DropDownList_TypeOfWork.SelectedValue = main.rodzaje(Convert.ToInt32(main.godziny(id).ToString())).NAZWAPRACY;
    //  string[] val;
    //  val = main.godziny(id).LICZBA_GODZIN.ToString().Split(@",".ToCharArray(), StringSplitOptions.None);
    //  TextBox1.Text = val[0];
    //  if (val.Length == 2)
    //    DropDownList_minutes.SelectedValue = Convert.ToString(((Convert.ToDecimal(val[1]) * 60) / 100));
    //  TextBox_work_description.Text = main.godziny(id).OPIS;
    //  m_Calendar.SelectedDate = main.godziny(id).DATA.Date;
    //  PlanDataSource.SelectParameters[0].DefaultValue = main.projekty(DropDownList_Project.SelectedValue, Convert.ToDecimal(CurrentYear)).ID.ToString();
    //  Label12.Text = String.Format("Aktualnie masz {0} godzin w tym projekcie", main.suma(main.projekty(DropDownList_Project.SelectedValue, Convert.ToDecimal(CurrentYear)).ID, UserName));
    //  Label14.Text = String.Format("Aktualnie masz zaplanowane {0} godzin w kategorii {1}", main.plankatsum(UserName, main.projekty(DropDownList_Project.SelectedValue, Convert.ToDecimal(CurrentYear)).ID_KATEGORII, Convert.ToDecimal(CurrentYear)), main.kategorie(main.projekty(DropDownList_Project.SelectedValue, Convert.ToDecimal(CurrentYear)).ID_KATEGORII).NAZWA);
    //  Label14.BackColor = Color.White;
    //  Label15.Text = String.Format("Aktualnie masz przepracowane {0} godzin w kategorii {1}", main.godzinykatsum(UserName, main.projekty(DropDownList_Project.SelectedValue, Convert.ToDecimal(CurrentYear)).ID_KATEGORII, Convert.ToDecimal(CurrentYear)), main.kategorie(main.projekty(DropDownList_Project.SelectedValue, Convert.ToDecimal(CurrentYear)).ID_KATEGORII).NAZWA);
    //  Label15.BackColor = Color.White;
    //  Label16.Text = String.Format("Aktualnie masz wpisanych {0} godzin", main.sumadaily(UserName, Calendar1.SelectedDate.Date.ToString()));
    //  Button_new.Enabled = false;
    //  m_CategoryDropDownList.Enabled = false;
    //  DropDownList_SubCategory.Enabled = false;
    //  DropDownList_Project.Enabled = false;
    //  DropDownList_State.Enabled = false;
    //  Label8.Text = string.Empty;
    //  TextBox_work_description.Enabled = true;
    //}
    ///// <summary>
    ///// GridView1 Row Deleting method
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    //protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //  decimal categoryID = Convert.ToDecimal(GridView1.DataKeys[e.RowIndex].Value);
    //  main.godzinyTableadapter.DeleteQuery(categoryID);
    //  RefreshGodziny();
    //}
    ///// <summary>
    ///// GridView1 Row Command method 
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e">Commands: Update, Delete, Cancel</param>
    //protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //  decimal id;
    //  id = Convert.ToDecimal(GridView1.SelectedValue);
    //  int categoryID = 0;
    //  categoryID = Convert.ToInt32(e.CommandArgument);
    //  string error;
    //  error = "0";
    //  if (e.CommandName == "Update")
    //  {
    //    try
    //    {
    //      if (GridView1.SelectedIndex != -1)
    //      {
    //        if (id == categoryID)
    //        {
    //          main.godzinyTableadapter.UpdateQuery(main.projekty(DropDownList_Project.SelectedValue, Convert.ToDecimal(CurrentYear)).ID, Convert.ToDecimal(TextBox1.Text) + Convert.ToDecimal(DropDownList_minutes.SelectedValue) / 60, TextBox_work_description.Text, main.statusy(DropDownList_State.SelectedValue).ID, main.rodzaje(DropDownList_TypeOfWork.SelectedValue).ID, Convert.ToDecimal(categoryID));
    //          RefreshGodziny();
    //        }
    //      }
    //    }
    //    catch (System.Data.SqlClient.SqlException exOra)
    //    {
    //      if (exOra.ErrorCode == -2146232008)
    //      {
    //        error = "1";
    //      }
    //    }
    //    catch (ConstraintException ex)
    //    {
    //      throw new Exception("Problem: " + ex.Message);
    //    }
    //    System.Text.StringBuilder redirecturl = new System.Text.StringBuilder();
    //    redirecturl.Append("GodzinyInsert.aspx?Proj=");
    //    redirecturl.Append("/");
    //    redirecturl.Append(Calendar1.SelectedDate.Date.Year.ToString());
    //    redirecturl.Append("-");
    //    redirecturl.Append(Calendar1.SelectedDate.Date.Month.ToString());
    //    redirecturl.Append("-");
    //    redirecturl.Append(Calendar1.SelectedDate.Date.Day.ToString());
    //    if (error == "1")
    //    {
    //      redirecturl.Append("/");
    //      redirecturl.Append("1");
    //    }
    //    else
    //    {
    //      redirecturl.Append("/");
    //      redirecturl.Append("0");
    //    }
    //    Response.Redirect(redirecturl.ToString());
    //  }
    //  else if (e.CommandName == "Delete")
    //  {
    //    categoryID = Convert.ToInt32(e.CommandArgument);
    //    try
    //    {
    //      main.godzinyTableadapter.DeleteQuery(Convert.ToDecimal(categoryID));
    //      RefreshGodziny();
    //    }
    //    catch (System.Data.SqlClient.SqlException exOra)
    //    {
    //      throw new Exception("Problem z baza danych: " + exOra.Message);
    //    }
    //    catch (ConstraintException ex)
    //    {
    //      throw new Exception("Problem: " + ex.Message);
    //    }
    //    System.Text.StringBuilder redirecturl = new System.Text.StringBuilder();
    //    redirecturl.Append("GodzinyInsert.aspx?Proj=");
    //    redirecturl.Append("/");
    //    redirecturl.Append(Calendar1.SelectedDate.Date.Year.ToString());
    //    redirecturl.Append("-");
    //    redirecturl.Append(Calendar1.SelectedDate.Date.Month.ToString());
    //    redirecturl.Append("-");
    //    redirecturl.Append(Calendar1.SelectedDate.Date.Day.ToString());
    //    redirecturl.Append("/");
    //    redirecturl.Append("0");
    //    Response.Redirect(redirecturl.ToString());
    //  }
    //}
    ///// <summary>
    ///// DropDownList_Project Selected Index Changed method
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    //protected void DropDownList_Project_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  if (DropDownList_Project.SelectedIndex == DropDownList_Project.Items.Count - 1)
    //  {
    //    DropDownList_Load();
    //    Label14.Visible = false;
    //    Label15.Visible = false;
    //  }
    //  else
    //  {
    //    PlanDataSource.SelectParameters[0].DefaultValue = main.projekty(DropDownList_Project.SelectedValue, Convert.ToDecimal(CurrentYear)).ID.ToString();
    //    Label12.Text = String.Format("Aktualnie masz {0} godzin w tym projekcie", main.suma(main.projekty(DropDownList_Project.SelectedValue, Convert.ToDecimal(CurrentYear)).ID, UserName));
    //    Label14.Text = String.Format("Aktualnie masz zaplanowane {0} godzin w kategorii {1}", main.plankatsum(UserName, main.projekty(DropDownList_Project.SelectedValue, Convert.ToDecimal(CurrentYear)).ID_KATEGORII, Convert.ToDecimal(CurrentYear)), main.kategorie(main.projekty(DropDownList_Project.SelectedValue, Convert.ToDecimal(CurrentYear)).ID_KATEGORII).NAZWA);
    //    Label14.BackColor = Color.White;
    //    Label15.Text = String.Format("Aktualnie masz przepracowane {0} godzin w kategorii {1}", main.godzinykatsum(UserName, main.projekty(DropDownList_Project.SelectedValue, Convert.ToDecimal(CurrentYear)).ID_KATEGORII, Convert.ToDecimal(CurrentYear)), main.kategorie(main.projekty(DropDownList_Project.SelectedValue, Convert.ToDecimal(CurrentYear)).ID_KATEGORII).NAZWA);
    //    Label15.BackColor = Color.White;
    //    Label16.Text = String.Format("Aktualnie masz wpisanych {0} godzin", main.sumadaily(UserName, Calendar1.SelectedDate.Date.ToString()));
    //  }
    //  Label8.Text = string.Empty;
    //}
    ///// <summary>
    ///// Calendar Selection Changed method
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    //protected void Calendar1_SelectionChanged(object sender, EventArgs e)
    //{
    //  Label16.Text = String.Format("Aktualnie masz wpisanych {0} godzin", main.sumadaily(UserName, Calendar1.SelectedDate.Date.ToString()));
    //  Button_new.Enabled = true;
    //  if (GridView1.SelectedRow == null)
    //  {
    //    CurrentYear = m_Calendar.SelectedDate.Year;
    //    main.projektyTableadapter.Fill(main.godzinySchema.PROJEKTY, Convert.ToDecimal(CurrentYear));
    //    PopulateProjectList_SelectedState_Or_Date_HasBeenChanged();
    //  }
    //}
    //protected void DropDownList_State_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  PopulateProjectList_SelectedState_Or_Date_HasBeenChanged();
    //}

    //private void PopulateProjectList_SelectedState_Or_Date_HasBeenChanged()
    //{
    //  GodzinySchema.KATEGORIERow kategorieRow = this.main.godzinySchema.KATEGORIE.NewKATEGORIERow();
    //  Main.kategorieRowClass kategorieItem = new Main.kategorieRowClass(kategorieRow);
    //  GodzinySchema.PODKATEGORIERow podkategorieRow = this.main.godzinySchema.PODKATEGORIE.NewPODKATEGORIERow();
    //  Main.podkategorieRowClass podkategorieItem = new Main.podkategorieRowClass(podkategorieRow);
    //  DropDownList_Project.Visible = true;
    //  DropDownList_Project.Items.Clear();
    //  foreach (GodzinySchema.PROJEKTYRow projektyRow in main.godzinySchema.PROJEKTY)
    //  {
    //    if (projektyRow.ROK == Convert.ToDecimal(CurrentYear) && main.statusy(DropDownList_State.SelectedValue).ID == projektyRow.ID_STATUSU)
    //    {
    //      DropDownList_Project.Items.Add(new Main.projektyRowClass(projektyRow).ToString());
    //    }
    //  }
    //  GodzinySchema.PROJEKTYRow projektyrow = this.main.godzinySchema.PROJEKTY.NewPROJEKTYRow();
    //  projektyrow.NAZWA = Resources.Resources.text_sellect_project;
    //  projektyrow.NAZWA_KROTKA = Resources.Resources.text_sellect_project;
    //  projektyrow.ID = 0;
    //  projektyrow.ID_UMOWY = 0;
    //  projektyrow.ROK = DateTime.Now.Year;
    //  projektyrow.LICZBA_GODZIN = 0;
    //  projektyrow.DATA_START = DateTime.Now;
    //  projektyrow.DATA_KONIEC = DateTime.Now;
    //  projektyrow.DATA_GWARANCJA = DateTime.Now;
    //  projektyrow.ID_KATEGORII = 0;
    //  projektyrow.ID_STATUSU = 0;
    //  projektyrow.ID_PODKATEGORII = 0;
    //  int numberProj = DropDownList_Project.Items.Count;
    //  if (numberProj > 0)
    //  {
    //    DropDownList_Project.Items.Add(new Main.projektyRowClass(projektyrow).ToString());
    //    DropDownList_Project.SelectedIndex = numberProj;
    //  }
    //  else
    //  {
    //    DropDownList_Project.Visible = false;
    //  }
    //  DropDownList_Project.DataBind();
    //  if (m_CategoryDropDownList.SelectedValue != Resources.Resources.text_sellect_category)
    //  {
    //    kategorieItem.MyRow = main.kategorie(m_CategoryDropDownList.SelectedValue);
    //    SelectProjekty(kategorieItem);
    //  }
    //  if (DropDownList_SubCategory.SelectedValue != Resources.Resources.text_sellect_subcategory)
    //  {
    //    podkategorieItem.MyRow = main.podkategorie(DropDownList_SubCategory.SelectedValue);
    //    SelectProjekty2(podkategorieItem);
    //  }
    //}
    //protected void DropDownList_TypeOfWork_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  if (DropDownList_TypeOfWork.SelectedValue != Resources.Resources.text_sellect_work_type)
    //  {
    //    TextBox_work_description.Text = main.rodzaje(DropDownList_TypeOfWork.SelectedValue).OPIS;
    //    TextBox_work_description.Enabled = true;
    //  }
    //  else
    //  {
    //    TextBox_work_description.Text = "Opis, zakres czynności, numer projektu/oferty itp.";
    //    TextBox_work_description.Enabled = false;
    //  }
    //  Label8.Text = string.Empty;
    //}
    //protected void Button_cancel_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    GridView1.SelectedIndex = -1;
    //    Button_new.Enabled = true;
    //  }
    //  catch (System.Data.SqlClient.SqlException exOra)
    //  {
    //    throw new Exception("Problem z baza danych: " + exOra.Message);
    //  }
    //  catch (ConstraintException ex)
    //  {
    //    throw new Exception("Problem: " + ex.Message);
    //  }
    //  System.Text.StringBuilder redirecturl = new System.Text.StringBuilder();
    //  redirecturl.Append("GodzinyInsert.aspx?Proj=");
    //  redirecturl.Append("/");
    //  redirecturl.Append(m_Calendar.SelectedDate.Date.Year.ToString());
    //  redirecturl.Append("-");
    //  redirecturl.Append(m_Calendar.SelectedDate.Date.Month.ToString());
    //  redirecturl.Append("-");
    //  redirecturl.Append(m_Calendar.SelectedDate.Date.Day.ToString());
    //  redirecturl.Append("/");
    //  redirecturl.Append("0");
    //  Response.Redirect(redirecturl.ToString());
    //}
  }
}
