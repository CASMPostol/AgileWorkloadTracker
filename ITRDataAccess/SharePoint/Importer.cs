using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using CAS.AgileWorkloadTracker.Linq;
using Microsoft.SharePoint.Linq;
using System.Globalization;
using TaskType = CAS.AgileWorkloadTracker.Linq.Type;

namespace CAS.ITRDataAccess.SharePoint
{
  public partial class Importer: Component
  {
    #region ctor
    public Importer()
    {
      InitializeComponent();
    }
    public Importer( IContainer container )
      : this()
    {
      container.Add( this );
    }
    #endregion

    #region public
    internal void ImportData()
    {
      Bugnet.DatabaseContentDataSet m_BugNETDataSet = m_importFromBugNet.GetDataFromDatabase();
      Import( m_BugNETDataSet );
      TimeTracking.TimeTrackingDataSet m_timeTrackingDataSet = m_importFromTimeTracking.GetDataFromDatabase();
      Import( m_timeTrackingDataSet );
    }
    internal void Import( Bugnet.DatabaseContentDataSet m_BugNETDataSet )
    {
      Import( m_BugNETDataSet.Project, p_DefaultStage, p_Entities );
      p_Entities.SubmitChanges();
      Console.WriteLine( "Project" );
      Import( m_BugNETDataSet.Version, p_Entities, p_DefaultStage );
      p_Entities.SubmitChanges();
      Console.WriteLine( "Version" );
      Import( m_BugNETDataSet.Status, p_Entities );
      p_Entities.SubmitChanges();
      Console.WriteLine( "Status" );
      Import( m_BugNETDataSet.Priority, p_Entities );
      p_Entities.SubmitChanges();
      Console.WriteLine( "Priority" );
      Import( m_BugNETDataSet.Resolution, p_Entities );
      p_Entities.SubmitChanges();
      Console.WriteLine( "Resolution" );
      Import( m_BugNETDataSet.Type, p_Entities );
      p_Entities.SubmitChanges();
      Console.WriteLine( "Type" );
      Import( m_BugNETDataSet.Bug, p_Entities );
      p_Entities.SubmitChanges();
      Console.WriteLine( "Bug" );
      Import( m_BugNETDataSet.BugComment, p_Entities );
    }
    internal void Import( TimeTracking.TimeTrackingDataSet m_timeTrackingDataSet )
    {
      throw new NotImplementedException();
      //Import( m_timeTrackingDataSet.RODZAJPRACY, m_Entities );
      //Import( m_timeTrackingDataSet.STATUSY, m_Entities ); alwazs Closed
      //Import( m_timeTrackingDataSet.KATEGORIE, m_Entities ); using onlz PODKATEGORIE
      Import( m_timeTrackingDataSet.PRACOWNICY, p_Entities );
      Import( m_timeTrackingDataSet.KONTRAHENCI, p_Entities );
      Import( m_timeTrackingDataSet.UMOWY, p_Entities );
      //Import( m_timeTrackingDataSet.PLATNOSCI, m_Entities );
      //Import( m_timeTrackingDataSet.POLISY, m_Entities );
      Import( m_timeTrackingDataSet.PROJEKTY, p_Entities );
      Import( m_timeTrackingDataSet.GODZINY, p_Entities );
      Import( m_timeTrackingDataSet.PLAN, p_Entities );
    }
    #endregion

    #region Bugnet import
    private void Import( Bugnet.DatabaseContentDataSet.ProjectDataTable projectDataTable, Stage stage, Entities _entt )
    {
      foreach ( var _row in projectDataTable )
      {
        Projects _new = Create<Projects>( _entt.Projects, m_ProjectsDictionary, _row.Name, _row.ProjectID );
        _new.Active = _row.Active > 0;
        _new.Body = _row.IsDescriptionNull() ? String.Empty : _row.Description;
        _new.Currency = Currency.PLN;
        _new.Project2ContractTitle = null;
        _new.Project2ResourcesTitle = null;
        _new.Project2StageTitle = stage;
        _new.ProjectBudget = 0;
        _new.ProjectHours = 0;
        _new.ProjectNumber = _row.Code;
        _new.ProjectStartDate = _row.CreateDate;
        _new.ProjectType = ProjectType.ProjectInternal;
        _new.ProjectWarrantyDate = _row.CreateDate + TimeSpan.FromDays( 364 );
      }
    }
    private void Import( Bugnet.DatabaseContentDataSet.VersionDataTable versionDataTable, Entities _entt, Stage projectStage )
    {
      foreach ( var _row in versionDataTable )
      {
        try
        {
          Milestone _new = Create<Milestone>( _entt.Milestone, m_MilestoneDictionary, _row.Name, _row.VersionID );
          _new.Milestone2ProjectTitle = GetOrAdd<Projects>( _entt.Projects, m_ProjectsDictionary, _row.ProjectID );
          _new.Order = _row.SortOrder;
          _new.Milestone2StageTitle = projectStage;
          _new.Active = true;
          _new.MilestoneHours = 0;
          _entt.SubmitChanges();
        }
        catch ( Exception _ex )
        {
          Console.WriteLine( String.Format( "Error importing Version of Name: {0}, because of {1}", _row.Name, _ex.Message ) );
        }
      }
    }
    private void Import( Bugnet.DatabaseContentDataSet.ResolutionDataTable resutionDataTable, Entities _entt )
    {
      foreach ( var _row in resutionDataTable )
      {
        Resolution _new = Create<Resolution>( _entt.Resolution, m_ResolutionDictionary, _row.Name, _row.ResolutionID );
      }
    }
    private void Import( Bugnet.DatabaseContentDataSet.StatusDataTable statusDataTable, Entities _entt )
    {
      foreach ( var _row in statusDataTable )
      {
        Status _new = Create<Status>( _entt.Status, m_StatusDictionary, _row.Name, _row.StatusID );
      }
    }
    private void Import( Bugnet.DatabaseContentDataSet.TypeDataTable typeDataTable, Entities _entt )
    {
      foreach ( var _row in typeDataTable )
      {
        TaskType _new = Create<TaskType>( _entt.Type, m_TaskTypeDictionary, _row.Name, _row.TypeID );
      }
    }
    private void Import( Bugnet.DatabaseContentDataSet.PriorityDataTable priorityDataTable, Entities _entt )
    {
      foreach ( var _row in priorityDataTable )
      {
        Priority _new = Create<Priority>( _entt.Priority, m_PriorityDictionary, _row.Name, _row.PriorityID );
      }
    }
    private void Import( Bugnet.DatabaseContentDataSet.BugDataTable bugDataTable, Entities _entt )
    {
      foreach ( var item in bugDataTable )
      {
        Tasks _newTasks = Create<Tasks>( _entt.Task, m_TasksDictionary, item.Description, item.BugID );
        _newTasks.Task2MilestoneDefinedInTitle = GetOrAdd<Milestone>( _entt.Milestone, m_MilestoneDictionary, item.VersionID );
        _newTasks.Task2MilestoneResolvedInTitle = GetOrAdd<Milestone>( _entt.Milestone, m_MilestoneDictionary, item.FixedInVersionId );
        _newTasks.Task2ProjectTitle = GetOrAdd<Projects>( _entt.Projects, m_ProjectsDictionary, item.ProjectID );
        if ( !item.IsAssignedToUserIdNull() )
          _newTasks.Task2ResourcesTitle = GetOrAdd<Resources>( _entt.Resources, m_ResourcesDictionary, item.AssignedToUserId );
        else
          _newTasks.Task2ResourcesTitle = null;
        _newTasks.Task2SPriorityTitle = GetOrAdd<Priority>( _entt.Priority, m_PriorityDictionary, item.PriorityID );
        _newTasks.Task2SResolutionTitle = GetOrAdd<Resolution>( _entt.Resolution, m_ResolutionDictionary, item.ResolutionID );
        _newTasks.Task2StatusTitle = GetOrAdd<Status>( _entt.Status, m_StatusDictionary, item.StatusID );
        _newTasks.Task2TypeTitle = GetOrAdd<TaskType>( _entt.Type, m_TaskTypeDictionary, item.TypeID );
      }
    }
    private void Import( Bugnet.DatabaseContentDataSet.BugCommentDataTable bugCommentDataTable, Entities _entt )
    {
      int _bugId = 0;
      int _iteration = 0;
      try
      {
        foreach ( var _row in bugCommentDataTable )
        {
          _bugId = _row.BugCommentID;
          TaskComments _new = new TaskComments()
          {
            Body = _row.Comment, //SPValidSubstring(),
            TaskComments2TaskTitle = GetOrAdd<Tasks>( _entt.Task, m_TasksDictionary, _row.BugID )
          };
          _entt.TaskComments.InsertOnSubmit( _new );
          Console.Write( "\r" );
          Console.Write( _iteration++ );
          //m_TaskCommentsDictionary.Add( _row.BugCommentID, _new );
          _entt.SubmitChanges();
        }
      }
      catch ( Exception _ex )
      {
        Console.WriteLine( String.Format( "Error importing BugCommentDataTable of Name: {0}, because of {1}", _bugId, _ex.Message ) );
      }
    }
    #endregion

    #region TimeTracking import
    private void Import( TimeTracking.TimeTrackingDataSet.PLANDataTable pLANDataTable, Entities m_Entities )
    {
      foreach ( var _row in pLANDataTable )
      {
        Estimation _new = Create<Estimation>( m_Entities.Estimation, m_EstimationDictionary, _row.OPIS, _row.ID );
        _new.EstimatedWorkload = _row.GODZINY;
        _new.Estimation2ProjectTitle = m_ProjectsDictionary[ _row.ID_PROJEKTU ];
        _new.Estimation2ResourcesTitle = GetResourcesFromTimeTrackerId( _row.ID_PRACOWNIKA );
      }
    }
    private void Import( TimeTracking.TimeTrackingDataSet.GODZINYDataTable gODZINYDataTable, Entities m_Entities )
    {
      foreach ( var _row in gODZINYDataTable )
      {
        Workload _new = Create<Workload>( m_Entities.Workload, m_WorkloadDictionary, _row.OPIS, _row.ID );
        _new.Hours = _row.IsLICZBA_GODZINNull() ? 0 : _row.LICZBA_GODZIN;
        _new.Workload2ProjectTitle = GetProjectFromTimeTrackerId( _row.IsID_PROJEKTUNull() ? -1 : _row.ID_PROJEKTU );
        _new.Workload2ResourcesTitle = GetResourcesFromTimeTrackerId( _row.IsID_PRACOWNIKANull() ? -1 : _row.ID_PRACOWNIKA );
        _new.Workload2StageTitle = _new.Workload2ProjectTitle != null ? _new.Workload2ProjectTitle.Project2StageTitle : null;
        _new.Workload2TaskTitle = CreateTask( _row.RODZAJPRACYRow, _new.Workload2ProjectTitle, _new.Workload2ResourcesTitle );
        _new.WorkloadDate = _row.IsDATANull() ? new Nullable<DateTime>() : _row.DATA;
      }
    }
    private Tasks CreateTask( TimeTracking.TimeTrackingDataSet.RODZAJPRACYRow rODZAJPRACYRow, Projects projects, Resources resources )
    {
      Tasks _newTask = new Tasks()
      {
        Task2MilestoneDefinedInTitle = null, // [AWT-3502] Add lookup from Milestones to Project http://itrserver/Bugs/BugDetail.aspx?bid=3502
        Task2MilestoneResolvedInTitle = null, // see above
        Task2ProjectTitle = projects,
        Task2ResourcesTitle = resources,
        Task2SPriorityTitle = null,
        Task2SResolutionTitle = null,
        Task2StatusTitle = null,
        Task2TypeTitle = null,
        Title = rODZAJPRACYRow.NAZWAPRACY
      };
      TaskComments _newTaskComment = new TaskComments()
      {
        Body = rODZAJPRACYRow.OPIS.SPValidSubstring(),
        TaskComments2TaskTitle = _newTask
      };
      return _newTask;
    }
    Dictionary<int, int> m_ProjectsMapppingDictionary = new Dictionary<int, int>() { };
    private Projects GetProjectFromTimeTrackerId( int projectId )
    {
      if ( !m_PriorityDictionary.ContainsKey( projectId ) )
        return m_ProjectsDictionary[ -projectId ];
      return m_ProjectsDictionary[ m_ProjectsMapppingDictionary[ projectId ] ];
    }
    private void Import( TimeTracking.TimeTrackingDataSet.PROJEKTYDataTable pROJEKTYDataTable, Entities m_Entities )
    {
      foreach ( var _row in pROJEKTYDataTable )
      {
        if ( !m_PriorityDictionary.ContainsKey( _row.ID ) )
          continue;
        Projects _newProject = Create<Projects>( m_Entities.Projects, m_ProjectsDictionary, _row.NAZWA_KROTKA, -_row.ID );
        _newProject.Active = false;
        _newProject.Body = _row.IsNAZWANull() ? "N/A" : _row.NAZWA.SPValidSubstring();
        _newProject.Currency = Currency.PLN;
        _newProject.Project2ContractTitle = GetOrAdd<Contracts>( m_Entities.Contracts, m_ContractDictionary, -_row.ID_UMOWY );
        _newProject.Project2ResourcesTitle = GetResourcesFromTimeTrackerId( _row.IsID_MANAGERANull() ? -1 : _row.ID_MANAGERA );
        _newProject.Project2StageTitle = p_DefaultStage;
        _newProject.ProjectBudget = _row.IsBUDZETNull() ? 0 : Convert.ToDouble( _row.BUDZET );
        _newProject.ProjectEndDate = _row.IsDATA_KONIECNull() ? DateTime.Today : _row.DATA_KONIEC;
        _newProject.ProjectHours = _row.IsLICZBA_GODZINNull() ? 0 : _row.LICZBA_GODZIN;
        _newProject.ProjectNumber = _row.IsNUMERNull() ? "N/A" : _row.NUMER;
        _newProject.ProjectStartDate = _row.IsDATA_STARTNull() ? DateTime.Today : _row.DATA_START;
        _newProject.ProjectType = _row.PODKATEGORIERow == null ? ProjectType.None : this.m_ProjectTypeMapping[ _row.PODKATEGORIERow.ID ];
        _newProject.ProjectWarrantyDate = _row.IsDATA_GWARANCJANull() ? DateTime.Today : _row.DATA_GWARANCJA;
      }
    }
    private void Import( TimeTracking.TimeTrackingDataSet.UMOWYDataTable uMOWYDataTable, Entities m_Entities )
    {
      foreach ( var _row in uMOWYDataTable )
      {
        Contracts _newContract = Create<Contracts>( m_Entities.Contracts, m_ContractDictionary, _row.NAZWA_KROTKA, -_row.ID );
        _newContract.Body = _row.IsPRZEDMIOTNull() ? "N/A" : _row.PRZEDMIOT.SPValidSubstring();
        _newContract.Body = _row.IsPRZEDMIOTNull() ? "N/A/" : _row.PRZEDMIOT.SPValidSubstring();
        _newContract.ContractDate = _row.IsDATA_UMOWYNull() ? DateTime.Today : _row.DATA_UMOWY;
        _newContract.ContractEndDate = _row.IsKONIECNull() ? DateTime.Today : _row.KONIEC;
        _newContract.ContractNumber = _row.IsNUMERNull() ? "N/A" : _row.NUMER;
        _newContract.ContractOffer = _row.IsOFERTANull() ? "N/A" : _row.OFERTA;
        _newContract.Contracts2PartnersTitle = _row.IsID_KLIENTANull() ? null : GetPartnerFromTimeTrackerId( _row.ID_KLIENTA );
        _newContract.ContractSubject = _row.IsNAZWA_KROTKANull() ? "N/A" : _row.NAZWA_KROTKA;
        _newContract.ContractValue = _row.IsKONTRAKTNull() ? 0 : Convert.ToDouble( _row.KONTRAKT );
        _newContract.ContractWarrantyDate = _row.IsGWARANCJANull() ? DateTime.Today : _row.GWARANCJA;
        _newContract.Currency = _row.IsWALUTANull() ? Currency.Invalid : GetCurrency( _row.WALUTA );
      }
    }

    private Currency GetCurrency( string p )
    {
      throw new NotImplementedException();
    }


    private Partners GetPartnerFromTimeTrackerId( int p )
    {
      throw new NotImplementedException();
    }

    private void Import( TimeTracking.TimeTrackingDataSet.KONTRAHENCIDataTable kONTRAHENCIDataTable, Entities m_Entities )
    {
      foreach ( var _row in kONTRAHENCIDataTable )
      {
      }
    }

    private void Import( TimeTracking.TimeTrackingDataSet.PRACOWNICYDataTable pRACOWNICYDataTable, Entities m_Entities )
    {
      foreach ( var _row in pRACOWNICYDataTable )
      {
      }
    }

    #region mapping
    private Dictionary<int, ProjectType> m_ProjectTypeMapping = new Dictionary<int, ProjectType>() 
    { 
      { 1, ProjectType.ProjectCommercial },
      { 2, ProjectType.ProjectInternal},
      { 3, ProjectType.Marketing},
      { 5, ProjectType.Office},
      { 8, ProjectType.AfterSalesServices},
      { 9, ProjectType.ProjectCommercial},
      {10, ProjectType.ProjectConception}
    };
    private Resources GetResourcesFromTimeTrackerId( int p )
    {
      throw new NotImplementedException();
    }
    #endregion

    #endregion

    #region data management
    private type Create<type>( EntityList<type> list, Dictionary<int, type> _dictionary, string title, int _key )
      where type: Element, new()
    {
      type _elmnt = new type();
      if ( !String.IsNullOrEmpty( title ) )
        _elmnt.Title = title.SPValidSubstring();
      if ( _dictionary.Keys.Contains( _key ) )
        _key = UniqueKey();
      _dictionary.Add( _key, _elmnt );
      list.InsertOnSubmit( _elmnt );
      return _elmnt;
    }
    private type Create<type>( EntityList<type> list, Dictionary<Guid, type> _dictionary, string title, Guid _key )
      where type: Element, new()
    {
      type _elmnt = new type();
      if ( !String.IsNullOrEmpty( title ) )
        _elmnt.Title = title;
      if ( _dictionary.Keys.Contains( _key ) )
        _key = Guid.NewGuid();
      _dictionary.Add( _key, _elmnt );
      list.InsertOnSubmit( _elmnt );
      return _elmnt;
    }
    private type GetOrAdd<type>( EntityList<type> _EDC, Dictionary<int, type> _dictionary, int? _key )
      where type: Element, new()
    {
      if ( !_key.HasValue )
        _key = UniqueKey();
      else if ( _dictionary.ContainsKey( _key.Value ) )
        return _dictionary[ _key.Value ];
      return Create<type>( _EDC, _dictionary, EmptyKey( _key.Value ), _key.Value );
    }
    private type GetOrAdd<type>( EntityList<type> _EDC, Dictionary<Guid, type> _dictionary, Guid? _key )
      where type: Element, new()
    {
      if ( !_key.HasValue )
        _key = Guid.NewGuid();
      else if ( _dictionary.ContainsKey( _key.Value ) )
        return _dictionary[ _key.Value ];
      return Create<type>( _EDC, _dictionary, EmptyKey( _key.Value ), _key.Value );
    }
    private short m_EmptyKeyIdx = 0;
    private int UniqueKey() { return m_EmptyKeyIdx--; }
    private static string EmptyKey( int key ) { return String.Format( CultureInfo.InvariantCulture, "EmptyKey{0}", key ); }
    private static string EmptyKey( Guid key ) { return String.Format( CultureInfo.InvariantCulture, "EmptyKey{0}", key ); }
    #endregion

    #region Dictionaries
    private Dictionary<int, Tasks> m_TasksDictionary = new Dictionary<int, Tasks>();
    private Dictionary<int, Projects> m_ProjectsDictionary = new Dictionary<int, Projects>();
    private Dictionary<int, Milestone> m_MilestoneDictionary = new Dictionary<int, Milestone>();
    private Dictionary<int, Resolution> m_ResolutionDictionary = new Dictionary<int, Resolution>();
    private Dictionary<int, TaskType> m_TaskTypeDictionary = new Dictionary<int, TaskType>();
    private Dictionary<int, Status> m_StatusDictionary = new Dictionary<int, Status>();
    private Dictionary<int, Priority> m_PriorityDictionary = new Dictionary<int, Priority>();
    private Dictionary<int, TaskComments> m_TaskCommentsDictionary = new Dictionary<int, TaskComments>();
    private Dictionary<Guid, Resources> m_ResourcesDictionary = new Dictionary<Guid, Resources>();
    private Dictionary<int, Estimation> m_EstimationDictionary = new Dictionary<int, Estimation>();
    private Dictionary<int, Workload> m_WorkloadDictionary = new Dictionary<int, Workload>();
    private Dictionary<int, Contracts> m_ContractDictionary = new Dictionary<int, Contracts>();
    #endregion

    #region private
    private const string DefaultStageTitle = "Imported";
    private Stage p_DefaultStage = null;
    private Stage DefaultStage
    {
      get
      {
        if ( p_DefaultStage == null )
          p_DefaultStage = ( from _sidx in p_Entities.Stage where _sidx.Title.Contains( DefaultStageTitle ) select _sidx ).FirstOrDefault();
        if ( p_DefaultStage == null )
        {
          p_DefaultStage = new Stage() { Title = DefaultStageTitle };
          p_Entities.Stage.InsertOnSubmit( p_DefaultStage );
        }
        return p_DefaultStage;
      }
    }
    private Entities p_Entities = null;
    private Entities m_Entities
    {
      get
      {
        if ( p_Entities == null )
          p_Entities = Entities.GetEntitity();
        return p_Entities;
      }
    }
    #endregion

    #region dispose
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
      if ( p_Entities != null )
      {
        p_Entities.Dispose();
        p_Entities = null;
      }
      base.Dispose( disposing );
    }
    #endregion

  }
}
