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
using SPExtensions = CAS.SharePoint.Extensions;
using CAS.SharePoint;

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
      Console.WriteLine( "Starting uploding data at: " + Properties.Settings.Default.RequestUrl );
      Bugnet.DatabaseContentDataSet m_BugNETDataSet = null;
      TimeTracking.TimeTrackingDataSet m_timeTrackingDataSet = null;
      using ( Entities _ent = Entities.GetEntitity() )
      {
        m_BugNETDataSet = m_importFromBugNet.GetDataFromDatabase();
        m_timeTrackingDataSet = m_importFromTimeTracking.GetDataFromDatabase();
        Import( m_timeTrackingDataSet.PRACOWNICY, _ent );
        Import( m_BugNETDataSet.aspnet_Users, _ent );
        Import( m_BugNETDataSet, _ent );
        Import( m_timeTrackingDataSet, _ent );
      }
    }
    #endregion

    #region Bugnet import
    private void Import( Bugnet.DatabaseContentDataSet m_BugNETDataSet, Entities _ent )
    {
      Import( m_BugNETDataSet.Project, GetDefaultStage( _ent ), _ent );
      Import( m_BugNETDataSet.Version, _ent, GetDefaultStage( _ent ) );
      Import( m_BugNETDataSet.Component, _ent );
      Import( m_BugNETDataSet.Status, _ent );
      Import( m_BugNETDataSet.Priority, _ent );
      Import( m_BugNETDataSet.Resolution, _ent );
      Import( m_BugNETDataSet.Type, _ent );
      Import( m_BugNETDataSet.Bug, m_BugNETDataSet.BugComment, _ent );
    }

    private void Import( Bugnet.DatabaseContentDataSet.ComponentDataTable componentDataTable, Entities _ent )
    {
      Console.WriteLine( "BugNet ComponentDataTable starting" );
      foreach ( Bugnet.DatabaseContentDataSet.ComponentRow _row in componentDataTable )
        Create<Category>( _ent.Category, m_CategoryDictionary, _row.Name, _row.ComponentID );
      _ent.SubmitChanges();
    }
    private void Import( Bugnet.DatabaseContentDataSet.aspnet_UsersDataTable usersDataTable, Entities m_Entities )
    {
      Console.WriteLine( "BugNet aspnet_UsersDataTable starting" );
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
      Console.WriteLine( "BugNet ProjectDataTable starting" );
      foreach ( var _row in projectDataTable )
      {
        Projects _new = Create<Projects>( _entt.Projects, m_ProjectsDictionaryBugNet, _row.Name, _row.ProjectID );
        _new.Active = _row.Active > 0;
        _new.Body = _row.IsDescriptionNull() ? "N/A" : _row.Description;
        _new.Project2ResourcesTitle = GetOrAdd<Resources>( _entt.Resources, m_ResourcesDictionaryBugNet, _row.ManagerUserID );
        _new.Project2StageTitle = stage;
        _new.ProjectNumber = _row.Code;
        _new.ProjectStartDate = _row.CreateDate;
        _new.ProjectType = ProjectType.ProjectInternal;
      }
      _entt.SubmitChanges();
    }
    private void Import( Bugnet.DatabaseContentDataSet.VersionDataTable versionDataTable, Entities _entt, Stage projectStage )
    {
      Console.WriteLine( "BugNet VersionDataTable starting" );
      foreach ( var _row in versionDataTable )
      {
        try
        {
          Milestone _new = Create<Milestone>( _entt.Milestone, m_MilestoneDictionary, _row.Name, _row.VersionID );
          _new.Default = false;
          _new.Milestone2ProjectTitle = GetOrAdd<Projects>( _entt.Projects, m_ProjectsDictionaryBugNet, _row.ProjectID );
          _new.SortOrder = _row.SortOrder;
          _new.BaselineEnd = _new.Milestone2ProjectTitle.ProjectEndDate.GetValueOrNull();
          _new.BaselineStart = _new.Milestone2ProjectTitle.ProjectStartDate.GetValueOrDefault();
          _new.MilestoneStart = SPExtensions.DateTimeNull;
          _new.MilestoneEnd = SPExtensions.DateTimeNull;
          _new.Milestone2StageTitle = projectStage;
          _new.Active = false;
          _new.MilestoneHours = 0;
          _entt.SubmitChanges();
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
      Console.WriteLine( "BugNet ResolutionDataTable starting" );
      foreach ( var _row in resutionDataTable )
        Create<Resolution>( _entt.Resolution, m_ResolutionDictionary, _row.Name, _row.ResolutionID );
      _entt.SubmitChanges();
    }
    private void Import( Bugnet.DatabaseContentDataSet.StatusDataTable statusDataTable, Entities _entt )
    {
      Console.WriteLine( "BugNet StatusDataTable starting" );
      foreach ( var _row in statusDataTable )
      {
        Status _cs = Create<Status>( _entt.Status, m_StatusDictionary, _row.Name, _row.StatusID );
        if ( _cs.Title.ToLower().Contains( "closed" ) )
          m_ClosedStatus = _cs;
      }
      _entt.SubmitChanges();
    }
    private void Import( Bugnet.DatabaseContentDataSet.TypeDataTable typeDataTable, Entities _entt )
    {
      Console.WriteLine( "BugNet TypeDataTable starting" );
      foreach ( var _row in typeDataTable )
        Create<TaskType>( _entt.Type, m_TaskTypeDictionary, _row.Name, _row.TypeID );
      _entt.SubmitChanges();
    }
    private void Import( Bugnet.DatabaseContentDataSet.PriorityDataTable priorityDataTable, Entities _entt )
    {
      Console.WriteLine( "BugNet PriorityDataTable starting" );
      foreach ( var _row in priorityDataTable )
        Create<Priority>( _entt.Priority, m_PriorityDictionary, _row.Name, _row.PriorityID );
      _entt.SubmitChanges();
    }
    private void Import( Bugnet.DatabaseContentDataSet.BugDataTable bugDataTable, Bugnet.DatabaseContentDataSet.BugCommentDataTable comments, Entities entt )
    {
      Console.WriteLine( "BugNet BugDataTable starting" );
      int _iteration = 0;
      int _bugId = 0;
      foreach ( var item in bugDataTable )
      {
        try
        {
          if ( item.LastUpdate < Properties.Settings.Default.WorkloadStartDate )
            continue;
          _bugId = item.BugID;
          Tasks _newTask = Create<Tasks>( entt.Task, null, item.Summary.SPValidSubstring(), item.BugID );
          _newTask.Body = item.Description;
          _newTask.Task2CategoryTitle = GetOrAdd<Category>( entt.Category, m_CategoryDictionary, item.ComponentID );
          _newTask.Task2MilestoneDefinedInTitle = GetOrAdd<Milestone>( entt.Milestone, m_MilestoneDictionary, item.VersionID );
          _newTask.Task2MilestoneResolvedInTitle = GetOrAdd<Milestone>( entt.Milestone, m_MilestoneDictionary, item.FixedInVersionId );
          _newTask.Task2ProjectTitle = GetOrAdd<Projects>( entt.Projects, m_ProjectsDictionaryBugNet, item.ProjectID );
          _newTask.BaselineEnd = _newTask.Task2MilestoneResolvedInTitle.BaselineEnd.GetValueOrNull();
          _newTask.BaselineStart = _newTask.Task2MilestoneResolvedInTitle.BaselineStart.GetValueOrNull();
          _newTask.TaskStart = SPExtensions.DateTimeNull;
          _newTask.TaskEnd = SPExtensions.DateTimeNull;
          if ( !item.IsAssignedToUserIdNull() )
            _newTask.Task2ResourcesTitle = GetOrAdd<Resources>( entt.Resources, m_ResourcesDictionaryBugNet, item.AssignedToUserId );
          else
            _newTask.Task2ResourcesTitle = null;
          _newTask.Task2SPriorityTitle = GetOrAdd<Priority>( entt.Priority, m_PriorityDictionary, item.PriorityID );
          _newTask.Task2SResolutionTitle = GetOrAdd<Resolution>( entt.Resolution, m_ResolutionDictionary, item.ResolutionID );
          if ( !_newTask.Task2SResolutionTitle.Title.ToLower().Contains( "closed" ) )
            _newTask.Task2MilestoneResolvedInTitle.Active = true;
          _newTask.Task2StatusTitle = GetOrAdd<Status>( entt.Status, m_StatusDictionary, item.StatusID );
          _newTask.Task2TypeTitle = GetOrAdd<TaskType>( entt.Type, m_TaskTypeDictionary, item.TypeID );
          GetOrAddEstimation( entt.Estimation, _newTask.Task2ResourcesTitle, _newTask.Task2ProjectTitle );
          foreach ( Bugnet.DatabaseContentDataSet.BugCommentRow _comment in item.GetBugCommentRows() )
            Import( _newTask, _comment, entt );
          foreach ( Bugnet.DatabaseContentDataSet.BugTimeEntryRow _te in item.GetBugTimeEntryRows() )
            Import( _newTask, _te, comments, entt );
          if ( _newTask.Task2TypeTitle.Title.ToLower().Contains( "new" ) )
          {
            Requirements _newRequirement = Create<Requirements>( entt.Requirements, null, _newTask.Task2TypeTitle.Title, -1 );
            _newRequirement.Body = _newTask.Body;
            _newRequirement.Requirements2MilestoneTitle = _newTask.Task2MilestoneResolvedInTitle;
            _newRequirement.Requirements2ProjectsTitle = _newTask.Task2ProjectTitle;
            _newRequirement.RequirementsType = RequirementsType.Functional;
          }
          Console.Write( "\r" );
          Console.Write( _iteration++ );
          if ( _iteration % 100 == 0 )
            entt.SubmitChanges();
        }
        catch ( Exception ex )
        {
          Console.WriteLine( String.Format( "Error importing BugDataTable of BugID: {0}, because of {1}", _bugId, ex.Message ) );
        }
      }
      entt.SubmitChanges();
    }
    private void Import( Tasks _newTask, Bugnet.DatabaseContentDataSet.BugTimeEntryRow _te, Bugnet.DatabaseContentDataSet.BugCommentDataTable _comments, Entities _entt )
    {
      int _bugId = 0;
      try
      {
        _bugId = _te.BugTimeEntryId;
        Resources _author = GetOrAdd<Resources>( _entt.Resources, m_ResourcesDictionaryBugNet, _te.CreatedUserId );
        Workload _new = new Workload()
        {
          Hours = Convert.ToDouble( _te.Duration ),
          ReadOnly = false,
          Title = _newTask.Title,//_comments[ _te.BugCommentId ].Comment,
          WeekNumber = WeekNumber( _te.WorkDate ),
          Workload2ProjectTitle = _newTask.Task2ProjectTitle,
          Workload2ResourcesTitle = _author,
          Workload2StageTitle = _newTask.Task2ProjectTitle == null ? null : _newTask.Task2ProjectTitle.Project2StageTitle,
          Workload2TaskTitle = _newTask,
          WorkloadDate = _te.WorkDate,
          Year = _te.WorkDate.Year
        };
        _entt.Workload.InsertOnSubmit( _new );
        _entt.SubmitChanges();
      }
      catch ( Exception _ex )
      {
        Console.WriteLine( String.Format( "Error importing BugTimeEntryRow of id: {0}, because of {1}", _bugId, _ex.Message ) );
      };
    }
    private void Import( Tasks task, Bugnet.DatabaseContentDataSet.BugCommentRow bugComment, Entities _entt )
    {
      int _bugId = 0;
      try
      {
        _bugId = bugComment.BugCommentID;
        TaskComments _new = new TaskComments()
        {
          Body = bugComment.Comment,
          TaskComments2TaskTitle = task
        };
        _entt.TaskComments.InsertOnSubmit( _new );
        _new.TaskComments2TaskTitle.Adjust( bugComment.CreatedDate );
        Resources _author = GetOrAdd<Resources>( _entt.Resources, m_ResourcesDictionaryBugNet, bugComment.CreatedUserId );
        GetOrAddEstimation( _entt.Estimation, _author, _new.TaskComments2TaskTitle.Task2ProjectTitle );
        _entt.SubmitChanges();
      }
      catch ( Exception _ex )
      {
        Console.WriteLine( String.Format( "Error importing BugCommentDataTable of Name: {0}, because of {1}", _bugId, _ex.Message ) );
      };
    }
    #endregion

    #region TimeTracking import
    private void Import( TimeTracking.TimeTrackingDataSet m_timeTrackingDataSet, Entities _ent )
    {
      Import( m_timeTrackingDataSet.KONTRAHENCI, _ent );
      Import( m_timeTrackingDataSet.UMOWY, _ent );
      //Import( m_timeTrackingDataSet.PLATNOSCI, m_Entities );
      //Import( m_timeTrackingDataSet.POLISY, m_Entities );
      Import( m_timeTrackingDataSet.PROJEKTY, _ent );
      Import( m_timeTrackingDataSet.PLAN, _ent );
      Import( m_timeTrackingDataSet.GODZINY, _ent );
    }
    private void Import( TimeTracking.TimeTrackingDataSet.PLANDataTable pLANDataTable, Entities m_Entities )
    {
      Console.WriteLine( "TimeTracking PLANDataTable starting" );
      foreach ( var _row in pLANDataTable )
      {
        if ( _row.IsID_PRACOWNIKANull() || _row.IsID_PRACOWNIKANull() )
          return;
        Estimation _new = GetOrAddEstimation
          (
            m_Entities.Estimation,
            GetOrAdd<Resources>( m_Entities.Resources, m_ResourcesDictionaryTimeTracking, _row.ID_PRACOWNIKA ),
            GetOrAdd<Projects>( m_Entities.Projects, m_ProjectsDictionaryTimeTracking, _row.ID_PROJEKTU )
          );
        _new.EstimatedWorkload = _row.GODZINY;
      }
      m_Entities.SubmitChanges();
    }
    private void Import( TimeTracking.TimeTrackingDataSet.GODZINYDataTable gODZINYDataTable, Entities m_Entities )
    {
      Console.WriteLine( "TimeTracking GODZINYDataTable starting" );
      int _iteration = 0;
      int _bugId = 0;
      foreach ( var _row in gODZINYDataTable )
      {
        try
        {
          if ( _row.IsDATANull() || _row.DATA < Properties.Settings.Default.WorkloadStartDate )
            continue;
          _bugId = _row.ID;
          Workload _new = Create<Workload>( m_Entities.Workload, null, _row.OPIS, _row.ID );
          _new.Hours = _row.IsLICZBA_GODZINNull() ? 0 : _row.LICZBA_GODZIN;
          _new.Workload2ProjectTitle = GetOrAdd<Projects>( m_Entities.Projects, m_ProjectsDictionaryTimeTracking, _row.ID_PROJEKTU );
          if ( !_row.IsID_PRACOWNIKANull() )
            _new.Workload2ResourcesTitle = GetOrAdd<Resources>( m_Entities.Resources, m_ResourcesDictionaryTimeTracking, _row.ID_PRACOWNIKA );
          _new.Workload2StageTitle = _new.Workload2ProjectTitle.Project2StageTitle;
          _new.WorkloadDate = _row.IsDATANull() ? SPExtensions.DateTimeNull : _row.DATA;
          _new.Workload2TaskTitle = CreateTask( m_Entities, _row.RODZAJPRACYRow, _new.Workload2ProjectTitle, _new.Workload2ResourcesTitle, _new.WorkloadDate.Value );
          _new.ReadOnly = true;
          _new.WeekNumber = WeekNumber( _new.WorkloadDate.Value );
          _new.Year = _new.WorkloadDate.Value.Year;
          if ( _new.WorkloadDate.HasValue )
            _new.Workload2TaskTitle.Adjust( _new.WorkloadDate.Value );
          Console.Write( "\r" );
          Console.Write( _iteration++ );
          if ( _iteration % 100 == 0 )
            m_Entities.SubmitChanges();
        }
        catch ( Exception ex )
        {
          Console.WriteLine( String.Format( "Error importing GODZINYDataTable of ID: {0}, because of {1}", _bugId, ex.Message ) );
        }
      }
      try
      {
        m_Entities.SubmitChanges();
      }
      catch ( Exception ex )
      {
        Console.WriteLine( String.Format( "Error importing GODZINYDataTable of ID: {0}, because of {1}", _bugId, ex.Message ) );
      }
    }
    private static int WeekNumber( DateTime date )
    {
      return date.DayOfYear / 7;
    }
    private void Import( TimeTracking.TimeTrackingDataSet.PROJEKTYDataTable pROJEKTYDataTable, Entities m_Entities )
    {
      Console.WriteLine( "TimeTracking PROJEKTYDataTable starting" );
      foreach ( var _row in pROJEKTYDataTable )
      {
        Projects _newProject = null;
        if ( ProjectsMapping.MappingTable.ContainsKey( _row.ID ) )
        {
          _newProject = GetOrAdd<Projects>( m_Entities.Projects, m_ProjectsDictionaryBugNet, ProjectsMapping.MappingTable[ _row.ID ] );
          m_ProjectsDictionaryTimeTracking.Add( _row.ID, _newProject );
        }
        else
        {
          _newProject = Create<Projects>( m_Entities.Projects, m_ProjectsDictionaryTimeTracking, _row.NAZWA_KROTKA, _row.ID );
          _newProject.Active = false;
        }
        _newProject.Body += String.Format( "<br/> <b> mapped with {0} <b/>", _row.IsNAZWANull() ? "N/A" : _row.NAZWA );
        _newProject.Currency = GetCurrency( "PLN" );
        if ( !_row.IsID_UMOWYNull() && _newProject.Project2ContractTitle == null )
          _newProject.Project2ContractTitle = GetOrAdd<Contracts>( m_Entities.Contracts, m_ContractDictionary, _row.ID_UMOWY );
        if ( _newProject.Project2ResourcesTitle == null && !_row.IsID_MANAGERANull() )
          _newProject.Project2ResourcesTitle = GetOrAdd<Resources>( m_Entities.Resources, m_ResourcesDictionaryTimeTracking, _row.ID_MANAGERA );
        if ( _newProject.Project2StageTitle == null )
          _newProject.Project2StageTitle = GetDefaultStage( m_Entities );
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
          _newProject.ProjectWarrantyDate = _row.IsDATA_GWARANCJANull() ? SPExtensions.DateTimeNull : _row.DATA_GWARANCJA;
      }
      m_Entities.SubmitChanges();
    }
    private void Import( TimeTracking.TimeTrackingDataSet.UMOWYDataTable uMOWYDataTable, Entities m_Entities )
    {
      Console.WriteLine( "TimeTracking UMOWYDataTable starting" );
      foreach ( var _row in uMOWYDataTable )
      {
        Contracts _newContract = Create<Contracts>( m_Entities.Contracts, m_ContractDictionary, _row.IsNAZWA_KROTKANull() ? "N/A" : _row.NAZWA_KROTKA, _row.ID );
        _newContract.Body = _row.IsPRZEDMIOTNull() ? "N/A" : _row.PRZEDMIOT.SPValidSubstring();
        _newContract.Body = _row.IsPRZEDMIOTNull() ? "N/A/" : _row.PRZEDMIOT.SPValidSubstring();
        _newContract.ContractDate = _row.IsDATA_UMOWYNull() ? SPExtensions.DateTimeNull : _row.DATA_UMOWY;
        _newContract.ContractEndDate = _row.IsKONIECNull() ? SPExtensions.DateTimeNull : _row.KONIEC;
        _newContract.ContractNumber = _row.IsNUMERNull() ? "N/A" : _row.NUMER;
        _newContract.ContractOffer = _row.IsOFERTANull() ? "N/A" : _row.OFERTA;
        _newContract.Contracts2PartnersTitle = _row.IsID_INWESTORANull() ? null : GetOrAdd<Partners>( m_Entities.Partners, m_PartnersDictionary, _row.ID_INWESTORA );
        _newContract.ContractSubject = _row.IsNAZWA_KROTKANull() ? "N/A" : _row.NAZWA_KROTKA;
        _newContract.ContractValue = _row.IsKONTRAKTNull() ? 0 : Convert.ToDouble( _row.KONTRAKT );
        _newContract.ContractWarrantyDate = _row.IsGWARANCJANull() ? SPExtensions.DateTimeNull : _row.GWARANCJA;
        _newContract.Currency = _row.IsWALUTANull() ? Currency.Invalid : GetCurrency( _row.WALUTA );
      }
      m_Entities.SubmitChanges();
    }
    private void Import( TimeTracking.TimeTrackingDataSet.KONTRAHENCIDataTable kONTRAHENCIDataTable, Entities m_Entities )
    {
      Console.WriteLine( "TimeTracking KONTRAHENCIDataTable starting" );
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
      m_Entities.SubmitChanges();
    }
    private void Import( TimeTracking.TimeTrackingDataSet.PRACOWNICYDataTable pRACOWNICYDataTable, Entities m_Entities )
    {
      Console.WriteLine( "TimeTracking PRACOWNICYDataTable starting" );
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
    private Tasks CreateTask( Entities entities, TimeTracking.TimeTrackingDataSet.RODZAJPRACYRow rODZAJPRACYRow, Projects project, Resources resource, DateTime workloadDate )
    {
      Tasks _newTask = new Tasks()
      {
        Task2MilestoneDefinedInTitle = project.Milestone.FirstOrDefault(),
        Task2MilestoneResolvedInTitle = project.Milestone.LastOrDefault(),
        Task2ProjectTitle = project,
        Task2ResourcesTitle = resource,
        Task2SPriorityTitle = null,
        Task2SResolutionTitle = null,
        Task2StatusTitle = m_ClosedStatus,
        Task2TypeTitle = null,
        Title = rODZAJPRACYRow == null ? "N/A" : rODZAJPRACYRow.NAZWAPRACY,
        BaselineEnd = workloadDate,
        BaselineStart = workloadDate,
        Body = rODZAJPRACYRow == null ? "N/A" : rODZAJPRACYRow.OPIS,
        TaskEnd = workloadDate,
        TaskStart = workloadDate
      };
      entities.Task.InsertOnSubmit( _newTask );
      return _newTask;
    }
    private Currency GetCurrency( string currency )
    {
      return Currency.PLN;
    }
    private Dictionary<string, int> m_ResourcesDictionaryMapping = new Dictionary<string, int>();
    #endregion

    #region data management
    private Estimation GetOrAddEstimation( EntityList<Estimation> esimationList, Resources resources, Projects project )
    {
      if ( resources == null || project == null )
        return null;
      Estimation _estimation = ( from _eidx in project.Estimation let _ridx = _eidx.Estimation2ResourcesTitle where resources.Identyfikator == _ridx.Identyfikator select _eidx ).FirstOrDefault();
      if ( _estimation == null )
      {
        _estimation = new Estimation()
        {
          EstimatedWorkload = 0,
          Estimation2ProjectTitle = project,
          Estimation2ResourcesTitle = resources
        };
        esimationList.InsertOnSubmit( _estimation );
      }
      return _estimation;
    }
    private type Create<type>( EntityList<type> list, Dictionary<int, type> _dictionary, string title, int _key )
      where type: Element, new()
    {
      type _elmnt = new type();
      if ( !String.IsNullOrEmpty( title ) )
        _elmnt.Title = title.SPValidSubstring();
      if ( _dictionary != null )
      {
        if ( _dictionary.Keys.Contains( _key ) )
          _key = UniqueKey();
        _dictionary.Add( _key, _elmnt );
      }
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
    private Dictionary<int, Projects> m_ProjectsDictionaryTimeTracking = new Dictionary<int, Projects>();
    private Dictionary<int, Projects> m_ProjectsDictionaryBugNet = new Dictionary<int, Projects>();
    private Dictionary<int, Milestone> m_MilestoneDictionary = new Dictionary<int, Milestone>();
    private Dictionary<int, Resolution> m_ResolutionDictionary = new Dictionary<int, Resolution>();
    private Dictionary<int, TaskType> m_TaskTypeDictionary = new Dictionary<int, TaskType>();
    private Dictionary<int, Category> m_CategoryDictionary = new Dictionary<int, Category>();
    private Dictionary<int, Status> m_StatusDictionary = new Dictionary<int, Status>();
    private Dictionary<int, Priority> m_PriorityDictionary = new Dictionary<int, Priority>();
    private Dictionary<int, Estimation> m_EstimationDictionary = new Dictionary<int, Estimation>();
    private Dictionary<int, Contracts> m_ContractDictionary = new Dictionary<int, Contracts>();
    private Dictionary<int, Resources> m_ResourcesDictionaryTimeTracking = new Dictionary<int, Resources>();
    private Dictionary<Guid, Resources> m_ResourcesDictionaryBugNet = new Dictionary<Guid, Resources>();
    private Dictionary<int, Partners> m_PartnersDictionary = new Dictionary<int, Partners>();
    #endregion

    #region private
    private Status m_ClosedStatus = null;
    private const string DefaultStageTitle = "Imported";
    private Stage p_DefaultStage = null;
    private Stage GetDefaultStage( Entities entities )
    {
      if ( p_DefaultStage == null )
        p_DefaultStage = ( from _sidx in entities.Stage where _sidx.Title.Contains( DefaultStageTitle ) select _sidx ).FirstOrDefault();
      if ( p_DefaultStage == null )
      {
        p_DefaultStage = new Stage() { Title = DefaultStageTitle };
        entities.Stage.InsertOnSubmit( p_DefaultStage );
      }
      return p_DefaultStage;
    }
    //private Entities p_Entities = null;
    //private Entities m_Entities
    //{
    //  get
    //  {
    //    if ( p_Entities == null )
    //      p_Entities = Entities.GetEntitity();
    //    return p_Entities;
    //  }
    //}
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
      //if ( p_Entities != null )
      //{
      //p_Entities.Dispose();
      //p_Entities = null;
      //}
      base.Dispose( disposing );
    }
    #endregion

  }
}
