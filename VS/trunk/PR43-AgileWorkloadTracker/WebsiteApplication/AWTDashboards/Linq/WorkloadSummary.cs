//_______________________________________________________________
//  Title   : WorkloadSummary
//  System  : Microsoft VisualStudio 2013 / C#
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL: $
//  $Id$
//
//  Copyright (C) 2015, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________

using CAS.AgileWorkloadTracker.DataModel.Linq;
using CAS.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CAS.AgileWorkloadTracker.Dashboards.Linq
{
  internal class WorkloadSummary
  {

    #region public
    /// <summary>
    /// Gets or sets the scope of calculation.
    /// </summary>
    /// <value>
    /// The label describing the scope.
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
    /// Summarizes the Workload in all available scopes.
    /// </summary>
    /// <param name="projects">The _projects.</param>
    /// <param name="me">The _me.</param>
    /// <param name="selectedProject">The selected project.</param>
    /// <returns></returns>
    public static List<WorkloadSummary> WorkloadSummaryList(Entities edc, IQueryable<Projects> projects, Users me)
    {
      try
      {
        List<WorkloadSummary> _ret = new List<WorkloadSummary>();
        WorkloadSummary _AllInAllProjects = null;
        if (projects != null)
        {
          _AllInAllProjects = AllInAllProjects(projects);
          _ret.Add(_AllInAllProjects);
        }
        if (me != null)
        {
          double _myAvailableHours = _AllInAllProjects == null ? 0 : _AllInAllProjects.Available - Math.Max(_AllInAllProjects.Allocated, _AllInAllProjects.Reported);
          _ret.Add(UserInAllProjects(edc, me, _myAvailableHours));
        }
        foreach (var _selectedProject in projects)
        {
          WorkloadSummary _AllInSelectedProject = AllInSelectedProject(_selectedProject);
          _ret.Add(_AllInSelectedProject);
          if (me != null)
          {
            double _availableForMe = _selectedProject.ProjectHours.GetValueOrDefault(0) - Math.Max(_AllInSelectedProject.Allocated, _AllInSelectedProject.Reported);
            _ret.Add(UserInSelectedProject(me, _selectedProject, _availableForMe));
          }
        }
        return _ret;
      }
      catch (Exception _ex)
      {
        throw new ApplicationError("WorkloadSummaryList", "N/A", _ex.Message, _ex);
      }
    }
    #endregion

    #region private
    private static WorkloadSummary UserInSelectedProject(Users me, Projects SelectedProject, double _availableForMe)
    {
      WorkloadSummary _ret = new WorkloadSummary() { Scope = String.Format(@"You in the project: {0}", SelectedProject.Title), Available = _availableForMe };
      var _myEstimates = from _Estimation in SelectedProject.Estimation where _Estimation.AssignedTo.Id == me.Id select _Estimation;
      _ret.Allocated = _myEstimates.Sum(_x => _x.EstimatedWorkload.GetValueOrDefault(0));
      var _myWorkloads = from _workload in SelectedProject.Workload where _workload.AssignedTo.Id == me.Id select _workload;
      _ret.Reported = _myWorkloads.Sum(_x => _x.MyHours);
      return _ret;
    }
    private static WorkloadSummary AllInSelectedProject(Projects SelectedProject)
    {
      WorkloadSummary _ret = new WorkloadSummary() { Scope = String.Format(@"All users in the project: {0}", SelectedProject.Title) };
      _ret.Allocated = SelectedProject.Estimation.Sum(_x => _x.EstimatedWorkload.GetValueOrDefault(0));
      _ret.Reported = SelectedProject.Workload.Sum(_x => _x.MyHours);
      _ret.Available = SelectedProject.ProjectHours.GetValueOrDefault(0);
      return _ret;
    }
    private static WorkloadSummary UserInAllProjects(Entities edc, Users me, double myAvailableHours)
    {
      WorkloadSummary _ret = new WorkloadSummary() { Scope = "You in all projects" };
      _ret.Available = myAvailableHours;
      var _myActiveWorkloads = from _widx in edc.Workload where _widx.Workload2ProjectTitle.Active.GetValueOrDefault(true) && _widx.AssignedTo.Id == me.Id select _widx;
      _ret.Reported = _myActiveWorkloads.Sum(_wld => _wld.MyHours);
      var _myActiveEstimate = from _estimate in edc.Estimation where _estimate.Estimation2ProjectTitle.Active.GetValueOrDefault(true) && _estimate.AssignedTo.Id == me.Id select _estimate;
      _ret.Allocated = _myActiveEstimate.Sum(_est => _est.EstimatedWorkload.GetValueOrDefault(0));
      return _ret;
    }
    private static WorkloadSummary AllInAllProjects(IQueryable<Projects> projects)
    {
      WorkloadSummary _ret = new WorkloadSummary() { Scope = "All users in all projects" };
      _ret.Available = projects.Sum(_idx => _idx.ProjectHours.GetValueOrDefault(0));
      IEnumerable<Estimation> _allEstimations = from _pix in projects let _estimation = _pix.Estimation from _eix in _estimation select _eix;
      _ret.Allocated = _allEstimations.Sum(_x => _x.EstimatedWorkload.GetValueOrDefault(0));
      IEnumerable<Workload> _allWorkloads = from _pix in projects let _workload = _pix.Workload from _widx in _workload select _widx;
      _ret.Reported = _allWorkloads.Sum(_x => _x.MyHours);
      return _ret;
    }
    private WorkloadSummary() { }
    #endregion

  }
}
