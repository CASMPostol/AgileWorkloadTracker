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
    public Importer()
    {
      InitializeComponent();
    }
    public Importer( IContainer container )
    {
      container.Add( this );
      InitializeComponent();
    }
    internal void Import( Bugnet.DatabaseContentDataSet m_BugNETDataSet )
    {
      using ( Entities _entt = new Entities() )
      {
        Stage _newStage = new Stage() { Title = "Imported" };
        _entt.Stage.InsertOnSubmit( _newStage );
        Import( m_BugNETDataSet.Project, _newStage, _entt );
        Console.WriteLine( "Project" );
        Import( m_BugNETDataSet.Version, _entt );
        Console.WriteLine( "Version" );
        Import( m_BugNETDataSet.Status, _entt );
        Console.WriteLine( "Status" );
        Import( m_BugNETDataSet.Priority, _entt );
        Console.WriteLine( "Priority" );
        Import( m_BugNETDataSet.Resolution, _entt );
        Console.WriteLine( "Resolution" );
        Import( m_BugNETDataSet.Type, _entt );
        Console.WriteLine( "Type" );
        Import( m_BugNETDataSet.Bug, _entt );
        Console.WriteLine( "Bug" );
        Import( m_BugNETDataSet.BugComment, _entt );
        Console.WriteLine( "Starting SubmitChanges" );
        _entt.SubmitChanges();
      }

    }
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
    private void Import( Bugnet.DatabaseContentDataSet.VersionDataTable versionDataTable, Entities _entt )
    {
      foreach ( var _row in versionDataTable )
      {
        Milestone _new = Create<Milestone>( _entt.Milestone, m_MilestoneDictionary, _row.Name, _row.VersionID );
        if ( m_ProjectsDictionary.ContainsKey( _row.ProjectID ) )
          _new.Milestone2StageTitle = m_ProjectsDictionary[ _row.ProjectID ].Project2StageTitle;
        //TODO error handling mechnism
        //TODOD [AWT-3502] Add lookup from Milestones to Project  http://itrserver/Bugs/BugDetail.aspx?bid=3502
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
      foreach ( var _row in bugCommentDataTable )
      {
        // Create<TaskComments>( _entt.TaskComments, m_TaskCommentsDictionary, String.Empty, _row.BugCommentID );
        TaskComments _new = new TaskComments()
        {
          Body = _row.Comment,
          TaskComments2TaskTitle = GetOrAdd<Tasks>( _entt.Task, m_TasksDictionary, _row.BugID )
        };
        _entt.TaskComments.InsertOnSubmit( _new );
        //m_TaskCommentsDictionary.Add( _row.BugCommentID, _new );
      }
    }
    #region data management
    private type Create<type>( EntityList<type> list, Dictionary<int, type> _dictionary, string title, int _key )
      where type: Element, new()
    {
      type _elmnt = new type();
      if ( !String.IsNullOrEmpty( title ) )
        _elmnt.Title = title;
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
    #endregion
  }
}
