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

using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CAS.AgileWorkloadTracker.Dashboards.Linq;
using CAS.AgileWorkloadTracker.Linq;
using CAS.SharePoint;
using CAS.SharePoint.Linq;
using CAS.SharePoint.Web;
using Microsoft.SharePoint;

namespace CAS.AgileWorkloadTracker.Dashboards.Webparts.WorkloadManagement
{
  /// <summary>
  /// Workload Management User Control
  /// </summary>
  [CLSCompliant( false )]
  public partial class WorkloadManagementUserControl: UserControl
  {
    public WorkloadManagementUserControl()
    {
      try
      {
        At = "LocalStateMachine";
        m_StateMachineEngine = new LocalStateMachine( this );
        At = "DataContextManagement";
        m_DataContext = new DataContextManagement<Entities>( this );
      }
      catch ( Exception _ex )
      {
        ShowActionResult( GenericStateMachineEngine.ActionResult.Exception( _ex, "WorkloadManagementUserControl" ) );
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
      public ControlState( ControlState _old )
      {
        if ( _old == null )
          return;
        InterfaceState = _old.InterfaceState;
      }
      #endregion
    }
    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
    protected override void OnInit( EventArgs e )
    {
      Page.RegisterRequiresControlState( this );
      base.OnInit( e );
    }
    /// <summary>
    /// Page Load method
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load( object sender, EventArgs e )
    {
      try
      {
        if ( !this.IsPostBack )
        {
          At = "InitMahine";
          m_StateMachineEngine.InitMahine();
          //Grid setup
          m_GridView.EmptyDataText = "No workload defined";
          m_GridView.Columns.Add( new BoundField() { DataField = "Hours", HeaderText = "Workload [h]" } );
          m_GridView.Columns.Add( new BoundField() { DataField = "Project", HeaderText = "Project" } );
          m_GridView.Columns.Add( new BoundField() { DataField = "Task", HeaderText = "Task" } );
          //m_GridView.Columns.Add( new BoundField() { DataField = "Description", HeaderText = "Description", Visible = true } );
          string _urlFormat = @"~/sites/awt/Lists/WorkloadList/DispForm.aspx?ID={0}";
          m_GridView.Columns.Add( new HyperLinkField() { DataTextField = "Description", HeaderText = "Workload", DataNavigateUrlFields = new string[] { "ID" }, DataNavigateUrlFormatString = _urlFormat, Visible = true } );
          m_GridView.Columns.Add( new BoundField() { DataField = "ID", HeaderText = "ID", Visible = false } );
          m_GridView.DataKeyNames = new String[] { "ID" };
          //Calendar setup
          m_Calendar.SelectedDate = DateTime.Now.Date;
          m_Calendar.VisibleDate = DateTime.Now.Date;
          m_GridViewProjectSummary.AutoGenerateColumns = true;
          m_GridViewProjectSummary.DataKeyNames = new String[] { "Scope" };
          //DropDownList'c setup 
          At = "ShouwUserInformation";
          if ( ShouwUserInformation() )
          {
            At = "FillupProjectDropDown";
            FillupProjectDropDown();
            At = "FillupMilestoneDropDown";
            FillupMilestoneDropDown();
            At = "FillupTaskaDropDown";
            FillupTaskaDropDown();
            At = "FindForUser";
            FillupWorkflowGridView();
            At = "FillupGridViewProjectSummary";
            FillupGridViewProjectSummary();
          }
          else
            m_PanelAddEdit.Enabled = false;
        }
        m_ProjectDropDown.SelectedIndexChanged += new EventHandler( m_ProjectDropDown_SelectedIndexChanged );
        m_MilestoneDropDown.SelectedIndexChanged += m_MilestoneDropDown_SelectedIndexChanged;
        m_ButtonSave.Click += new EventHandler( m_StateMachineEngine.SaveButton_Click );
        m_ButtonAddNew.Click += new EventHandler( m_StateMachineEngine.NewButton_Click );
        m_ButtonCancel.Click += new EventHandler( m_StateMachineEngine.CancelButton_Click );
        m_ButtonEdit.Click += new EventHandler( m_StateMachineEngine.EditButton_Click );
        m_ButtonDelete.Click += new EventHandler( m_StateMachineEngine.DeleteButton_Click );
      }
      catch ( ApplicationError _ax )
      {
        this.Controls.Add( _ax.CreateMessage( At, true ) );
      }
      catch ( Exception _ex )
      {
        ShowActionResult( GenericStateMachineEngine.ActionResult.Exception( _ex, "Page_Load" ) );
      }
    }
    /// <summary>
    /// Loads the state of the control.
    /// </summary>
    /// <param name="savedState">The state.</param>
    protected override void LoadControlState( object savedState )
    {
      if ( savedState != null )
      {
        m_ControlState = (ControlState)savedState;
        m_StateMachineEngine.InitMahine( m_ControlState.InterfaceState );
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
    protected override void OnPreRender( EventArgs e )
    {
      SetEnabled( m_ControlState.SetEnabled );
      if ( m_GridView.SelectedIndex == -1 )
      {
        m_ButtonDelete.Enabled = false;
        m_ButtonEdit.Enabled = false;
      }
      base.OnPreRender( e );
    }
    #endregion

    #region State machine
    private LocalStateMachine m_StateMachineEngine;
    private class LocalStateMachine: StateMachine
    {
      #region public
      internal LocalStateMachine( WorkloadManagementUserControl _parent )
      {
        m_Parent = _parent;
      }
      internal void InitMahine( InterfaceState _ControlState )
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
      protected override void SetEnabled( GenericStateMachineEngine.ControlsSet _buttons )
      {
        m_Parent.m_ControlState.SetEnabled = _buttons;
      }
      protected override void ShowActionResult( GenericStateMachineEngine.ActionResult _rslt )
      {
        m_Parent.ShowActionResult( _rslt );
      }
      protected override void SMError( GenericStateMachineEngine.InterfaceEvent interfaceEvent )
      {
        m_Parent.ShowActionResult( GenericStateMachineEngine.ActionResult.Exception( new ApplicationException( "State Machine Error" ), "SMError" ) );
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
          if ( m_Parent.m_ControlState.InterfaceState == value )
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
      if ( !Page.IsValid )
        return GenericStateMachineEngine.ActionResult.NotValidated( "Required information must be provided." );
      try
      {
        double _hours = TextBoxToHours( m_WorkloadHoursTextBox, m_WorkloadHoursLabel.Text );
        if ( m_TaskDropDown.SelectedValue.IsNullOrEmpty() )
          return GenericStateMachineEngine.ActionResult.NotValidated( "You must select a task to create new workload" );
        Tasks _task = Element.GetAtIndex<Tasks>( m_DataContext.DataContext.Task, m_TaskDropDown.SelectedValue );
        At = "newOne";
        Workload _newOne = new Workload()
        {
          Hours = _hours,
          Title = m_WorkloadDescriptionTextBox.Text,
          Workload2ProjectTitle = SelectedProject,
          Workload2StageTitle = SelectedProject == null ? null : SelectedProject.Project2StageTitle,
          Workload2TaskTitle = _task,
          WorkloadDate = m_Calendar.SelectedDate.Date == DateTime.Today ? DateTime.Now : m_Calendar.SelectedDate.Date,
          ReadOnly = false,
          WeekNumber = m_Calendar.SelectedDate.Date.WeekNumber(),
          Year = m_Calendar.SelectedDate.Date.Year
        };
        At = "InsertOnSubmit";
        m_DataContext.DataContext.Workload.InsertOnSubmit( _newOne );
        At = "SubmitChanges #1";
        m_DataContext.DataContext.SubmitChanges();
        _newOne.Workload2ResourcesTitle = Me;
        At = "SubmitChanges #2";
        m_DataContext.DataContext.SubmitChanges();
        FillupWorkflowGridView();
        FillupGridViewProjectSummary();
      }
      catch ( ApplicationError _ax )
      {
        return GenericStateMachineEngine.ActionResult.Exception( _ax, "CreateNewWokload at: " + At );
      }
      catch ( Exception _ex )
      {
        return GenericStateMachineEngine.ActionResult.Exception( _ex, "CreateNewWokload at: " + At );
      }
      return GenericStateMachineEngine.ActionResult.Success;
    }
    private GenericStateMachineEngine.ActionResult Delete()
    {
      try
      {
        Workload _wkl = Element.GetAtIndex<Workload>( m_DataContext.DataContext.Workload, m_GridView.SelectedDataKey.Value.ToString() );
        m_GridView.SelectedIndex = -1;
        m_DataContext.DataContext.Workload.RecycleOnSubmit( _wkl );
        m_DataContext.DataContext.SubmitChanges();
        FillupWorkflowGridView();
      }
      catch ( Exception _ex )
      {
        return GenericStateMachineEngine.ActionResult.Exception( _ex, "Delete" );
      }
      return GenericStateMachineEngine.ActionResult.Success;
    }
    private void SetEnabled( GenericStateMachineEngine.ControlsSet _set )
    {
      m_WorkloadHoursTextBox.Enabled = ( _set & GenericStateMachineEngine.ControlsSet.EditModeOn ) != 0;
      m_WorkloadDescriptionTextBox.Enabled = ( _set & GenericStateMachineEngine.ControlsSet.EditModeOn ) != 0;
      bool _viewMode = ( _set & GenericStateMachineEngine.ControlsSet.EditModeOn ) == 0;
      //m_ProjectDropDown.Enabled = (_set & GenericStateMachineEngine.ControlsSet.EditModeOn) != 0;
      m_TaskDropDown.Enabled = !_viewMode;
      m_MilestoneDropDown.Enabled = !_viewMode;
      m_GridView.Enabled = _viewMode;
      m_Calendar.Enabled = _viewMode;
      //Buttons
      m_ButtonSave.Enabled = ( _set & GenericStateMachineEngine.ControlsSet.SaveOn ) != 0;
      m_ButtonDelete.Enabled = ( _set & GenericStateMachineEngine.ControlsSet.DeleteOn ) != 0;
      m_ButtonCancel.Enabled = ( _set & GenericStateMachineEngine.ControlsSet.CancelOn ) != 0;
      m_ButtonEdit.Enabled = ( ( _set & GenericStateMachineEngine.ControlsSet.EditOn ) != 0 );
      m_ButtonAddNew.Enabled = ( _set & GenericStateMachineEngine.ControlsSet.NewOn ) != 0;
    }
    private GenericStateMachineEngine.ActionResult Show()
    {
      try
      {
        UpdateWorkload();
      }
      catch ( Exception _ex )
      {
        return GenericStateMachineEngine.ActionResult.Exception( _ex, "GenericStateMachineEngine.ActionResult" );
      };
      return GenericStateMachineEngine.ActionResult.Success;
    }
    private GenericStateMachineEngine.ActionResult Update()
    {
      double _hours = TextBoxToHours( m_WorkloadHoursTextBox, m_WorkloadHoursLabel.Text );
      try
      {
        int _indx = m_GridView.SelectedIndex;
        Workload _wkl = Element.GetAtIndex<Workload>( m_DataContext.DataContext.Workload, m_GridView.SelectedDataKey.Value.ToString() );
        Tasks _task = m_TaskDropDown.GetSelected<Tasks>( m_DataContext.DataContext.Task );
        _wkl.Hours = _hours;
        _wkl.Title = m_WorkloadDescriptionTextBox.Text;
        _wkl.WorkloadDate = m_Calendar.SelectedDate.Date;
        _wkl.Workload2ProjectTitle = SelectedProject;
        _wkl.Workload2StageTitle = SelectedProject == null ? null : SelectedProject.Project2StageTitle;
        _wkl.Workload2TaskTitle = _task;
        _task.CalculateWorkload();
        m_DataContext.DataContext.SubmitChanges();
        FillupWorkflowGridView();
        m_GridView.SelectedIndex = _indx;
        UpdateWorkload();
        FillupGridViewProjectSummary();
      }
      catch ( ApplicationError _ae )
      {
        return GenericStateMachineEngine.ActionResult.Exception( _ae, "GenericStateMachineEngine.ActionResult" );
      }
      catch ( Exception _ex )
      {
        return GenericStateMachineEngine.ActionResult.Exception( _ex, "GenericStateMachineEngine.ActionResult" );
      }
      return GenericStateMachineEngine.ActionResult.Success;
    }
    private void ClearUserInterface()
    {
      m_GridView.SelectedIndex = -1;
      m_WorkloadDescriptionTextBox.Text = String.Empty;
      Workload _lastWorkload = ( from _wlx in Me.Workload where _wlx.WorkloadDate.GetValueOrDefault().Date == DateTime.Today.Date orderby _wlx.WorkloadDate descending select _wlx ).FirstOrDefault();
      DateTime _lastActivityTime = DateTime.Today.Date + TimeSpan.FromHours( 8 );
      if ( _lastWorkload != null && _lastActivityTime < _lastWorkload.WorkloadDate )
        _lastActivityTime = _lastWorkload.WorkloadDate.Value;
      m_WorkloadHoursTextBox.Text = ( ( DateTime.Now - _lastActivityTime ).TotalMinutes / 60.0 ).ToString();
    }
    #endregion

    #region helpers
    private double TextBoxToHours( TextBox _textBox, string _label )
    {
      try
      {
        return Convert.ToDouble( _textBox.Text );
      }
      catch ( Exception )
      {
        throw new ApplicationException( String.Format( "{0} texbox has wrong number", _label ) );
      }
    }
    private bool ShouwUserInformation()
    {
      if ( Me == null )
      {
        this.Controls.Add( new Literal() { Text = String.Format( CAS.SharePoint.Web.CommonDefinitions.ErrorMessageFormat, "User not recognized - you must be added to the Recourses" ) } );
        return false;
      }
      else
      {
        this.Controls.Add( new Literal() { Text = String.Format( CAS.SharePoint.Web.CommonDefinitions.ErrorMessageFormat, "Welcome: " + Me.EmployeeADAccountTitle ) } );
        return true;
      }
    }
    private void FillupWorkflowGridView()
    {
      if ( Me == null )
        m_GridView.DataSource = null;
      else
      {
        At = "m_GridView.DataSource";
        m_GridView.DataSource = from _wlidx in Me.Workload
                                where _wlidx.WorkloadDate.Value.Date == m_Calendar.SelectedDate.Date
                                select new
                                {
                                  Hours = _wlidx.Hours.GetValueOrDefault( 0 ),
                                  Project = _wlidx.Workload2ProjectTitle == null ? m_SelectProjectDropDownEntry : _wlidx.Workload2ProjectTitle.Title,
                                  Task = _wlidx.Workload2TaskTitle == null ? m_SelectTaskDropDownEntry : _wlidx.Workload2TaskTitle.Title,
                                  Description = _wlidx.Title,
                                  ID = _wlidx.Identyfikator
                                };
      }
      m_GridView.DataBind();
      ClearUserInterface();
    }
    private void FillupTaskaDropDown()
    {
      m_TaskDropDown.Items.Clear();
      if ( SelectedMilestone == null )
        m_TaskDropDown.Items.Add( new ListItem( m_SelectMilestoneDropDownEntry, String.Empty ) { Selected = true } );
      else
      {
        IQueryable<Tasks> _tasks = from _tidx in SelectedMilestone.Tasks0
                                   let _stts = _tidx.Task2StatusTitle
                                   where ( _stts == null ? true : _stts.Active.GetValueOrDefault( true ) ) &&
                                         ( _tidx.Task2ResourcesTitle == null || _tidx.Task2ResourcesTitle == Me )
                                   orderby _tidx.Title ascending
                                   select _tidx;
        m_TaskDropDown.EntityListDataSource( _tasks );
      }
    }
    private void FillupMilestoneDropDown()
    {
      m_MilestoneDropDown.Items.Clear();
      if ( SelectedProject == null )
        m_MilestoneDropDown.Items.Add( new ListItem( m_SelectProjectDropDownEntry, String.Empty ) { Selected = true } );
      else
      {
        IQueryable<Milestone> _mstns = from _tidx in SelectedProject.Milestone
                                       where _tidx.Active.GetValueOrDefault( false )
                                       orderby _tidx.SortOrder ascending
                                       select _tidx;
        m_MilestoneDropDown.EntityListDataSource<Milestone>( _mstns );
        Milestone _default = ( from _amx in _mstns
                               where _amx.Default.GetValueOrDefault( false )
                               select _amx ).FirstOrDefault();
        m_MilestoneDropDown.SelectItem4Element( _default );
      }
    }
    private void FillupProjectDropDown()
    {
      m_ProjectDropDown.Items.Clear();
      m_ProjectDropDown.Items.Add( new ListItem( m_SelectProjectDropDownEntry, String.Empty ) { Selected = true } );
      if ( Me == null || MyProjects == null )
        return;
      foreach ( var _row2 in MyProjects )
        m_ProjectDropDown.Items.Add( new ListItem( _row2.Title, _row2.Identyfikator.ToString() ) );
    }
    private void FillupGridViewProjectSummary()
    {
      m_GridViewProjectSummary.DataSource = WorkloadSummary.WorkloadSummaryList( MyProjects, Me );
      m_GridViewProjectSummary.DataBind();
    }
    private void UpdateWorkload()
    {
      if ( m_GridView.SelectedIndex < 0 )
        return;
      string _selection = m_GridView.SelectedDataKey.Value.ToString();
      Workload _workload = Element.GetAtIndex<Workload>( m_DataContext.DataContext.Workload, _selection );
      m_WorkloadDescriptionTextBox.Text = _workload.Title;
      m_WorkloadHoursTextBox.Text = _workload.Hours.GetValueOrDefault( 0 ).ToString();
      m_ProjectDropDown.SelectItem4Element( _workload.Workload2ProjectTitle );
      FillupMilestoneDropDown();
      m_MilestoneDropDown.SelectItem4Element( _workload.Workload2TaskTitle != null ? _workload.Workload2TaskTitle.Task2MilestoneResolvedInTitle : null );
      FillupTaskaDropDown();
      m_TaskDropDown.SelectItem4Element( _workload.Workload2TaskTitle );
    }
    #endregion

    #region event handlers
    private void m_ProjectDropDown_SelectedIndexChanged( object sender, EventArgs e )
    {
      try
      {
        if ( SelectedProject == null || Me == null )
          return;
        FillupMilestoneDropDown();
        FillupTaskaDropDown();
      }
      catch ( Exception _ex )
      {
        ShowActionResult( GenericStateMachineEngine.ActionResult.Exception( _ex, "m_ProjectDropDown_SelectedIndexChanged" ) );
      }
    }

    private void m_MilestoneDropDown_SelectedIndexChanged( object sender, EventArgs e )
    {
      try
      {
        if ( SelectedProject == null || Me == null )
          return;
        FillupTaskaDropDown();
      }
      catch ( Exception _ex )
      {
        ShowActionResult( GenericStateMachineEngine.ActionResult.Exception( _ex, "m_MilestoneDropDown_SelectedIndexChanged" ) );
      }
    }
    protected void m_GridView_SelectedIndexChanged( object sender, EventArgs e )
    {
      try
      {
        UpdateWorkload();
      }
      catch ( Exception _ex )
      {
        ShowActionResult( GenericStateMachineEngine.ActionResult.Exception( _ex, "m_GridView_SelectedIndexChanged" ) );
      }
    }
    /// <summary>
    /// Calendar Selection Changed method
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void m_Calendar_SelectionChanged( object sender, EventArgs e )
    {
      try
      {
        m_WorkloadHoursTextBox.Text = String.Empty;
        m_WorkloadDescriptionTextBox.Text = String.Empty;
        if ( Me == null )
          return;
        double _hoursADay = ( from _widx in Me.Workload
                              where _widx.WorkloadDate.Value.Date == m_Calendar.SelectedDate.Date
                              select _widx ).Sum( _x => _x.Hours.GetValueOrDefault( 0 ) );
        string _rprtTemplate = "You have reported {0} working hours for the selected day {1:D}";
        m_HoursADayLabel.Text = String.Format( _rprtTemplate, _hoursADay, m_Calendar.SelectedDate.Date );
        FillupWorkflowGridView();
      }
      catch ( Exception _ex )
      {
        ShowActionResult( GenericStateMachineEngine.ActionResult.Exception( _ex, "m_Calendar_SelectionChanged" ) );
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
    private ControlState m_ControlState = new ControlState( null );
    private CAS.AgileWorkloadTracker.Linq.Resources p_me = null;
    private CAS.AgileWorkloadTracker.Linq.Resources Me
    {
      get
      {
        try
        {
          if ( p_me == null )
            p_me = Resources.FindForUser( m_DataContext.DataContext, SPContext.Current.Web.CurrentUser );
          return p_me;

        }
        catch ( Exception xe )
        {
          throw new ApplicationError( "Me", "", xe.Message, xe );
        }
      }
    }
    private Milestone p_Milestone = null;
    private Milestone SelectedMilestone
    {
      get
      {
        if ( p_Milestone == null && m_ProjectDropDown.SelectedIndex > 0 )
          p_Milestone = Element.GetAtIndex<Milestone>( m_DataContext.DataContext.Milestone, m_MilestoneDropDown.SelectedValue );
        return p_Milestone;
      }
    }
    private Projects p_Projects = null;
    private Projects SelectedProject
    {
      get
      {
        if ( p_Projects == null && m_ProjectDropDown.SelectedIndex > 0 )
          p_Projects = Element.GetAtIndex<Projects>( m_DataContext.DataContext.Projects, m_ProjectDropDown.SelectedValue );
        return p_Projects;
      }
    }
    private IQueryable<Projects> p_MyProjects = null;
    private IQueryable<Projects> MyProjects
    {
      get
      {
        if ( p_MyProjects == null )
          p_MyProjects = from _association in Me.Estimation let _pidx = _association.Estimation2ProjectTitle where _pidx != null && _pidx.Active.GetValueOrDefault(true) select _pidx;
        return p_MyProjects;
      }
    }
    #endregion

    #region errors handling
    internal void ShowActionResult( GenericStateMachineEngine.ActionResult _rslt )
    {
      if ( _rslt.LastActionResult == GenericStateMachineEngine.ActionResult.Result.Success )
        return;
      if ( _rslt.LastActionResult == GenericStateMachineEngine.ActionResult.Result.Exception )
      {
#if DEBUG
        string _format = CommonDefinitions.Convert2ErrorMessageFormat( "Exception at: {0}/{1} of : {2}." );
        this.Controls.Add( new Literal() { Text = String.Format( _format, _rslt.ActionException.Source, At, _rslt.ActionException.Message ) } );
#endif
        Anons.WriteEntry( _rslt.ActionException.Source, _rslt.ActionException.Message );
      }
      else
      {
        string _format = CommonDefinitions.Convert2ErrorMessageFormat( "Validation error at: {0}/{1} of : {2}." );
        this.Controls.Add( new Literal() { Text = String.Format( _format, At, _rslt.ActionException.Message ) } );
      }
    }
    #endregion

  }
}
