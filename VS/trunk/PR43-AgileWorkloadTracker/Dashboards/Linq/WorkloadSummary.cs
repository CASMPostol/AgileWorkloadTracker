using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Linq;
using CAS.AgileWorkloadTracker.Linq;
using CAS.SharePoint;

namespace CAS.AgileWorkloadTracker.Dashboards.Linq
{
  internal class WorkloadSummary
  {

    #region public
    /// <summary>
    /// Gets or sets the scope OF calculation.
    /// </summary>
    /// <value>
    /// The labeb describing the scope.
    /// </value>
    public string Scope { get; set; }
    /// <summary>
    /// Gets or sets the available hours in the scope.
    /// </summary>
    /// <value>
    /// The available hours.
    /// </value>
    public double Available { get; set; }
    /// <summary>
    /// Gets or sets the number of allocated hours in this scope.
    /// </summary>
    /// <value>
    /// The allocated hours.
    /// </value>
    public double Allocated { get; set; }
    /// <summary>
    /// Gets or sets the number of already reported hours in this scope.
    /// </summary>
    /// <value>
    /// The reported.
    /// </value>
    public double Reported { get; set; }
    /// <summary>
    /// Summarises the Workload in all available scopes.
    /// </summary>
    /// <param name="projects">The _projects.</param>
    /// <param name="me">The _me.</param>
    /// <param name="selectedProject">The selected project.</param>
    /// <returns></returns>
    public static List<WorkloadSummary> WorkloadSummaryList( IQueryable<Projects> projects, Resources me )
    {
      try
      {
        List<WorkloadSummary> _ret = new List<WorkloadSummary>();
        WorkloadSummary _AllInAllProjects = null;
        if ( projects != null )
        {
          _AllInAllProjects = AllInAllProjects( projects );
          _ret.Add( _AllInAllProjects );
        }
        if ( me != null )
        {
          double _myAvailableHours = _AllInAllProjects == null ? 0 : _AllInAllProjects.Available - Math.Max( _AllInAllProjects.Allocated, _AllInAllProjects.Reported );
          _ret.Add( UserInAllProjets( me, _myAvailableHours ) );
        }
        foreach ( var _selectedProject in projects )
        {
          WorkloadSummary _AllInSelectedProject = AllInSelectedProject( _selectedProject );
          _ret.Add( _AllInSelectedProject );
          if ( me != null )
          {
            double _availableForMe = _selectedProject.ProjectHours.GetValueOrDefault( 0 ) - Math.Max( _AllInSelectedProject.Allocated, _AllInSelectedProject.Reported );
            _ret.Add( UserInSelectedProject( me, _selectedProject, _availableForMe ) );
          }
        }
        return _ret;
      }
      catch ( Exception _ex)
      {
        throw new ApplicationError("WorkloadSummaryList", "N/A", _ex.Message, _ex);
      }
    }
    #endregion

    #region private
    private static WorkloadSummary UserInSelectedProject( Resources _me, Projects SelectedProject, double _availableForMe )
    {
      WorkloadSummary _ret = new WorkloadSummary() { Scope = String.Format( @"You in the project: {0}", SelectedProject.Title ), Available = _availableForMe };
      var _myEstimates = from _eidx in SelectedProject.Estimation where _eidx.Estimation2ResourcesTitle == _me select _eidx;
      _ret.Allocated = _myEstimates.Sum( _x => _x.EstimatedWorkload.GetValueOrDefault( 0 ) );
      var _myWokloads = from _widx in SelectedProject.Workload where _widx.Workload2ResourcesTitle == _me select _widx;
      _ret.Reported = _myWokloads.Sum( _x => _x.Hours.GetValueOrDefault( 0 ) );
      return _ret;
    }
    private static WorkloadSummary AllInSelectedProject( Projects SelectedProject )
    {
      WorkloadSummary _ret = new WorkloadSummary() { Scope = String.Format( @"All users in the project: {0}", SelectedProject.Title ) };
      _ret.Allocated = SelectedProject.Estimation.Sum( _x => _x.EstimatedWorkload.GetValueOrDefault( 0 ) );
      _ret.Reported = SelectedProject.Workload.Sum( _x => _x.Hours.GetValueOrDefault( 0 ) );
      _ret.Available = SelectedProject.ProjectHours.GetValueOrDefault( 0 );
      return _ret;
    }
    private static WorkloadSummary UserInAllProjets( Resources _me, double _myAvailableHours )
    {
      WorkloadSummary _ret = new WorkloadSummary() { Scope = "You in all projects" };
      _ret.Available = _myAvailableHours;
      var _myActiveWorkloads = from _widx in _me.Workload where _widx.Workload2ProjectTitle.Active.GetValueOrDefault( true ) select _widx;
      _ret.Reported = _myActiveWorkloads.Sum( _wld => _wld.Hours.GetValueOrDefault( 0 ) );
      var _myActiveEstimate = from _estimate in _me.Estimation where _estimate.Estimation2ProjectTitle.Active.GetValueOrDefault( true ) select _estimate;
      _ret.Allocated = _myActiveEstimate.Sum( _est => _est.EstimatedWorkload.GetValueOrDefault( 0 ) );
      return _ret;
    }
    private static WorkloadSummary AllInAllProjects( IQueryable<Projects> _projects )
    {
      WorkloadSummary _ret = new WorkloadSummary() { Scope = "All users in all projects" };
      _ret.Available = _projects.Sum( _idx => _idx.ProjectHours.GetValueOrDefault( 0 ) );
      var _allEstmations = from _pix in _projects let _eent = _pix.Estimation from _eix in _eent select _eix;
      _ret.Allocated = _allEstmations.Sum( _x => _x.EstimatedWorkload.GetValueOrDefault( 0 ) );
      var _allWorkloads = from _pix in _projects let _wkEnt = _pix.Workload from _widx in _wkEnt select _widx;
      _ret.Reported = _allWorkloads.Sum( _x => _x.Hours.GetValueOrDefault( 0 ) );
      return _ret;
    }
    private WorkloadSummary() { }
    #endregion

  }
}
