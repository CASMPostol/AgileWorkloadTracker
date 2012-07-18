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
  /// <summary>
  /// Importer
  /// </summary>
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
      TimeTracking.TimeTrackingDataSet m_timeTrackingDataSet = m_importFromTimeTracking.GetDataFromDatabase();
      Import( m_timeTrackingDataSet.PRACOWNICY, m_Entities );
      Import( m_BugNETDataSet.aspnet_Users, m_Entities );
      throw new NotImplementedException();
      Import( m_BugNETDataSet );
      Import( m_timeTrackingDataSet );
    }
    private void Import( Bugnet.DatabaseContentDataSet m_BugNETDataSet )
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
    private void Import( TimeTracking.TimeTrackingDataSet m_timeTrackingDataSet )
    {
      //Import( m_timeTrackingDataSet.RODZAJPRACY, m_Entities );
      //Import( m_timeTrackingDataSet.STATUSY, m_Entities ); alwazs Closed
      //Import( m_timeTrackingDataSet.KATEGORIE, m_Entities ); using onlz PODKATEGORIE
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
    private void Import( Bugnet.DatabaseContentDataSet.aspnet_UsersDataTable usersDataTable, Entities m_Entities )
    {
      foreach ( var _row in usersDataTable )
      {
        if ( m_ResourcesDictionaryMapping.ContainsKey( _row.LoweredUserName ) )
        {
          Resources _new = m_ResourcesDictionaryTimeTracking[ m_ResourcesDictionaryMapping[ _row.LoweredUserName ] ];
          m_ResourcesDictionaryBugNet.Add( _row.UserId, _new );
        }
        else
          Create<Resources>( m_Entities.Resources, m_ResourcesDictionaryBugNet, _row.UserName, _row.UserId );
      }
      m_Entities.SubmitChanges();
    }
    private void Import( Bugnet.DatabaseContentDataSet.ProjectDataTable projectDataTable, Stage stage, Entities _entt )
    {
      foreach ( var _row in projectDataTable )
      {
        Projects _new = Create<Projects>( _entt.Projects, m_ProjectsDictionaryBugNet, _row.Name, _row.ProjectID );
        _new.Active = _row.Active > 0;
        _new.Body = _row.IsDescriptionNull() ? String.Empty : _row.Description;
        _new.Currency = Currency.PLN;
        _new.Project2ContractTitle = null;
        _new.Project2ResourcesTitle = GetOrAdd<Resources>( _entt.Resources, m_ResourcesDictionaryBugNet, _row.ManagerUserID );
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
          _new.Milestone2ProjectTitle = GetOrAdd<Projects>( _entt.Projects, m_ProjectsDictionaryBugNet, _row.ProjectID );
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
        Create<Resolution>( _entt.Resolution, m_ResolutionDictionary, _row.Name, _row.ResolutionID );
    }
    private void Import( Bugnet.DatabaseContentDataSet.StatusDataTable statusDataTable, Entities _entt )
    {
      foreach ( var _row in statusDataTable )
        Create<Status>( _entt.Status, m_StatusDictionary, _row.Name, _row.StatusID );
    }
    private void Import( Bugnet.DatabaseContentDataSet.TypeDataTable typeDataTable, Entities _entt )
    {
      foreach ( var _row in typeDataTable )
        Create<TaskType>( _entt.Type, m_TaskTypeDictionary, _row.Name, _row.TypeID );
    }
    private void Import( Bugnet.DatabaseContentDataSet.PriorityDataTable priorityDataTable, Entities _entt )
    {
      foreach ( var _row in priorityDataTable )
        Create<Priority>( _entt.Priority, m_PriorityDictionary, _row.Name, _row.PriorityID );
    }
    private void Import( Bugnet.DatabaseContentDataSet.BugDataTable bugDataTable, Entities _entt )
    {
      foreach ( var item in bugDataTable )
      {
        Tasks _newTasks = Create<Tasks>( _entt.Task, m_TasksDictionary, item.Summary.SPValidSubstring(), item.BugID );
        //TODO  [AWT-3519] Task title contains HTML tags. http://itrserver/Bugs/BugDetail.aspx?bid=3519
        //_newTasks.Description = item.Description;
        _newTasks.Task2MilestoneDefinedInTitle = GetOrAdd<Milestone>( _entt.Milestone, m_MilestoneDictionary, item.VersionID );
        _newTasks.Task2MilestoneResolvedInTitle = GetOrAdd<Milestone>( _entt.Milestone, m_MilestoneDictionary, item.FixedInVersionId );
        _newTasks.Task2ProjectTitle = GetOrAdd<Projects>( _entt.Projects, m_ProjectsDictionaryBugNet, item.ProjectID );
        if ( !item.IsAssignedToUserIdNull() )
          _newTasks.Task2ResourcesTitle = GetOrAdd<Resources>( _entt.Resources, m_ResourcesDictionaryBugNet, item.AssignedToUserId );
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
          _new.TaskComments2TaskTitle.Adjust( _row.CreatedDate );
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
        if ( _row.IsID_PRACOWNIKANull() || _row.IsID_PRACOWNIKANull() )
          return;
        Estimation _new = CheckAddEstimation(
          GetOrAdd<Resources>( m_Entities.Resources, m_ResourcesDictionaryTimeTracking, _row.ID_PRACOWNIKA ), GetOrAdd<Projects>( m_Entities.Projects, m_ProjectsDictionaryTimeTracking, _row.ID_PROJEKTU ) );
        _new.EstimatedWorkload = _row.GODZINY;
      }
    }
    private void Import( TimeTracking.TimeTrackingDataSet.GODZINYDataTable gODZINYDataTable, Entities m_Entities )
    {
      foreach ( var _row in gODZINYDataTable )
      {
        Workload _new = Create<Workload>( m_Entities.Workload, m_WorkloadDictionary, _row.OPIS, _row.ID );
        _new.Hours = _row.IsLICZBA_GODZINNull() ? 0 : _row.LICZBA_GODZIN;
        _new.Workload2ProjectTitle = GetOrAdd<Projects>( m_Entities.Projects, m_ProjectsDictionaryTimeTracking, _row.ID_PROJEKTU );
        if ( !_row.IsID_PRACOWNIKANull() )
          _new.Workload2ResourcesTitle = GetOrAdd<Resources>( m_Entities.Resources, m_ResourcesDictionaryTimeTracking, _row.ID_PRACOWNIKA );
        _new.Workload2StageTitle = _new.Workload2ProjectTitle != null ? _new.Workload2ProjectTitle.Project2StageTitle : null;
        _new.Workload2TaskTitle = CreateTask( _row.RODZAJPRACYRow, _new.Workload2ProjectTitle, _new.Workload2ResourcesTitle );
        _new.WorkloadDate = _row.IsDATANull() ? new Nullable<DateTime>() : _row.DATA;
      }
    }
    private void Import( TimeTracking.TimeTrackingDataSet.PROJEKTYDataTable pROJEKTYDataTable, Entities m_Entities )
    {
      foreach ( var _row in pROJEKTYDataTable )
      {
        Projects _newProject = null;
        if ( ProjectsMapping.MappingTable.ContainsKey( _row.ID ) )
        {
          _newProject = GetOrAdd<Projects>( m_Entities.Projects, m_ProjectsDictionaryBugNet, ProjectsMapping.MappingTable[ _row.ID ] );
          m_ProjectsDictionaryTimeTracking.Add( _row.ID, _newProject );
        }
        else
          _newProject = Create<Projects>( m_Entities.Projects, m_ProjectsDictionaryTimeTracking, _row.NAZWA_KROTKA, _row.ID );
        _newProject.Active = false;
        _newProject.Body += String.Format( "<\br> mapped with {0}", _row.IsNAZWANull() ? "N/A" : _row.NAZWA );
        _newProject.Currency = GetCurrency( "PLN" );
        if ( _newProject.Project2ContractTitle == null )
          _newProject.Project2ContractTitle = GetOrAdd<Contracts>( m_Entities.Contracts, m_ContractDictionary, -_row.ID_UMOWY );
        if ( _newProject.Project2ResourcesTitle == null && !_row.IsID_MANAGERANull() )
          _newProject.Project2ResourcesTitle = GetOrAdd<Resources>( m_Entities.Resources, m_ResourcesDictionaryTimeTracking, _row.ID_MANAGERA );
        if ( _newProject.Project2StageTitle == null )
          _newProject.Project2StageTitle = p_DefaultStage;
        _newProject.ProjectBudget += _row.IsBUDZETNull() ? 0 : Convert.ToDouble( _row.BUDZET );
        if ( !_row.IsDATA_KONIECNull() )
          _newProject.Adjust( _row.DATA_KONIEC );
        _newProject.ProjectHours += _row.IsLICZBA_GODZINNull() ? 0 : _row.LICZBA_GODZIN;
        _newProject.ProjectNumber += String.Format( " mapped with {0}", _row.IsNUMERNull() ? "N/A" : _row.NUMER );
        if ( !_row.IsDATA_STARTNull() )
          _newProject.Adjust( _row.DATA_START );
        if ( !_newProject.ProjectType.HasValue )
          _newProject.ProjectType = _row.PODKATEGORIERow == null ? ProjectType.None : ProjectsMapping.m_ProjectTypeMapping[ _row.PODKATEGORIERow.ID ];
        if ( !_newProject.ProjectWarrantyDate.HasValue )
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
        _newContract.Contracts2PartnersTitle = _row.IsID_INWESTORANull() ? null : GetOrAdd<Partners>( m_Entities.Partners, m_PartnersDictionary, _row.ID_INWESTORA );
        _newContract.ContractSubject = _row.IsNAZWA_KROTKANull() ? "N/A" : _row.NAZWA_KROTKA;
        _newContract.ContractValue = _row.IsKONTRAKTNull() ? 0 : Convert.ToDouble( _row.KONTRAKT );
        _newContract.ContractWarrantyDate = _row.IsGWARANCJANull() ? DateTime.Today : _row.GWARANCJA;
        _newContract.Currency = _row.IsWALUTANull() ? Currency.Invalid : GetCurrency( _row.WALUTA );
      }
    }
    private void Import( TimeTracking.TimeTrackingDataSet.KONTRAHENCIDataTable kONTRAHENCIDataTable, Entities m_Entities )
    {
      foreach ( var _row in kONTRAHENCIDataTable )
      {
        Partners _new = Create<Partners>( m_Entities.Partners, m_PartnersDictionary, _row.NAZWA_KROTKA, _row.ID );
        _new.Body = _row.NAZWA;
        _new.CellPhone = "N/A";
        _new.EMail = _row.IsEMAILNull() ? "N/A" : _row.EMAIL;
        _new.WorkAddress = _row.ULICA_NUMER;
        _new.WorkCity = _row.MIASTO;
        _new.WorkCountry = _row.KRAJ;
        _new.WorkFax = _row.FAX;
        _new.WorkPhone = _row.TELEFON;
        _new.WorkZip = _row.KOD;
      }
    }
    private void Import( TimeTracking.TimeTrackingDataSet.PRACOWNICYDataTable pRACOWNICYDataTable, Entities m_Entities )
    {
      foreach ( var _row in pRACOWNICYDataTable )
      {
        Resources _new = Create<Resources>( m_Entities.Resources, m_ResourcesDictionaryTimeTracking, _row.NAZWISKO_IMIE, _row.ID );
        _new.EMail = "N/A";
        _new.JobTitle = _row.STANOWISKO;
        m_Entities.Resources.InsertOnSubmit( _new );
        m_ResourcesDictionaryMapping.Add( _row.LOGIN.ToLower(), _row.ID );
      }
      m_Entities.SubmitChanges();
    }
    #endregion

    #region mapping
    private Tasks CreateTask( TimeTracking.TimeTrackingDataSet.RODZAJPRACYRow rODZAJPRACYRow, Projects project, Resources resource )
    {
      Tasks _newTask = new Tasks()
      {
        Task2MilestoneDefinedInTitle = project.Milestone.FirstOrDefault(),
        Task2MilestoneResolvedInTitle = project.Milestone.LastOrDefault(),
        Task2ProjectTitle = project,
        Task2ResourcesTitle = resource,
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
    private Currency GetCurrency( string currency )
    {
      return Currency.PLN;
    }
    private Dictionary<string, int> m_ResourcesDictionaryMapping = new Dictionary<string, int>();
    #endregion

    #region data management
    private Estimation CheckAddEstimation( Resources resources, Projects project )
    {
      Estimation _estimation = ( from _eidx in project.Estimation let _ridx = _eidx.Estimation2ResourcesTitle where _eidx.Identyfikator == _ridx.Identyfikator select _eidx ).FirstOrDefault();
      if ( _estimation == null )
        _estimation = new Estimation()
        {
          EstimatedWorkload = 0,
          Estimation2ProjectTitle = project,
          Estimation2ResourcesTitle = resources
        };
      return _estimation;
    }
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
    private type Create<type>( EntityList<type> list, Dictionary<string, type> _dictionary, string title, string _key )
      where type: Element, new()
    {
      type _elmnt = new type();
      if ( !String.IsNullOrEmpty( title ) )
        _elmnt.Title = title;
      if ( _dictionary.Keys.Contains( _key ) )
        _key = Guid.NewGuid().ToString();
      _dictionary.Add( _key, _elmnt );
      list.InsertOnSubmit( _elmnt );
      return _elmnt;
    }
    private type GetOrAdd<type>( EntityList<type> _EDC, Dictionary<Guid, type> _dictionary, Guid _key )
          where type: Element, new()
    {
      if ( _dictionary.ContainsKey( _key ) )
        return _dictionary[ _key ];
      return Create<type>( _EDC, _dictionary, _key.ToString(), _key );
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
    private type GetOrAdd<type>( EntityList<type> _EDC, Dictionary<string, type> _dictionary, string _key )
      where type: Element, new()
    {
      if ( _dictionary.ContainsKey( _key ) )
        return _dictionary[ _key ];
      return Create<type>( _EDC, _dictionary, _key, _key );
    }
    private short m_EmptyKeyIdx = 0;
    private int UniqueKey() { return m_EmptyKeyIdx--; }
    private static string EmptyKey( int key ) { return String.Format( CultureInfo.InvariantCulture, "EmptyKey{0}", key ); }
    private static string EmptyKey( Guid key ) { return String.Format( CultureInfo.InvariantCulture, "EmptyKey{0}", key ); }
    #endregion

    #region Dictionaries
    private Dictionary<int, Tasks> m_TasksDictionary = new Dictionary<int, Tasks>();
    private Dictionary<int, Projects> m_ProjectsDictionaryTimeTracking = new Dictionary<int, Projects>();
    private Dictionary<int, Projects> m_ProjectsDictionaryBugNet = new Dictionary<int, Projects>();
    private Dictionary<int, Milestone> m_MilestoneDictionary = new Dictionary<int, Milestone>();
    private Dictionary<int, Resolution> m_ResolutionDictionary = new Dictionary<int, Resolution>();
    private Dictionary<int, TaskType> m_TaskTypeDictionary = new Dictionary<int, TaskType>();
    private Dictionary<int, Status> m_StatusDictionary = new Dictionary<int, Status>();
    private Dictionary<int, Priority> m_PriorityDictionary = new Dictionary<int, Priority>();
    private Dictionary<int, TaskComments> m_TaskCommentsDictionary = new Dictionary<int, TaskComments>();
    private Dictionary<int, Estimation> m_EstimationDictionary = new Dictionary<int, Estimation>();
    private Dictionary<int, Workload> m_WorkloadDictionary = new Dictionary<int, Workload>();
    private Dictionary<int, Contracts> m_ContractDictionary = new Dictionary<int, Contracts>();
    private Dictionary<int, Resources> m_ResourcesDictionaryTimeTracking = new Dictionary<int, Resources>();
    private Dictionary<Guid, Resources> m_ResourcesDictionaryBugNet = new Dictionary<Guid, Resources>();
    private Dictionary<int, Partners> m_PartnersDictionary = new Dictionary<int, Partners>();
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
