using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CAS.AgileWorkloadTracker.Linq;
using Microsoft.SharePoint;
using CAS.SharePoint.Linq;

namespace CAS.AgileWorkloadTracker.Dashboards.Webparts.WorkloadManagement
{
  public partial class WorkloadManagementUserControl : UserControl
  {
    private int CurrentYear
    {
      get
      {
        //TODO ???
        //if (Session[m_keyCurrentYear] != null && Session[m_keyCurrentYear] is int)
        //  return ((int)Session[m_keyCurrentYear]);
        if (m_Calendar.SelectedDate.Year > 2000)
          return m_Calendar.SelectedDate.Year;
        return DateTime.Now.Year;
      }
      set
      {
        //Session[m_keyCurrentYear] = value;
      }
    }
    /// <summary>
    /// Method loading KATEGORIE Table rows to m_CategoryDropDownList
    /// </summary>
    protected void Kat_Load()
    {
      //TODO get from list
      //foreach (GodzinySchema.KATEGORIERow row in main.godzinySchema.KATEGORIE)
      //  m_CategoryDropDownList.Items.Add(new Main.kategorieRowClass(row).ToString());
    }
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
    private void FillupTaskaDropDown()
    {
      m_TaskDropDown.Items.Clear();
      if (m_ProjectDropDown.SelectedIndex <= 0)
        m_TaskDropDown.Items.Add(new ListItem(m_SelectProjectDropDownEntry, String.Empty) { Selected = true });
      else
      {
        m_TaskDropDown.Items.Clear();
        m_TaskDropDown.Items.Add(new ListItem(m_SelectTaskDropDownEntry, String.Empty) { Selected = true });
        Projects _cp = Element.GetAtIndex<Projects>(_entt.DataContext.Projects, m_ProjectDropDown.SelectedValue);
        foreach (Tasks _taskIdx in from _tidx in _cp.Tasks select _tidx)
          m_TaskDropDown.Items.Add(new ListItem(_taskIdx.Tytuł, _taskIdx.Identyfikator.ToString()));
        //TODO liczba godzin w projekcie planowane i wykorzystane 
      }
    }
    /// <summary>
    /// Method loading KATEGORIE, PODKATEGORIE and PROJEKTY Table rows into DropDownLists 
    /// and selecting first indexes of these DropDownLists
    /// </summary>
    private void FillupProjectDropDown()
    {
      //TODO [AWT-3488] Change the ProjectYear column in the Projects
      m_ProjectDropDown.Items.Clear();
      m_ProjectDropDown.Items.Add(new ListItem(m_SelectProjectDropDownEntry, String.Empty) { Selected = true });
      foreach (var _row2 in from _pidx in _entt.DataContext.Projects select _pidx)
        m_ProjectDropDown.Items.Add(new ListItem(_row2.Tytuł, _row2.Identyfikator.ToString()));
    }
    ///// <summary>
    ///// Accepting GodzinySchema changes
    ///// </summary>
    //public void RefreshGodziny()
    //{
    //  main.godzinySchema.AcceptChanges();
    //}
    protected override void OnInit(EventArgs e)
    {
      base.OnInit(e);
      _entt = new DataContextManagement<Entities>(this);
    }
    /// <summary>
    /// Page Load method
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
      //  UserName = UserIdentityHelper.GetCurrentUser(Session);
      try
      {
        if (!this.IsPostBack)
        {
          //Grid setup
          m_GridView.EmptyDataText = "No workload defined";
          m_GridView.Columns.Add(new BoundField() { DataField = "Task", HeaderText = "Task" });
          m_GridView.Columns.Add(new BoundField() { DataField = "Hours", HeaderText = "Workload [h]" });
          m_GridView.Columns.Add(new BoundField() { DataField = "Project", HeaderText = "Project" });
          m_GridView.Columns.Add(new BoundField() { DataField = "ID", HeaderText = "ID", Visible = false });
          m_GridView.DataKeyNames = new String[] { "ID" };
          //Calendar setup
          m_Calendar.SelectedDate = DateTime.Now.Date;
          m_Calendar.VisibleDate = DateTime.Now.Date;
          //DropDownList'c setup 
          FillupProjectDropDown();
        }
        At = "FillupTaskaDropDown";
        FillupTaskaDropDown();
        m_ProjectDropDown.SelectedIndexChanged += new EventHandler(m_ProjectDropDown_SelectedIndexChanged);
        At = "FindForUser";
        FillupWorkflowGridView();
      }
      catch (Exception ex)
      {
        ReportException(ex);
      }
    }
    private void FillupWorkflowGridView()
    {
      if (Me == null)
      {
        this.Controls.Add(new Literal() { Text = "User not recognized - you must be added to the Recourses" });
        m_GridView.Visible = false;
      }
      else
      {
        this.Controls.Add(new Literal() { Text = "Welcome: " + Me.EmployeeADAccount.Tytuł });
        At = "m_GridView.DataSource";
        m_GridView.DataSource = from _wlidx in Me.Workload
                                where _wlidx.WorkloadDate.Value.Date == m_Calendar.SelectedDate.Date
                                select new
                                {
                                  Task = _wlidx.Workload2TaskTitle == null ? m_SelectTaskDropDownEntry : _wlidx.Workload2TaskTitle.Tytuł,
                                  Hours = _wlidx.Hours.GetValueOrDefault(0),
                                  Project = _wlidx.Workload2ProjectTitle == null ? m_SelectProjectDropDownEntry : _wlidx.Workload2ProjectTitle.Tytuł,
                                  ID = _wlidx.Identyfikator
                                };
        m_GridView.DataBind();
      }
    }
    CAS.AgileWorkloadTracker.Linq.Resources p_me = null;
    private CAS.AgileWorkloadTracker.Linq.Resources Me
    {
      get
      {
        if (p_me == null)
          p_me = CAS.AgileWorkloadTracker.Linq.Resources.FindForUser(_entt.DataContext, SPContext.Current.Web.CurrentUser);
        return p_me;
      }
    }
    private void ReportException(Exception ex)
    {
      string _format = "Exception at: {0} of : {1}.";
      this.Controls.Add(new Literal() { Text = String.Format(_format, At, ex.Message) });
    }
    #region event handlers
    private void m_ProjectDropDown_SelectedIndexChanged(object sender, EventArgs e)
    {
      FillupTaskaDropDown();
    }
    protected void m_GridView_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        if (m_GridView.SelectedIndex < 0)
          return;
        string _selection = m_GridView.SelectedDataKey.Value.ToString();
        Workload _ld = Element.GetAtIndex<Workload>(_entt.DataContext.Workload, _selection);
        m_WorkloadDescriptionTextBox.Text = _ld.Tytuł;
        m_WorkloadHoursTextBox.Text = _ld.Hours.GetValueOrDefault(0).ToString();
      }
      catch (Exception _ex)
      {
        ReportException(_ex);
      }
    }
    #endregion
    /// <summary>
    /// Button_new Click method inserting new GODZINY Table row
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void CreateNewWokload()
    {
      if (!Page.IsValid)
        return;
      double _hours = Convert.ToDouble(m_WorkloadHoursTextBox.Text);
      Projects _project = Element.GetAtIndex<Projects>(_entt.DataContext.Projects, m_ProjectDropDown.SelectedValue);
      Tasks _task = Element.GetAtIndex<Tasks>(_entt.DataContext.Task, m_TaskDropDown.SelectedValue);
      Workload _newOne = new Workload()
      {
        Hours = _hours,
        Tytuł = m_WorkloadDescriptionTextBox.Text,
        Workload2ProjectTitle = _project,
        Workload2ResourcesTitle = Me,
        Workload2StageTitle = _project.Project2StageTitle,
        Workload2TaskTitle = _task,
        WorkloadDate = m_Calendar.SelectedDate.Date
      };
      _entt.DataContext.Workload.InsertOnSubmit(_newOne);
      _entt.DataContext.SubmitChanges();
    }
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
    /// <summary>
    /// GridView1 Row Command method 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e">Commands: Update, Delete, Cancel</param>
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
    /// <summary>
    /// Calendar Selection Changed method
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void m_Calendar_SelectionChanged(object sender, EventArgs e)
    {
      //double _hoursADay = (from _widx in _entt.Workload where _widx.WorkloadDate == m_Calendar.SelectedDate select _widx.Hours.Value ).ToList<double>().AsQueryable().Sum<double>();
      m_ButtonAddNew.Enabled = true;
      m_WorkloadHoursTextBox.Text = String.Empty;
      m_WorkloadDescriptionTextBox.Text = String.Empty;
      FillupWorkflowGridView();
      //this.f
      //if (m_GridView.SelectedRow == null)
      //{
      //CurrentYear = m_Calendar.SelectedDate.Year;
      //main.projektyTableadapter.Fill(main.godzinySchema.PROJEKTY, Convert.ToDecimal(CurrentYear));
      //PopulateProjectList_SelectedState_Or_Date_HasBeenChanged();
      //}

    }
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
    private string At { get; set; }
    private const string m_SelectProjectDropDownEntry = "  -- select project -- ";
    private const string m_SelectTaskDropDownEntry = "  -- select task -- ";
    private const string m_keyCurrentYear = "CurrentYear";
    private DataContextManagement<Entities> _entt = null;
  }
}
