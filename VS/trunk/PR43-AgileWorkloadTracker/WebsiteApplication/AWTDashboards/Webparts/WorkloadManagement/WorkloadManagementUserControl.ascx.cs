//<summary>
//  Title   : Workload Management User Control
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL:$
//  $Id$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.AgileWorkloadTracker.Dashboards.Linq;
using CAS.AgileWorkloadTracker.DataModel.Linq;
using CAS.SharePoint;
using CAS.SharePoint.Linq;
using CAS.SharePoint.Web;
using Microsoft.SharePoint;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CAS.AgileWorkloadTracker.Dashboards.Webparts.WorkloadManagement
{
  /// <summary>
  /// Workload Management User Control
  /// </summary>
  [CLSCompliant(false)]
  public partial class WorkloadManagementUserControl : UserControl
  {
    public WorkloadManagementUserControl()
    {
      try
      {
        At = "LocalStateMachine";
        m_StateMachineEngine = new LocalStateMachine(this);
        At = "DataContextManagement";
        m_DataContext = new DataContextManagement<Entities>(this);
      }
      catch (Exception _ex)
      {
        GenericStateMachineEngine.ActionResult.Exception(_ex, "WorkloadManagementUserControl").CreateActionResultControl(x => this.Controls.Add(x));
      }
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
      public ControlState(ControlState oldControlState)
      {
        if (oldControlState == null)
          return;
        InterfaceState = oldControlState.InterfaceState;
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
          At = "InitializeMachine";
          m_StateMachineEngine.InitializeMachine();
          //Grid setup
          m_GridView.EmptyDataText = "No workload defined";
          m_GridView.Columns.Add(new BoundField() { DataField = "Hours", HeaderText = "Workload [h]" });
          m_GridView.Columns.Add(new BoundField() { DataField = "Project", HeaderText = "Project" });
          m_GridView.Columns.Add(new BoundField() { DataField = "Task", HeaderText = "Task" });
          //m_GridView.Columns.Add( new BoundField() { DataField = "Description", HeaderText = "Description", Visible = true } );
          string _urlFormat = @"~/sites/awt/Lists/WorkloadList/DispForm.aspx?ID={0}";
          m_GridView.Columns.Add(new HyperLinkField() { DataTextField = "Description", HeaderText = "Workload", DataNavigateUrlFields = new string[] { "ID" }, DataNavigateUrlFormatString = _urlFormat, Visible = true });
          m_GridView.Columns.Add(new BoundField() { DataField = "ID", HeaderText = "ID", Visible = false });
          m_GridView.DataKeyNames = new String[] { "ID" };
          //Calendar setup
          m_Calendar.SelectedDate = DateTime.Now.Date;
          m_Calendar.VisibleDate = DateTime.Now.Date;
          m_GridViewProjectSummary.AutoGenerateColumns = true;
          m_GridViewProjectSummary.DataKeyNames = new String[] { "Scope" };
          //DropDownList setup 
          At = "ShowUserInformation";
          if (ShowUserInformation())
          {
            Entities _edc = m_DataContext.DataContext;
            At = "FillupProjectDropDown";
            FillupProjectDropDown();
            At = "FillupMilestoneDropDown";
            FillupMilestoneDropDown();
            At = "FillupTaskaDropDown";
            FillupTaskDropDown();
            At = "FindForUser";
            FillupWorkflowGridView(_edc);
            At = "FillupGridViewProjectSummary";
            FillupGridViewProjectSummary(m_DataContext.DataContext);
          }
          else
            m_PanelAddEdit.Enabled = false;
        }
        m_ProjectDropDown.SelectedIndexChanged += new EventHandler(m_ProjectDropDown_SelectedIndexChanged);
        m_MilestoneDropDown.SelectedIndexChanged += m_MilestoneDropDown_SelectedIndexChanged;
        m_ButtonSave.Click += new EventHandler(m_StateMachineEngine.SaveButton_Click);
        m_ButtonAddNew.Click += new EventHandler(m_StateMachineEngine.NewButton_Click);
        m_ButtonCancel.Click += new EventHandler(m_StateMachineEngine.CancelButton_Click);
        m_ButtonEdit.Click += new EventHandler(m_StateMachineEngine.EditButton_Click);
        m_ButtonDelete.Click += new EventHandler(m_StateMachineEngine.DeleteButton_Click);
      }
      catch (ApplicationError _ax)
      {
        this.Controls.Add(_ax.CreateMessage(At, true));
      }
      catch (Exception _ex)
      {
        GenericStateMachineEngine.ActionResult.Exception(_ex, "Page_Load").CreateActionResultControl(x => this.Controls.Add(x));
      }
    }
    /// <summary>
    /// Loads the state of the control.
    /// </summary>
    /// <param name="savedState">The state.</param>
    protected override void LoadControlState(object savedState)
    {
      if (savedState != null)
      {
        m_ControlState = (ControlState)savedState;
        m_StateMachineEngine.InitializeMachine(m_ControlState.InterfaceState);
      }
      else
        m_StateMachineEngine.InitializeMachine();
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
      internal LocalStateMachine(WorkloadManagementUserControl parent)
      {
        m_Parent = parent;
      }
      internal void InitializeMachine(InterfaceState _ControlState)
      {
        m_Parent.m_ControlState.InterfaceState = _ControlState;
      }
      internal void InitializeMachine()
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
        return m_Parent.CreateNewWorkload();
      }
      protected override GenericStateMachineEngine.ActionResult Delete()
      {
        return m_Parent.Delete();
      }
      protected override void SetEnabled(GenericStateMachineEngine.ControlsSet buttons)
      {
        m_Parent.m_ControlState.SetEnabled = buttons;
      }
      protected override void ShowActionResult(GenericStateMachineEngine.ActionResult actionResult)
      {
        actionResult.CreateActionResultControl(x => m_Parent.Controls.Add(x));
      }
      protected override void SMError(GenericStateMachineEngine.InterfaceEvent interfaceEvent)
      {
        GenericStateMachineEngine.ActionResult.Exception(new ApplicationException("State Machine Error"), "SMError").CreateActionResultControl(x => m_Parent.Controls.Add(x));
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
    private GenericStateMachineEngine.ActionResult CreateNewWorkload()
    {
      if (!Page.IsValid)
        return GenericStateMachineEngine.ActionResult.NotValidated("Required information must be provided.");
      try
      {
        if (m_TaskDropDown.SelectedValue.IsNullOrEmpty())
          return GenericStateMachineEngine.ActionResult.NotValidated("You must select a task to create new workload");
        Tasks _task = m_DataContext.DataContext.Task.GetAtIndex<Tasks>(m_TaskDropDown.SelectedValue);
        At = "newOne";
        Entities _edc = m_DataContext.DataContext;
        Workload _newOne = new Workload()
        {
          AssignedTo = Me,
          Comments = m_WorkloadDescriptionTextBox.Text, //TODO not sure what to do
          Duration = 0,
          EndDate = m_WorkloadEndDateTimeControl.SelectedDate,
          EventCanceled = false,
          EventType = 0,
          FAllDayEvent = false,
          FRecurrence = false,
          Location = "In office",
          MasterSeriesItemID = 0,
          ReadOnly = false,
          RecurrenceData = "",
          RecurrenceID = DateTime.Today,
          StartDate = m_WorkloadStartDateTimeControl.SelectedDate,
          Title = m_WorkloadDescriptionTextBox.Text,
          Workload2ProjectTitle = SelectedProject,
          Workload2StageTitle = SelectedProject == null ? null : SelectedProject.Project2StageTitle,
          Workload2TaskID = _task,
          UID = new Guid(),
        };
        At = "InsertOnSubmit";
        _edc.Workload.InsertOnSubmit(_newOne);
        At = "SubmitChanges #1";
        m_DataContext.DataContext.SubmitChanges();
        _newOne.AssignedTo = Me;
        At = "SubmitChanges #2";
        m_DataContext.DataContext.SubmitChanges();
        FillupWorkflowGridView(_edc);
        FillupGridViewProjectSummary(_edc);
      }
      catch (ApplicationError _ax)
      {
        return GenericStateMachineEngine.ActionResult.Exception(_ax, "CreateNewWorkload at: " + At);
      }
      catch (Exception _ex)
      {
        return GenericStateMachineEngine.ActionResult.Exception(_ex, "CreateNewWorkload at: " + At);
      }
      return GenericStateMachineEngine.ActionResult.Success;
    }
    private GenericStateMachineEngine.ActionResult Delete()
    {
      try
      {
        Entities _edc = m_DataContext.DataContext;
        Workload _wkl = _edc.Workload.GetAtIndex<Workload>(m_GridView.SelectedDataKey.Value.ToString());
        m_GridView.SelectedIndex = -1;
        m_DataContext.DataContext.Workload.RecycleOnSubmit(_wkl);
        m_DataContext.DataContext.SubmitChanges();
        FillupWorkflowGridView(_edc);
      }
      catch (Exception _ex)
      {
        return GenericStateMachineEngine.ActionResult.Exception(_ex, "Delete");
      }
      return GenericStateMachineEngine.ActionResult.Success;
    }
    private void SetEnabled(GenericStateMachineEngine.ControlsSet controlsSet)
    {
      m_WorkloadEndDateTimeControl.Enabled = (controlsSet & GenericStateMachineEngine.ControlsSet.EditModeOn) != 0;
      m_WorkloadDescriptionTextBox.Enabled = (controlsSet & GenericStateMachineEngine.ControlsSet.EditModeOn) != 0;
      bool _viewMode = (controlsSet & GenericStateMachineEngine.ControlsSet.EditModeOn) == 0;
      //m_ProjectDropDown.Enabled = (_set & GenericStateMachineEngine.ControlsSet.EditModeOn) != 0;
      m_TaskDropDown.Enabled = !_viewMode;
      m_MilestoneDropDown.Enabled = !_viewMode;
      m_GridView.Enabled = _viewMode;
      m_Calendar.Enabled = _viewMode;
      //Buttons
      m_ButtonSave.Enabled = (controlsSet & GenericStateMachineEngine.ControlsSet.SaveOn) != 0;
      m_ButtonDelete.Enabled = (controlsSet & GenericStateMachineEngine.ControlsSet.DeleteOn) != 0;
      m_ButtonCancel.Enabled = (controlsSet & GenericStateMachineEngine.ControlsSet.CancelOn) != 0;
      m_ButtonEdit.Enabled = ((controlsSet & GenericStateMachineEngine.ControlsSet.EditOn) != 0);
      m_ButtonAddNew.Enabled = (controlsSet & GenericStateMachineEngine.ControlsSet.NewOn) != 0;
    }
    private GenericStateMachineEngine.ActionResult Show()
    {
      try
      {
        UpdateWorkload();
      }
      catch (Exception _ex)
      {
        return GenericStateMachineEngine.ActionResult.Exception(_ex, "GenericStateMachineEngine.ActionResult");
      };
      return GenericStateMachineEngine.ActionResult.Success;
    }
    private GenericStateMachineEngine.ActionResult Update()
    {
      try
      {
        int _idx = m_GridView.SelectedIndex;
        Entities _edc = m_DataContext.DataContext;
        Workload _wkl = _edc.Workload.GetAtIndex<Workload>(m_GridView.SelectedDataKey.Value.ToString());
        Tasks _task = m_TaskDropDown.GetSelected<Tasks>(m_DataContext.DataContext.Task);
        _wkl.Title = m_WorkloadDescriptionTextBox.Text;
        _wkl.EndDate = m_WorkloadEndDateTimeControl.SelectedDate;
        _wkl.StartDate = m_WorkloadStartDateTimeControl.SelectedDate;
        _wkl.Workload2ProjectTitle = SelectedProject;
        _wkl.Workload2StageTitle = SelectedProject == null ? null : SelectedProject.Project2StageTitle;
        _wkl.Workload2TaskID = _task;
        _task.CalculateWorkload();
        m_DataContext.DataContext.SubmitChanges();
        FillupWorkflowGridView(_edc);
        m_GridView.SelectedIndex = _idx;
        UpdateWorkload();
        FillupGridViewProjectSummary(_edc);
      }
      catch (ApplicationError _ae)
      {
        return GenericStateMachineEngine.ActionResult.Exception(_ae, "GenericStateMachineEngine.ActionResult");
      }
      catch (Exception _ex)
      {
        return GenericStateMachineEngine.ActionResult.Exception(_ex, "GenericStateMachineEngine.ActionResult");
      }
      return GenericStateMachineEngine.ActionResult.Success;
    }
    private void ClearUserInterface()
    {
      Entities edc = m_DataContext.DataContext;
      m_GridView.SelectedIndex = -1;
      m_WorkloadDescriptionTextBox.Text = String.Empty;
      Workload _lastWorkload = (from _wlx in Me.Workload(edc) where _wlx.StartDate.GetValueOrDefault().Date == DateTime.Today.Date orderby _wlx.StartDate descending select _wlx).FirstOrDefault();
      DateTime _lastActivityTime = DateTime.Today.Date + TimeSpan.FromHours(8);
      if (_lastWorkload != null && _lastActivityTime < _lastWorkload.EndDate.Value)
        _lastActivityTime = _lastWorkload.EndDate.Value;
      m_WorkloadEndDateTimeControl.SelectedDate = DateTime.Now;
      m_WorkloadStartDateTimeControl.SelectedDate = _lastActivityTime;
    }
    #endregion

    #region helpers
    private double TextBoxToHours(TextBox textBox, string label)
    {
      try
      {
        return Convert.ToDouble(textBox.Text);
      }
      catch (Exception)
      {
        throw new ApplicationException(String.Format("{0} TexBox has wrong number", label));
      }
    }
    private bool ShowUserInformation()
    {
      if (Me == null)
      {
        this.Controls.Add(new Literal() { Text = String.Format(CAS.SharePoint.Web.CommonDefinitions.ErrorMessageFormat, "User not recognized - you must be added to the Recourses") });
        return false;
      }
      else
      {
        this.Controls.Add(new Literal() { Text = String.Format(CAS.SharePoint.Web.CommonDefinitions.ErrorMessageFormat, "Welcome: " + Me.Name) });
        return true;
      }
    }
    private void FillupWorkflowGridView(Entities edc)
    {
      if (Me == null)
        m_GridView.DataSource = null;
      else
      {
        At = "m_GridView.DataSource";
        m_GridView.DataSource = from _workload in Me.Workload(this.m_DataContext.DataContext)
                                where _workload.StartDate.Value.Date == m_Calendar.SelectedDate.Date
                                select new
                                {
                                  Hours = _workload.MyHours,
                                  Project = _workload.Workload2ProjectTitle == null ? m_SelectProjectDropDownEntry : _workload.Workload2ProjectTitle.Title,
                                  Task = _workload.Workload2TaskID == null ? m_SelectTaskDropDownEntry : _workload.Workload2TaskID.Title,
                                  Description = _workload.Title,
                                  ID = _workload.Id
                                };
      }
      m_GridView.DataBind();
      ClearUserInterface();
    }
    private void FillupTaskDropDown()
    {
      m_TaskDropDown.Items.Clear();
      if (SelectedMilestone == null)
        m_TaskDropDown.Items.Add(new ListItem(m_SelectMilestoneDropDownEntry, String.Empty) { Selected = true });
      else
      {
        IQueryable<Tasks> _tasks = from _task in SelectedMilestone.Tasks0
                                   let _status = _task.Task2StatusTitle
                                   where (_status == null ? true : _status.Active.GetValueOrDefault(true)) &&
                                         (_task.AssignedTo == null || _task.AssignedTo == Me)
                                   orderby _task.Title ascending
                                   select _task;
        m_TaskDropDown.EntityListDataSource(_tasks);
      }
    }
    private void FillupMilestoneDropDown()
    {
      m_MilestoneDropDown.Items.Clear();
      if (SelectedProject == null)
        m_MilestoneDropDown.Items.Add(new ListItem(m_SelectProjectDropDownEntry, String.Empty) { Selected = true });
      else
      {
        IQueryable<Milestone> _milestones = from _milestone in SelectedProject.Milestone
                                            where _milestone.Active.GetValueOrDefault(false)
                                            orderby _milestone.SortOrder ascending
                                            select _milestone;
        m_MilestoneDropDown.EntityListDataSource<Milestone>(_milestones);
        Milestone _default = (from _amx in _milestones
                              where _amx.Default.GetValueOrDefault(false)
                              select _amx).FirstOrDefault();
        m_MilestoneDropDown.SelectItem4Element(_default);
      }
    }
    private void FillupProjectDropDown()
    {
      m_ProjectDropDown.Items.Clear();
      m_ProjectDropDown.Items.Add(new ListItem(m_SelectProjectDropDownEntry, String.Empty) { Selected = true });
      if (Me == null || MyProjects == null)
        return;
      foreach (var _row2 in MyProjects)
        m_ProjectDropDown.Items.Add(new ListItem(_row2.Title, _row2.Id.ToString()));
    }
    private void FillupGridViewProjectSummary(Entities edc)
    {
      m_GridViewProjectSummary.DataSource = WorkloadSummary.WorkloadSummaryList(edc, MyProjects, Me);
      m_GridViewProjectSummary.DataBind();
    }
    private void UpdateWorkload()
    {
      if (m_GridView.SelectedIndex < 0)
        return;
      string _selection = m_GridView.SelectedDataKey.Value.ToString();
      Workload _workload = m_DataContext.DataContext.Workload.GetAtIndex<Workload>(_selection);
      m_WorkloadDescriptionTextBox.Text = _workload.Title;
      m_WorkloadStartDateTimeControl.SelectedDate = _workload.StartDate.GetValueOrDefault(Extensions.SPMinimum);
      m_WorkloadEndDateTimeControl.SelectedDate = _workload.EndDate.GetValueOrDefault(DateTime.Now);
      m_ProjectDropDown.SelectItem4Element(_workload.Workload2ProjectTitle);
      FillupMilestoneDropDown();
      m_MilestoneDropDown.SelectItem4Element(_workload.Workload2TaskID != null ? _workload.Workload2TaskID.Task2MilestoneResolvedInTitle : null);
      FillupTaskDropDown();
      m_TaskDropDown.SelectItem4Element(_workload.Workload2TaskID);
    }
    #endregion

    #region event handlers
    private void m_ProjectDropDown_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        if (SelectedProject == null || Me == null)
          return;
        FillupMilestoneDropDown();
        FillupTaskDropDown();
      }
      catch (Exception _ex)
      {
        GenericStateMachineEngine.ActionResult.Exception(_ex, "m_ProjectDropDown_SelectedIndexChanged").CreateActionResultControl(x => this.Controls.Add(x));
      }
    }
    private void m_MilestoneDropDown_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        if (SelectedProject == null || Me == null)
          return;
        FillupTaskDropDown();
      }
      catch (Exception _ex)
      {
        GenericStateMachineEngine.ActionResult.Exception(_ex, "m_MilestoneDropDown_SelectedIndexChanged").CreateActionResultControl(x => this.Controls.Add(x));
      }
    }
    protected void m_GridView_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        UpdateWorkload();
      }
      catch (Exception _ex)
      {
        GenericStateMachineEngine.ActionResult.Exception(_ex, "m_GridView_SelectedIndexChanged").CreateActionResultControl(x => this.Controls.Add(x));
      }
    }
    /// <summary>
    /// Calendar Selection Changed method
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void m_Calendar_SelectionChanged(object sender, EventArgs e)
    {
      try
      {
        m_WorkloadEndDateTimeControl.SelectedDate = m_Calendar.SelectedDate.Date + TimeSpan.FromHours(16);
        m_WorkloadStartDateTimeControl.SelectedDate = m_Calendar.SelectedDate.Date + TimeSpan.FromHours(8);
        m_WorkloadDescriptionTextBox.Text = String.Empty;
        if (Me == null)
          return;
        double _hoursADay = (from _workload in Me.Workload(m_DataContext.DataContext)
                             where _workload.StartDate.Value.Date == m_Calendar.SelectedDate.Date
                             select _workload).Sum(_x => _x.MyHours);
        string _template = "You have reported {0} working hours for the selected day {1:D}";
        m_HoursADayLabel.Text = String.Format(_template, _hoursADay, m_Calendar.SelectedDate.Date);
        FillupWorkflowGridView(m_DataContext.DataContext);
      }
      catch (Exception _ex)
      {
        GenericStateMachineEngine.ActionResult.Exception(_ex, "m_Calendar_SelectionChanged").CreateActionResultControl(x => this.Controls.Add(x));
      }
    }
    #endregion

    #region vars
    private string At { get; set; }
    private const string m_SelectProjectDropDownEntry = "  -- select project -- ";
    private const string m_SelectMilestoneDropDownEntry = "  -- select milestone -- ";
    private const string m_SelectTaskDropDownEntry = "  -- select task -- ";
    private const string m_keyCurrentYear = "CurrentYear";
    private DataContextManagement<Entities> m_DataContext = null;
    private ControlState m_ControlState = new ControlState(null);
    private Users b_Me = null;
    private Users Me
    {
      get
      {
        try
        {
          if (b_Me == null)
            b_Me = Users.FindSPUser(m_DataContext.DataContext, SPContext.Current.Web.CurrentUser);
          return b_Me;
        }
        catch (Exception xe)
        {
          throw new ApplicationError("Me", "", xe.Message, xe);
        }
      }
    }
    private Milestone p_Milestone = null;
    private Milestone SelectedMilestone
    {
      get
      {
        if (p_Milestone == null && m_ProjectDropDown.SelectedIndex > 0)
          p_Milestone = m_DataContext.DataContext.Milestone.GetAtIndex<Milestone>(m_MilestoneDropDown.SelectedValue);
        return p_Milestone;
      }
    }
    private Projects p_Projects = null;
    private Projects SelectedProject
    {
      get
      {
        if (p_Projects == null && m_ProjectDropDown.SelectedIndex > 0)
          p_Projects = m_DataContext.DataContext.Projects.GetAtIndex<Projects>(m_ProjectDropDown.SelectedValue);
        return p_Projects;
      }
    }
    private IQueryable<Projects> p_MyProjects = null;
    private IQueryable<Projects> MyProjects
    {
      get
      {
        if (p_MyProjects == null)
          p_MyProjects = from _association in Me.Estimation(m_DataContext.DataContext) let _pidx = _association.Estimation2ProjectTitle where _pidx != null && _pidx.Active.GetValueOrDefault(true) select _pidx;
        return p_MyProjects;
      }
    }
    #endregion

  }
}
