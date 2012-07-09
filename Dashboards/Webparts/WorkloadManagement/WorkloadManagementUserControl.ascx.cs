using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CAS.AgileWorkloadTracker.Linq;
using CAS.SharePoint.Linq;
using CAS.SharePoint.Web;
using Microsoft.SharePoint;

namespace CAS.AgileWorkloadTracker.Dashboards.Webparts.WorkloadManagement
{
  public partial class WorkloadManagementUserControl : UserControl
  {
    public WorkloadManagementUserControl()
    {
      m_StateMachineEngine = new LocalStateMachine(this);
    }

    #region UserControl override
    [Serializable]
    private class ControlState
    {
      #region state fields
      internal GenericStateMachineEngine.ControlsSet SetEnabled = 0;
      public GenericStateMachineEngine.InterfaceState InterfaceState = GenericStateMachineEngine.InterfaceState.ViewState;
      #endregion

      #region public
      public ControlState(ControlState _old)
      {
        if (_old == null)
          return;
        InterfaceState = _old.InterfaceState;
      }
      #endregion
    }
    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
    protected override void OnInit(EventArgs e)
    {
      Page.RegisterRequiresControlState(this);
      m_DataContext = new DataContextManagement<Entities>(this);
      base.OnInit(e);
    }
    /// <summary>
    /// Page Load method
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
      try
      {
        if (!this.IsPostBack)
        {
          m_StateMachineEngine.InitMahine();
          //Grid setup
          m_GridView.EmptyDataText = "No workload defined";
          m_GridView.Columns.Add(new BoundField() { DataField = "Hours", HeaderText = "Workload [h]" });
          m_GridView.Columns.Add(new BoundField() { DataField = "Project", HeaderText = "Project" });
          m_GridView.Columns.Add(new BoundField() { DataField = "Task", HeaderText = "Task" });
          m_GridView.Columns.Add(new BoundField() { DataField = "ID", HeaderText = "ID", Visible = false });
          m_GridView.DataKeyNames = new String[] { "ID" };
          //Calendar setup
          m_Calendar.SelectedDate = DateTime.Now.Date;
          m_Calendar.VisibleDate = DateTime.Now.Date;
          //DropDownList'c setup 
          FillupProjectDropDown();
          At = "FillupTaskaDropDown";
          FillupTaskaDropDown();
          At = "FindForUser";
          FillupWorkflowGridView();
          ShouwUserInformation();
        }
        m_ProjectDropDown.SelectedIndexChanged += new EventHandler(m_ProjectDropDown_SelectedIndexChanged);
        m_ButtonSave.Click += new EventHandler(m_StateMachineEngine.SaveButton_Click);
        m_ButtonAddNew.Click += new EventHandler(m_StateMachineEngine.NewButton_Click);
        m_ButtonCancel.Click += new EventHandler(m_StateMachineEngine.CancelButton_Click);
        m_ButtonEdit.Click += new EventHandler(m_StateMachineEngine.EditButton_Click);
        m_ButtonDelete.Click += new EventHandler(m_StateMachineEngine.DeleteButton_Click);
      }
      catch (Exception _ex)
      {
        ShowActionResult(GenericStateMachineEngine.ActionResult.Exception(_ex, "Page_Load"));
      }
    }
    /// <summary>
    /// Loads the state of the control.
    /// </summary>
    /// <param name="state">The state.</param>
    protected override void LoadControlState(object state)
    {
      if (state != null)
      {
        m_ControlState = (ControlState)state;
        m_StateMachineEngine.InitMahine(m_ControlState.InterfaceState);
      }
      else
        m_StateMachineEngine.InitMahine();
    }
    /// <summary>
    /// Saves any server control state changes that have occurred since the time the page was posted back to the server.
    /// </summary>
    /// <returns>
    /// Returns the server control's current state. If there is no state associated with the control, this method returns null.
    /// </returns>
    protected override object SaveControlState()
    {
      return m_ControlState;
    }
    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
    protected override void OnPreRender(EventArgs e)
    {
      SetEnabled(m_ControlState.SetEnabled);
      if (m_GridView.SelectedIndex == -1)
      {
        m_ButtonDelete.Enabled = false;
        m_ButtonEdit.Enabled = false;
      }
      base.OnPreRender(e);
    }
    #endregion

    #region State machine
    private LocalStateMachine m_StateMachineEngine;
    private class LocalStateMachine : StateMachine
    {
      #region public
      internal LocalStateMachine(WorkloadManagementUserControl _parent)
      {
        m_Parent = _parent;
      }
      internal void InitMahine(InterfaceState _ControlState)
      {
        m_Parent.m_ControlState.InterfaceState = _ControlState;
      }
      internal void InitMahine()
      {
        m_Parent.m_ControlState.InterfaceState = InterfaceState.ViewState;
        EnterState();
      }
      #endregion

      #region StateMachine override
      protected override void ClearUserInterface()
      {
        m_Parent.ClearUserInterface();
      }
      protected override GenericStateMachineEngine.ActionResult Create()
      {
        return m_Parent.CreateNewWokload();
      }
      protected override GenericStateMachineEngine.ActionResult Delete()
      {
        return m_Parent.Delete();
      }
      protected override void SetEnabled(GenericStateMachineEngine.ControlsSet _buttons)
      {
        m_Parent.m_ControlState.SetEnabled = _buttons;
      }
      protected override GenericStateMachineEngine.ActionResult Show()
      {
        return m_Parent.Show();
      }
      protected override void ShowActionResult(GenericStateMachineEngine.ActionResult _rslt)
      {
        m_Parent.ShowActionResult(_rslt);
      }
      protected override void SMError(GenericStateMachineEngine.InterfaceEvent interfaceEvent)
      {
        m_Parent.ShowActionResult(GenericStateMachineEngine.ActionResult.Exception(new ApplicationException("State Machine Error"), "SMError"));
      }
      protected override GenericStateMachineEngine.ActionResult Update()
      {
        return m_Parent.Update();
      }
      protected override GenericStateMachineEngine.InterfaceState CurrentMachineState
      {
        get
        {
          return m_Parent.m_ControlState.InterfaceState;
        }
        set
        {
          if (m_Parent.m_ControlState.InterfaceState == value)
            return;
          m_Parent.m_ControlState.InterfaceState = value;
          EnterState();
        }
      }
      #endregion

      #region private
      private WorkloadManagementUserControl m_Parent { get; set; }
      #endregion

    }
    private GenericStateMachineEngine.ActionResult CreateNewWokload()
    {
      if (!Page.IsValid)
        return GenericStateMachineEngine.ActionResult.NotValidated("Required information must be provided.");
      try
      {
        double _hours = TextBoxToHours();
        Projects _project = Element.GetAtIndex<Projects>(m_DataContext.DataContext.Projects, m_ProjectDropDown.SelectedValue);
        Tasks _task = Element.GetAtIndex<Tasks>(m_DataContext.DataContext.Task, m_TaskDropDown.SelectedValue);
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
        m_DataContext.DataContext.Workload.InsertOnSubmit(_newOne);
        m_DataContext.DataContext.SubmitChanges();
      }
      catch (Exception _ex)
      {
        return GenericStateMachineEngine.ActionResult.Exception(_ex, "CreateNewWokload");
      }
      return GenericStateMachineEngine.ActionResult.Success;
    }
    private double TextBoxToHours()
    {
      return Convert.ToDouble(m_WorkloadHoursTextBox.Text);
    }
    private GenericStateMachineEngine.ActionResult Delete()
    {
      try
      {
        Workload _wkl = Element.GetAtIndex<Workload>(m_DataContext.DataContext.Workload, m_GridView.SelectedDataKey.Value.ToString());
        m_GridView.SelectedIndex = -1;
        m_DataContext.DataContext.Workload.RecycleOnSubmit(_wkl);
        m_DataContext.DataContext.SubmitChanges();
      }
      catch (Exception _ex)
      {
        return GenericStateMachineEngine.ActionResult.Exception(_ex, "Delete");
      }
      return GenericStateMachineEngine.ActionResult.Success;
    }
    private void SetEnabled(GenericStateMachineEngine.ControlsSet _set)
    {
      m_WorkloadMinutesDropDown.Enabled = (_set & GenericStateMachineEngine.ControlsSet.EditModeOn) != 0;
      m_WorkloadHoursTextBox.Enabled = (_set & GenericStateMachineEngine.ControlsSet.EditModeOn) != 0;
      m_WorkloadDescriptionTextBox.Enabled = (_set & GenericStateMachineEngine.ControlsSet.EditModeOn) != 0;
      m_ProjectDropDown.Enabled = (_set & GenericStateMachineEngine.ControlsSet.EditModeOn) != 0;
      m_TaskDropDown.Enabled = (_set & GenericStateMachineEngine.ControlsSet.EditModeOn) != 0;
      m_GridView.Enabled = (_set & GenericStateMachineEngine.ControlsSet.EditModeOn) == 0;
      m_Calendar.Enabled = (_set & GenericStateMachineEngine.ControlsSet.EditModeOn) == 0;
      //Buttons
      m_ButtonSave.Enabled = (_set & GenericStateMachineEngine.ControlsSet.SaveOn) != 0;
      m_ButtonDelete.Enabled = (_set & GenericStateMachineEngine.ControlsSet.DeleteOn) != 0;
      m_ButtonCancel.Enabled = (_set & GenericStateMachineEngine.ControlsSet.CancelOn) != 0;
      m_ButtonEdit.Enabled = ((_set & GenericStateMachineEngine.ControlsSet.EditOn) != 0);
      m_ButtonAddNew.Enabled = (_set & GenericStateMachineEngine.ControlsSet.NewOn) != 0;
    }
    private GenericStateMachineEngine.ActionResult Show()
    {
      try
      {
        this.FillupWorkflowGridView();
      }
      catch (Exception _ex)
      {
        return GenericStateMachineEngine.ActionResult.Exception(_ex, "GenericStateMachineEngine.ActionResult");
      };
      return GenericStateMachineEngine.ActionResult.Success;
    }
    private GenericStateMachineEngine.ActionResult Update()
    {
      double _hours = TextBoxToHours();
      try
      {
        Workload _wkl = Element.GetAtIndex<Workload>(m_DataContext.DataContext.Workload, m_GridView.SelectedDataKey.Value.ToString());
        Tasks _task = Element.GetAtIndex<Tasks>(m_DataContext.DataContext.Task, m_TaskDropDown.SelectedValue);
        _wkl.Hours = _hours;
        _wkl.Tytuł = m_WorkloadDescriptionTextBox.Text;
        _wkl.WorkloadDate = m_Calendar.SelectedDate.Date;
        _wkl.Workload2TaskTitle = _task;
        m_DataContext.DataContext.SubmitChanges();
      }
      catch (Exception _ex)
      {
        return GenericStateMachineEngine.ActionResult.Exception(_ex, "GenericStateMachineEngine.ActionResult");
      }
      return GenericStateMachineEngine.ActionResult.Success;
    }
    private void ClearUserInterface()
    {
      m_GridView.SelectedIndex = -1;
      m_WorkloadDescriptionTextBox.Text = String.Empty;
      m_WorkloadHoursTextBox.Text = String.Empty;
    }
    #endregion

    #region helpers
    private void ShouwUserInformation()
    {
      if (Me == null)
      {
        this.Controls.Add(new Literal() { Text = String.Format(CAS.SharePoint.Web.CommonDefinitions.ErrorMessageFormat, "User not recognized - you must be added to the Recourses") });
      }
      else
        this.Controls.Add(new Literal() { Text = String.Format(CAS.SharePoint.Web.CommonDefinitions.ErrorMessageFormat, "Welcome: " + Me.EmployeeADAccount.Tytuł) });
    }
    private void FillupWorkflowGridView()
    {
      if (Me == null)
        m_GridView.DataSource = null;
      else
      {
        At = "m_GridView.DataSource";
        m_GridView.DataSource = from _wlidx in Me.Workload
                                where _wlidx.WorkloadDate.Value.Date == m_Calendar.SelectedDate.Date
                                select new
                                {
                                  Hours = _wlidx.Hours.GetValueOrDefault(0),
                                  Project = _wlidx.Workload2ProjectTitle == null ? m_SelectProjectDropDownEntry : _wlidx.Workload2ProjectTitle.Tytuł,
                                  Task = _wlidx.Workload2TaskTitle == null ? m_SelectTaskDropDownEntry : _wlidx.Workload2TaskTitle.Tytuł,
                                  ID = _wlidx.Identyfikator
                                };
      }
      m_GridView.DataBind();
      m_GridView.SelectedIndex = -1;
    }
    private void FillupTaskaDropDown()
    {
      m_TaskDropDown.Items.Clear();
      if (m_ProjectDropDown.SelectedIndex <= 0)
        m_TaskDropDown.Items.Add(new ListItem(m_SelectProjectDropDownEntry, String.Empty) { Selected = true });
      else
      {
        m_TaskDropDown.Items.Clear();
        m_TaskDropDown.Items.Add(new ListItem(m_SelectTaskDropDownEntry, String.Empty) { Selected = true });
        Projects _cp = Element.GetAtIndex<Projects>(m_DataContext.DataContext.Projects, m_ProjectDropDown.SelectedValue);
        foreach (Tasks _taskIdx in from _tidx in _cp.Tasks select _tidx)
          m_TaskDropDown.Items.Add(new ListItem(_taskIdx.Tytuł, _taskIdx.Identyfikator.ToString()));
        //TODO liczba godzin w projekcie planowane i wykorzystane 
      }
    }
    private void FillupProjectDropDown()
    {
      //TODO [AWT-3488] Change the ProjectYear column in the Projects
      m_ProjectDropDown.Items.Clear();
      m_ProjectDropDown.Items.Add(new ListItem(m_SelectProjectDropDownEntry, String.Empty) { Selected = true });
      foreach (var _row2 in from _pidx in m_DataContext.DataContext.Projects select _pidx)
        m_ProjectDropDown.Items.Add(new ListItem(_row2.Tytuł, _row2.Identyfikator.ToString()));
    }
    private string At { get; set; }
    #endregion

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
        Workload _workload = Element.GetAtIndex<Workload>(m_DataContext.DataContext.Workload, _selection);
        m_WorkloadDescriptionTextBox.Text = _workload.Tytuł;
        m_WorkloadHoursTextBox.Text = _workload.Hours.GetValueOrDefault(0).ToString();
        m_ProjectDropDown.Select(_workload.Workload2ProjectTitle != null ? _workload.Workload2ProjectTitle.Identyfikator.Value : 0);
        FillupTaskaDropDown();
        m_TaskDropDown.Select(_workload.Workload2TaskTitle != null ? _workload.Workload2TaskTitle.Identyfikator.Value : 0);
      }
      catch (Exception _ex)
      {
        ShowActionResult(GenericStateMachineEngine.ActionResult.Exception(_ex, "m_GridView_SelectedIndexChanged"));
      }
    }
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
    #endregion

    #region vars
    private const string m_SelectProjectDropDownEntry = "  -- select project -- ";
    private const string m_SelectTaskDropDownEntry = "  -- select task -- ";
    private const string m_keyCurrentYear = "CurrentYear";
    private DataContextManagement<Entities> m_DataContext = null;
    private ControlState m_ControlState = new ControlState(null);
    private CAS.AgileWorkloadTracker.Linq.Resources p_me = null;
    private CAS.AgileWorkloadTracker.Linq.Resources Me
    {
      get
      {
        if (p_me == null)
          p_me = CAS.AgileWorkloadTracker.Linq.Resources.FindForUser(m_DataContext.DataContext, SPContext.Current.Web.CurrentUser);
        return p_me;
      }
    }
    #endregion

    #region errors handling
    internal void ShowActionResult(GenericStateMachineEngine.ActionResult _rslt)
    {
      if (_rslt.LastActionResult == GenericStateMachineEngine.ActionResult.Result.Success)
        return;
      if (_rslt.LastActionResult == GenericStateMachineEngine.ActionResult.Result.Exception)
      {
#if DEBUG
        string _format = String.Format(CAS.SharePoint.Web.CommonDefinitions.ErrorMessageFormat, "Exception at: {0} of : {1}.");
        this.Controls.Add(new Literal() { Text = String.Format(_format, _rslt.ActionException.Source + At, _rslt.ActionException.Message) });
#endif
        Anons.WriteEntry(_rslt.ActionException.Source, _rslt.ActionException.Message);
      }
      else
      {
        string _format = String.Format(CAS.SharePoint.Web.CommonDefinitions.ErrorMessageFormat, "Validation error at: {0} of : {1}.");
        this.Controls.Add(new Literal() { Text = String.Format(_format, At, _rslt.ActionException.Message) });
      }
    }
    #endregion

    //private int CurrentYear
    //{
    //  get
    //  {
    //    //TODO ???
    //    //if (Session[m_keyCurrentYear] != null && Session[m_keyCurrentYear] is int)
    //    //  return ((int)Session[m_keyCurrentYear]);
    //    if (m_Calendar.SelectedDate.Year > 2000)
    //      return m_Calendar.SelectedDate.Year;
    //    return DateTime.Now.Year;
    //  }
    //  set
    //  {
    //    //Session[m_keyCurrentYear] = value;
    //  }
    //}
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
  }
}
