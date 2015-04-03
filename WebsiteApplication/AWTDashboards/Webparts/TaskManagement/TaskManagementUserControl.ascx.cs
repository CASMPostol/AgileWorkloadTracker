//<summary>
//  Title   : Task Management User Control
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

using CAS.AgileWorkloadTracker.DataModel.Linq;
using CAS.SharePoint;
using CAS.SharePoint.Linq;
using CAS.SharePoint.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using TaskType = CAS.AgileWorkloadTracker.DataModel.Linq.Type;

namespace CAS.AgileWorkloadTracker.Dashboards.Webparts.TaskManagement
{
  /// <summary>
  /// Task Management User Control
  /// </summary>
  [CLSCompliant(false)]
  public partial class TaskManagementUserControl : UserControl
  {
    #region creator
    public TaskManagementUserControl()
    {
      try
      {
        At = "LocalStateMachine";
        m_StateMachineEngine = new LocalStateMachine(this);
        At = "DataContextManagement";
        m_DataContext = new DataContextManagement<Entities>(this);
        m_ControlState = new ControlState(new EventHandler<EventArgs>(m_ControlState_TaskRefresh));
      }
      catch (Exception _ex)
      {
        GenericStateMachineEngine.ActionResult.Exception(_ex, "WorkloadManagementUserControl").CreateActionResultControl(x => Controls.Add(x));
      }
    }
    #endregion

    #region public
    internal void SetInterconnectionData(Dictionary<ConnectionSelector, IWebPartRow> m_ProvidersDictionary)
    {
      foreach (var _item in m_ProvidersDictionary)
      {
        try
        {
          switch (_item.Key)
          {
            case ConnectionSelector.ProjectInterconnection:
              new ProjectInterconnectionData().SetRowData(_item.Value, m_StateMachineEngine.NewDataEventHandler);
              break;
            case ConnectionSelector.TaskInterconnection:
              new TaskInterconnectionData().SetRowData(_item.Value, m_StateMachineEngine.NewDataEventHandler);
              break;
            default:
              break;
          }

        }
        catch (Exception _ex)
        {
          At = _item.Key.ToString();
          GenericStateMachineEngine.ActionResult.Exception(_ex, "SetInterconnectionData").CreateActionResultControl(x => Controls.Add(x));
        }
      }
    }
    #endregion

    #region UserControl override
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
          At = "InitMahine";
          m_StateMachineEngine.InitializeMachine();
          Entities _ent = m_DataContext.DataContext;
          At = "m_TypeDropDown";
          m_TypeDropDown.EntityListDataSource<TaskType>(_ent.Type);
          At = "m_ResolutionDropDown";
          m_ResolutionDropDown.EntityListDataSource<Resolution>(_ent.Resolution);
          At = "m_PriorityDropDown";
          m_PriorityDropDown.EntityListDataSource<Priority>(_ent.Priority);
          At = "m_StatusDropDown";
          m_StatusDropDown.EntityListDataSource<Status>(_ent.Status);
          m_ControlState.PageLoad = true;
        }
        At = "Events handlers";
        m_ButtonSave.Click += new EventHandler(m_StateMachineEngine.SaveButton_Click);
        m_ButtonAddNew.Click += new EventHandler(m_StateMachineEngine.NewButton_Click);
        m_ButtonCancel.Click += new EventHandler(m_StateMachineEngine.CancelButton_Click);
        m_ButtonEdit.Click += new EventHandler(m_StateMachineEngine.EditButton_Click);
        m_ButtonDelete.Click += new EventHandler(m_StateMachineEngine.DeleteButton_Click);
        m_RequirementDropDown.SelectedIndexChanged += new EventHandler(m_RequirementDropDown_SelectedIndexChanged);
        m_MilestoneDropDown.SelectedIndexChanged += new EventHandler(m_MilestoneDropDown_SelectedIndexChanged);
        m_ShowAllMilestonesCheckBox.CheckedChanged += new EventHandler(m_ShowAllMilestonesCheckBox_CheckedChanged);
      }
      catch (ApplicationError _ax)
      {
        this.Controls.Add(_ax.CreateMessage(At, true));
      }
      catch (Exception _ex)
      {
        GenericStateMachineEngine.ActionResult.Exception(_ex, "Page_Load").CreateActionResultControl(x => Controls.Add(x));
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
        m_ControlState.TaskRefresh += new EventHandler<EventArgs>(m_ControlState_TaskRefresh);
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
      if (m_ControlState.ProjectID.IsNullOrEmpty())
      {
        m_ButtonAddNew.Enabled = false;
      }
      if (m_ControlState.TaskID.IsNullOrEmpty())
      {
        m_ButtonDelete.Enabled = false;
        m_ButtonEdit.Enabled = false;
      }
      base.OnPreRender(e);
    }
    #endregion

    #region SetInterconnectionData
    internal void SetInterconnectionData(ProjectInterconnectionData e)
    {
      if (e.ID.IsNullOrEmpty() || m_ControlState.ProjectID.Contains(e.ID))
        return;
      m_ControlState.ProjectID = e.ID;
      ProjectChanged(CurrentProject);
    }
    internal void SetInterconnectionData(TaskInterconnectionData e)
    {
      m_ControlState.TaskID = e.ID;
    }
    #endregion

    #region private

    #region State machine
    private LocalStateMachine m_StateMachineEngine;
    private class LocalStateMachine : GenericStateMachineEngine
    {
      #region public
      internal LocalStateMachine(TaskManagementUserControl parent)
      {
        m_Parent = parent;
      }
      internal void InitializeMachine(InterfaceState controlState)
      {
        m_Parent.m_ControlState.InterfaceState = controlState;
      }
      internal void InitializeMachine()
      {
        m_Parent.m_ControlState.InterfaceState = InterfaceState.ViewState;
        EnterState();
      }
      #endregion

      #region NewDataEventHandlers
      internal void NewDataEventHandler(object sender, ProjectInterconnectionData e)
      {
        switch (CurrentMachineState)
        {
          case InterfaceState.ViewState:
            m_Parent.SetInterconnectionData(e);
            break;
          case InterfaceState.EditState:
          case InterfaceState.NewState:
          default:
            break;
        }
      }
      internal void NewDataEventHandler(object sender, TaskInterconnectionData e)
      {
        switch (CurrentMachineState)
        {
          case InterfaceState.ViewState:
            m_Parent.SetInterconnectionData(e);
            break;
          case InterfaceState.EditState:
          case InterfaceState.NewState:
          default:
            break;
        }
      }
      #endregion

      #region StateMachine override
      protected override void ClearUserInterface()
      {
        m_Parent.ClearUserInterface();
      }
      protected override GenericStateMachineEngine.ActionResult Create()
      {
        return m_Parent.CreateTask();
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
      protected override void EnterState()
      {
        base.EnterState();
        switch (CurrentMachineState)
        {
          case InterfaceState.ViewState:
            m_Parent.m_ControlState_TaskRefresh(this, new EventArgs());
            break;
          case InterfaceState.EditState:
            m_Parent.ClearTaskComments();
            break;
          case InterfaceState.NewState:
            break;
        }

      }
      #endregion

      #region private
      private TaskManagementUserControl m_Parent { get; set; }
      #endregion

    }
    private GenericStateMachineEngine.ActionResult CreateTask()
    {
      if (!Page.IsValid)
        return GenericStateMachineEngine.ActionResult.NotValidated("Required information must be provided.");
      if (m_TaskCommentsTextBox.Text.Length <= 5)
        return GenericStateMachineEngine.ActionResult.NotValidated("The task comment length must be 5 characters at least");
      try
      {
        if (m_ControlState.ProjectID.IsNullOrEmpty())
          return GenericStateMachineEngine.ActionResult.NotValidated("Project is not selected.");
        if (m_TaskTitleTextBox.Text.Length <= 5)
          return GenericStateMachineEngine.ActionResult.NotValidated("The Title length must be at least 5 characters.");
        if (m_TaskCommentsTextBox.Text.Length <= 5)
          return GenericStateMachineEngine.ActionResult.NotValidated("The task comment length must be at least 5 characters.");
        Entities _ent = m_DataContext.DataContext;
        Milestone _milestone = m_MilestoneDropDown.GetSelected<Milestone>(_ent.Milestone);
        Requirements _requirements = m_RequirementDropDown.GetSelected<Requirements>(_ent.Requirements);
        if (_requirements.Requirements2MilestoneTitle != _milestone)
          return GenericStateMachineEngine.ActionResult.NotValidated("Inconsistency validation violated: task and requirement must be coupled with the same milestone.");
        Projects _project = _ent.Projects.GetAtIndex<Projects>(m_ControlState.ProjectID);
        TaskType _taskType = m_TypeDropDown.GetSelected<TaskType>(_ent.Type);
        Resolution _resolution = m_ResolutionDropDown.GetSelected<Resolution>(_ent.Resolution);
        Priority _priority = m_PriorityDropDown.GetSelected<Priority>(_ent.Priority);
        Status _status = m_StatusDropDown.GetSelected<Status>(_ent.Status);
        Users _resource = (Users)m_AsignedToDropDown.GetSelected<Item>(_ent.UserInformationList);
        Category _category = m_CategoryDropDown.GetSelected<Category>(_ent.Category);
        Milestone _version = m_VersionDropDown.GetSelected<Milestone>(_ent.Milestone);
        At = "newOne";
        Tasks _newTask = new Tasks()
        {
          Active = true,
          AssignedTo = _resource,
          BaselineStart = DateTime.Now.Date,
          BaselineEnd = DateTime.Now.Date,
          Body = m_TaskCommentsTextBox.Text,
          Hours = 0,
          PercentComplete = 0,
          Priority = 0,
          StartDate = DateTime.Now.Date,
          TaskDueDate = m_DueDateDateTimeControl.SelectedDate,
          TaskStatus = TaskStatus.NotStarted,
          Task2CategoryTitle = _category,
          Task2MilestoneDefinedInTitle = _version,
          Task2MilestoneResolvedInTitle = _milestone,
          Task2ProjectTitle = _project,
          Task2RequirementsTitle = _requirements,
          Task2SPriorityTitle = _priority,
          Task2SResolutionTitle = _resolution,
          Task2StatusTitle = _status,
          Task2TypeTitle = _taskType,
          TaskEnd = DateTime.Today.Date,
          TaskStart = DateTime.Today.Date,
          Title = m_TaskTitleTextBox.Text
        };
        UpdateDueData(_newTask);
        At = "InsertOnSubmit";
        _ent.Task.InsertOnSubmit(_newTask);
        if (_milestone != null)
          _milestone.MilestoneEnd = _newTask.TaskEnd;
        At = "SubmitChanges #1";
        _ent.SubmitChanges();
      }
      catch (GenericStateMachineEngine.ActionResult actionResult)
      {
        return actionResult;
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
        //Workload _wkl = Element.GetAtIndex<Workload>( m_DataContext.DataContext.Workload, m_GridView.SelectedDataKey.Value.ToString() );
        //m_GridView.SelectedIndex = -1;
        //m_DataContext.DataContext.Workload.RecycleOnSubmit( _wkl );
        //m_DataContext.DataContext.SubmitChanges();
        //FillupWorkflowGridView();
      }
      catch (Exception _ex)
      {
        return GenericStateMachineEngine.ActionResult.Exception(_ex, "Delete");
      }
      return GenericStateMachineEngine.ActionResult.Success;
    }
    private void SetEnabled(GenericStateMachineEngine.ControlsSet controlsSet)
    {
      //Buttons
      m_ButtonSave.Enabled = (controlsSet & GenericStateMachineEngine.ControlsSet.SaveOn) != 0;
      m_ButtonDelete.Enabled = (controlsSet & GenericStateMachineEngine.ControlsSet.DeleteOn) != 0;
      m_ButtonCancel.Enabled = (controlsSet & GenericStateMachineEngine.ControlsSet.CancelOn) != 0;
      m_ButtonEdit.Enabled = (controlsSet & GenericStateMachineEngine.ControlsSet.EditOn) != 0;
      m_ButtonAddNew.Enabled = (controlsSet & GenericStateMachineEngine.ControlsSet.NewOn) != 0;
      m_DropDownPanel.Enabled = (controlsSet & GenericStateMachineEngine.ControlsSet.EditModeOn) != 0;
      m_TaskTitleTextBox.Enabled = (controlsSet & GenericStateMachineEngine.ControlsSet.NewModeOn) != 0;
      m_TaskCommentsTextBox.Enabled = (controlsSet & GenericStateMachineEngine.ControlsSet.EditModeOn) != 0;
    }
    private GenericStateMachineEngine.ActionResult Show()
    {
      try
      {
        //UpdateWorkload();
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
        CurrentTask.TaskEnd = DateTime.Now;
        Entities _ent = m_DataContext.DataContext;
        if (CurrentTask.Task2CategoryTitle == null || CurrentTask.Task2CategoryTitle.Id != m_CategoryDropDown.SelectedValue.String2Int())
          CurrentTask.Task2CategoryTitle = m_CategoryDropDown.GetSelected<Category>(_ent.Category);
        if (CurrentTask.Task2MilestoneDefinedInTitle == null || CurrentTask.Task2MilestoneDefinedInTitle.Id != m_VersionDropDown.SelectedValue.String2Int())
          CurrentTask.Task2MilestoneDefinedInTitle = m_VersionDropDown.GetSelected<Milestone>(_ent.Milestone);
        if (CurrentTask.Task2RequirementsTitle == null || CurrentTask.Task2RequirementsTitle.Id != m_RequirementDropDown.SelectedValue.String2Int())
          CurrentTask.Task2RequirementsTitle = m_RequirementDropDown.GetSelected<Requirements>(_ent.Requirements);
        if (CurrentTask.Task2MilestoneResolvedInTitle == null || CurrentTask.Task2MilestoneResolvedInTitle != CurrentTask.Task2RequirementsTitle.Requirements2MilestoneTitle)
          CurrentTask.Task2MilestoneResolvedInTitle = CurrentTask.Task2RequirementsTitle.Requirements2MilestoneTitle;
        if (CurrentTask.AssignedTo == null || CurrentTask.AssignedTo.Id != m_AsignedToDropDown.SelectedValue.String2Int())
          CurrentTask.AssignedTo = (Users)m_AsignedToDropDown.GetSelected<Item>(_ent.UserInformationList);
        if (CurrentTask.Task2SPriorityTitle == null || CurrentTask.Task2SPriorityTitle.Id != m_PriorityDropDown.SelectedValue.String2Int())
          CurrentTask.Task2SPriorityTitle = m_PriorityDropDown.GetSelected<Priority>(_ent.Priority);
        if (CurrentTask.Task2SResolutionTitle == null || CurrentTask.Task2SResolutionTitle.Id != m_ResolutionDropDown.SelectedValue.String2Int())
          CurrentTask.Task2SResolutionTitle = m_ResolutionDropDown.GetSelected<Resolution>(_ent.Resolution);
        if (CurrentTask.Task2StatusTitle == null || CurrentTask.Task2StatusTitle.Id != m_StatusDropDown.SelectedValue.String2Int())
        {
          CurrentTask.Task2StatusTitle = m_StatusDropDown.GetSelected<Status>(_ent.Status);
          CurrentTask.Active = CurrentTask.Task2StatusTitle == null ? true : CurrentTask.Task2StatusTitle.Active.GetValueOrDefault(true);
        }
        if (CurrentTask.Task2TypeTitle == null || CurrentTask.Task2TypeTitle.Id != m_TypeDropDown.SelectedValue.String2Int())
          CurrentTask.Task2TypeTitle = m_TypeDropDown.GetSelected<CAS.AgileWorkloadTracker.DataModel.Linq.Type>(_ent.Type);
        if (CurrentTask.Task2MilestoneResolvedInTitle != null)
          CurrentTask.Task2MilestoneResolvedInTitle.MilestoneEnd = CurrentTask.TaskEnd;
        if (m_TaskCommentsTextBox.Text.Length > 0)
        {
          Workload _newComment = new Workload()
          {
            AssignedTo = null, //TODO
            Category = null, //TODO
            Comments = m_TaskCommentsTextBox.Text,
            Duration = 0,
            EndDate = m_WorkloadEndDateTimeControl.SelectedDate,
            EventCanceled = false,
            EventType = 0,
            FAllDayEvent = false,
            FRecurrence = false,
            Location = null, //TODO add control to provide information about Location
            ReadOnly = true,
            StartDate = m_WorkloadStartDateTimeControl.SelectedDate,
            Title = m_TaskCommentsTextBox.Text,
            Workload2ProjectTitle = CurrentTask.Task2ProjectTitle,
            Workload2StageTitle = CurrentTask.Task2ProjectTitle.Project2StageTitle,
            Workload2TaskID = CurrentTask,
          };
          _ent.Workload.InsertOnSubmit(_newComment);
        }
        UpdateDueData(CurrentTask);
        _ent.SubmitChanges();
      }
      catch (GenericStateMachineEngine.ActionResult _ar)
      {
        return _ar;
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
    private void UpdateDueData(Tasks task)
    {
      if (m_DueDateDateTimeControl.IsDateEmpty || (task.BaselineEnd.HasValue && task.BaselineEnd == m_DueDateDateTimeControl.SelectedDate))
        return;
      Milestone _milestone = task.Task2MilestoneResolvedInTitle;
      if (_milestone != null)
      {
        if (_milestone.BaselineEnd.HasValue && _milestone.BaselineEnd < m_DueDateDateTimeControl.SelectedDate.Date)
        {
          m_DueDateDateTimeControl.SelectedDate = _milestone.BaselineEnd.Value;
          throw GenericStateMachineEngine.ActionResult.NotValidated("The DueData exceeds the deadline of the current milestone - change the milestone or adjust this time.");
        }
      }
      task.BaselineEnd = m_DueDateDateTimeControl.SelectedDate.Date;
    }
    private void ClearUserInterface()
    {
      SetTargetForProject();
      m_ShowAllMilestonesCheckBox.Checked = false;
      FillupRequirementDropDown4Milestone(m_DataContext.DataContext);
      m_TaskCommentsTextBox.Text = String.Empty;
      m_TaskTitleTextBox.Text = String.Empty;
      m_DueDateDateTimeControl.ClearSelection();
    }
    #endregion

    #region vars
    [Serializable]
    private class ControlState
    {
      #region state fields
      internal GenericStateMachineEngine.ControlsSet SetEnabled = 0;
      public GenericStateMachineEngine.InterfaceState InterfaceState = GenericStateMachineEngine.InterfaceState.ViewState;
      public string p_ProjectID = String.Empty;
      public string p_TaskID = String.Empty;
      [NonSerialized()]
      internal bool PageLoad = false;
      public string TaskID
      {
        get { return p_TaskID; }
        set
        {
          if ((p_ProjectIDChanged && !PageLoad))
            return;
          if (p_TaskID.CompareTo(value) == 0)
            return;
          p_TaskID = value;
          if (TaskRefresh == null)
            return;
          TaskRefresh(this, new EventArgs());
        }
      }
      public string ProjectID
      {
        get { return p_ProjectID; }
        set
        {
          p_ProjectID = value;
          p_TaskID = String.Empty;
          p_ProjectIDChanged = true;
        }
      }
      #endregion

      #region public
      [field: NonSerialized()]
      internal event EventHandler<EventArgs> TaskRefresh;
      public ControlState(EventHandler<EventArgs> taskRefresh)
      {
        TaskRefresh += taskRefresh;
      }
      #endregion

      #region private
      [NonSerialized]
      private bool p_ProjectIDChanged = false;
      #endregion
    }
    private string At { get; set; }
    private DataContextManagement<Entities> m_DataContext = null;
    private ControlState m_ControlState = null;
    private Tasks p_CurrentTask = null;
    private Tasks CurrentTask
    {
      get
      {
        if (m_ControlState.TaskID.IsNullOrEmpty())
          return null;
        if (p_CurrentTask == null)
          p_CurrentTask = m_DataContext.DataContext.Task.GetAtIndex<Tasks>(m_ControlState.TaskID);
        return p_CurrentTask;
      }
    }
    private Projects p_CurrentProject = null;
    private Projects CurrentProject
    {
      get
      {
        if (m_ControlState.ProjectID.IsNullOrEmpty())
          return null;
        if (p_CurrentProject == null)
          p_CurrentProject = m_DataContext.DataContext.Projects.GetAtIndex<Projects>(m_ControlState.ProjectID);
        return p_CurrentProject;
      }
    }
    #endregion

    #region events handling
    private void m_MilestoneDropDown_SelectedIndexChanged(object sender, EventArgs e)
    {
      Milestone _cm = m_MilestoneDropDown.GetSelected<Milestone>(m_DataContext.DataContext.Milestone);
      if (_cm == null)
        m_ShowAllMilestonesCheckBox.Checked = true;
      Requirements _currentRequirement = m_RequirementDropDown.GetSelected<Requirements>(m_DataContext.DataContext.Requirements);
      if (m_ShowAllMilestonesCheckBox.Checked)
      {
        m_RequirementDropDown.EntityListDataSource(_cm.Milestone2ProjectTitle.Requirements);
        if (_currentRequirement == null)
        {
          Requirements _firs = _cm == null ? null : _cm.Requirements.FirstOrDefault<Requirements>();
          m_RequirementDropDown.SelectItem4Element(_firs);
        }
        else
          m_RequirementDropDown.SelectItem4Element(_currentRequirement);
      }
      else
        m_RequirementDropDown.EntityListDataSource(_cm.Requirements);
    }
    private void m_RequirementDropDown_SelectedIndexChanged(object sender, EventArgs e)
    {
      Requirements _cr = m_RequirementDropDown.GetSelected<Requirements>(m_DataContext.DataContext.Requirements);
      m_MilestoneDropDown.SelectItem4Element(_cr.Requirements2MilestoneTitle);
    }
    private void m_ShowAllMilestonesCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      int? _pId = m_ControlState.ProjectID.String2Int();
      if (!_pId.HasValue)
      {
        ProjectNotSelected(m_RequirementDropDown);
        ProjectNotSelected(m_MilestoneDropDown);
        return;
      }
      Entities _edc = this.m_DataContext.DataContext;
      Requirements _currentRequirement = m_RequirementDropDown.GetSelected<Requirements>(_edc.Requirements);
      if (m_ShowAllMilestonesCheckBox.Checked)
        m_RequirementDropDown.EntityListDataSource<Requirements>(_edc.ActiveRequirements(_pId.Value));
      else
        FillupRequirementDropDown4Milestone(_edc);
      m_RequirementDropDown.SelectItem4Element(_currentRequirement);
    }
    #endregion

    private void FillupRequirementDropDown4Milestone(Entities edc)
    {
      Milestone _cm = m_MilestoneDropDown.GetSelected<Milestone>(edc.Milestone);
      m_RequirementDropDown.EntityListDataSource(_cm == null ? null : _cm.Requirements);
    }
    private void m_ControlState_TaskRefresh(object sender, EventArgs e)
    {
      if (CurrentTask == null)
      {
        m_TaskTitleTextBox.Text = String.Empty;
        m_TaskCommentsTextBox.Text = String.Empty;
        return;
      }
      m_TaskTitleTextBox.Text = CurrentTask.Title;
      m_TaskCommentsTextBox.Text = CurrentTask.Body;
      if (CurrentTask.BaselineEnd.HasValue)
        m_DueDateDateTimeControl.SelectedDate = CurrentTask.BaselineEnd.Value;
      else
        m_DueDateDateTimeControl.ClearSelection();
      m_CategoryDropDown.SelectItem4Element(CurrentTask.Task2CategoryTitle);
      m_VersionDropDown.SelectItem4Element(CurrentTask.Task2MilestoneDefinedInTitle);
      m_MilestoneDropDown.SelectItem4Element(CurrentTask.Task2RequirementsTitle == null ? CurrentTask.Task2MilestoneResolvedInTitle : CurrentTask.Task2RequirementsTitle.Requirements2MilestoneTitle);
      if (!m_ShowAllMilestonesCheckBox.Checked && CurrentTask.Task2RequirementsTitle != null)
        m_RequirementDropDown.EntityListDataSource<Requirements>(CurrentTask.Task2RequirementsTitle.Requirements2MilestoneTitle.Requirements);
      m_RequirementDropDown.SelectItem4Element(CurrentTask.Task2RequirementsTitle);
      m_AsignedToDropDown.SelectItem4Element(CurrentTask.AssignedTo);
      m_ResolutionDropDown.SelectItem4Element(CurrentTask.Task2SResolutionTitle);
      m_StatusDropDown.SelectItem4Element(CurrentTask.Task2StatusTitle);
      m_TypeDropDown.SelectItem4Element(CurrentTask.Task2TypeTitle);
      m_PriorityDropDown.SelectItem4Element(CurrentTask.Task2SPriorityTitle);
    }
    private void ClearTaskComments()
    {
      m_TaskCommentsTextBox.Text = String.Empty;
    }
    private void ProjectChanged(Projects _project)
    {
      m_TaskCommentsTextBox.Text = String.Empty;
      m_TaskTitleTextBox.Text = String.Empty;
      if (_project == null)
      {
        ProjectNotSelected(m_AsignedToDropDown);
        ProjectNotSelected(m_CategoryDropDown);
        m_ProjectLabel.Text = String.Empty;
      }
      else
      {
        Entities _edc = this.m_DataContext.DataContext;
        m_ProjectLabel.Text = _project.Title;
        int _pId = _project.Id.Value;
        m_AsignedToDropDown.EntityListDataSource(from _estimation in _edc.Estimation
                                                 where _estimation.Estimation2ProjectTitle.Id == _pId
                                                 select _estimation.AssignedTo);
        m_CategoryDropDown.EntityListDataSource(_project.Category);
        m_VersionDropDown.EntityListDataSource(_project.Milestone);
        IQueryable<Milestone> _activeMilestones = from _milestone in _project.Milestone
                                                  where _milestone.Active.GetValueOrDefault(true)
                                                  select _milestone;
        m_MilestoneDropDown.EntityListDataSource(_activeMilestones);
        Milestone _firsMilestone = _activeMilestones.FirstOrDefault<Milestone>();
        m_MilestoneDropDown.SelectItem4Element(_firsMilestone);
        if (m_ShowAllMilestonesCheckBox.Checked)
        {
          m_RequirementDropDown.EntityListDataSource(_project.Requirements);
          m_RequirementDropDown.SelectItem4Element(_firsMilestone.Requirements.FirstOrDefault<Requirements>());
        }
        else
          m_RequirementDropDown.EntityListDataSource(_firsMilestone == null ? null : _firsMilestone.Requirements);
      }
    }

    private void SetTargetForProject()
    {
      return;
      //int? _pId = m_ControlState.ProjectID.String2Int();
      //if ( _pId.HasValue )
      //{
      //  Entities _dcxt = this.m_DataContext.DataContext;
      //  m_RequirementDropDown.EntityListDataSource( _dcxt.ActiveRequirements( _pId.Value ) );
      //  //TODO handle default requirements.
      //  //Milestone _milestoneDefault = ( from _mlstx in _mlstns where _mlstx.Default.HasValue && _mlstx.Default.Value select _mlstx ).FirstOrDefault<Milestone>();
      //  //if ( _milestoneDefault != null )
      //  //{
      //  //  string _idx = _milestoneDefault.Identyfikator.IntToString();
      //  //  m_VersionDropDown.SelectedValue = _idx;
      //  //  m_MilestoneDropDown.SelectedValue = _idx;
      //  //  if ( _milestoneDefault.MilestoneEnd.HasValue )
      //  //    m_DueDateDateTimeControl.SelectedDate = _milestoneDefault.MilestoneEnd.Value;
      //  //  else
      //  //    m_DueDateDateTimeControl.ClearSelection();
      //  //}
      //  //m_RequirementDropDown.EntityListDataSource( from _mlstnx in _activeMilestones
      //  //                                            from _rsrcx in _mlstnx.Requirements
      //  //                                            select _rsrcx );
      //}
      //else
      //{
      //  ProjectNotSelected( m_RequirementDropDown );
      //  ProjectNotSelected( m_VersionDropDown );
      //  ProjectNotSelected( m_MilestoneDropDown );
      //}
    }
    private void ProjectNotSelected(DropDownList dropDownList)
    {
      dropDownList.Items.Clear();
      dropDownList.Items.Add(new ListItem("-- Project not selected --", "-1"));
    }

    #endregion
  }
}
