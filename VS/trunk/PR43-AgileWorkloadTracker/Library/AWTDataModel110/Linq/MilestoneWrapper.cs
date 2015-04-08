//<summary>
//  Title   : class MilestoneWrapper 
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.AgileWorkloadTracker.DataModel110.Linq
{
  /// <summary>
  /// MilestoneWrapper class
  /// </summary>
  internal class MilestoneWrapper : ElementWrapper<DataModel.Linq.Milestone>, CAS.AgileWorkloadTracker.DataModel110.Linq.IMilestoneWrapper
  {

    #region creator
    /// <summary>
    /// Initializes a new instance of the <see cref="MilestoneWrapper"/> class.
    /// </summary>
    /// <param name="milestone">The milestone.</param>
    public MilestoneWrapper(DataModel.Linq.Milestone milestone)
      : base(milestone)
    {
      if (milestone == null)
      {
        return;
      }
      b_ProjectTitle = milestone.Milestone2ProjectTitle.Title;
      List<DataModel.Linq.Tasks> _activeTasksList = (from _tsx in Element.Tasks0
                                                     where _tsx.Active.GetValueOrDefault(false)
                                                     select _tsx).ToList();
      _activeTasksList.Sort();
      b_ActiveTasks = _activeTasksList.Count;
      StringBuilder _description = new StringBuilder();
      double _hours = 0;
      foreach (var _at in _activeTasksList)
        _hours += _at.Workload.Sum(x => x.Hours.GetValueOrDefault(0));
      _description.AppendFormat("Hours={4:f0} Accepted Hours={0:f0} Estimated Hours={1:f0} Hours in progress {2:f2} Tasks {3}",
                                 milestone.AcceptedHours.GetValueOrDefault(0), milestone.EstimatedHours.GetValueOrDefault(0), _hours, Element.Tasks0.Count(), milestone.MilestoneHours.GetValueOrDefault(0));
      _description.AppendLine();
      _description.AppendLine("Requirements:");
      NotInProgress = true;
      foreach (var _rqx in milestone.Requirements)
      {
        _description.AppendLine(_rqx.Title);
        foreach (var _tsx in from _tsx in _rqx.Tasks where _tsx.Active.GetValueOrDefault(false) orderby _tsx.Title select _tsx)
        {
          _description.AppendFormat("    Task:{0} hours: {1:f2}", _tsx.Title, _tsx.Workload.Sum(x => x.Hours.GetValueOrDefault(0)));
          _description.AppendLine();
          NotInProgress = false;
        }
      }
      ForceInactiveAllowed = !NotInProgress;
      _description.AppendLine();
      _description.AppendLine("{0}");
      b_Description = _description.ToString();
    }
    #endregion

    #region properties
    /// <summary>
    /// Gets or sets a value indicating whether the Milestone is not in progress - may be made inactive.
    /// </summary>
    /// <value>
    ///   <c>true</c> if [not in progress]; otherwise, <c>false</c>.
    /// </value>
    public bool NotInProgress
    {
      get
      {
        return b_NotInProgress;
      }
      set
      {
        RaiseHandler<bool>(value, ref b_NotInProgress, "NotInProgress", this);
      }
    }
    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    /// <value>The description.</value>
    public string Description
    {
      get
      {
        return String.Format(b_Description, NotInProgress ? Element.Active.GetValueOrDefault(false) ? "The milestone may be made inactive." : "The milestone is already be inactive." : "The milestone cannot be made inactive");
      }
      set
      {
        RaiseHandler<string>(value, ref b_Description, "Description", this);
      }
    }
    /// <summary>
    /// Gets or sets the active tasks.
    /// </summary>
    /// <value>The active tasks.</value>
    public int ActiveTasks
    {
      get
      {
        return b_ActiveTasks;
      }
      set
      {
        RaiseHandler<int>(value, ref b_ActiveTasks, "ActiveTasks", this);
      }
    }
    /// <summary>
    /// Gets or sets the project title.
    /// </summary>
    /// <value>The project title.</value>
    public string ProjectTitle
    {
      get
      {
        return b_ProjectTitle;
      }
      set
      {
        RaiseHandler<string>(value, ref b_ProjectTitle, "ProjectTitle", this);
      }
    }
    /// <summary>
    /// Gets or sets a value indicating whether [force inactiv allowed].
    /// </summary>
    /// <value><c>true</c> if [force inactiv allowed]; otherwise, <c>false</c>.</value>
    public bool ForceInactiveAllowed
    {
      get
      {
        return b_ForceInactivAllowed;
      }
      set
      {
        RaiseHandler<bool>(value, ref b_ForceInactivAllowed, "ForceInactivAllowed", this);
      }
    }
    #endregion

    #region public API
    /// <summary>
    /// Updates this instance.
    /// </summary>
    /// <exception cref="System.ArgumentNullException">Element</exception>
    public void Update()
    {
      if (base.Element == null)
        throw new ArgumentNullException("Element");
      Element.Update(EntitiesWrapper.Entities);
    }
    /// <summary>
    /// Forces the make inactive.
    /// </summary>
    /// <param name="target">The target.</param>
    /// <exception cref="System.ArgumentNullException">Element</exception>
    public void ForceMakeInactive(IMilestoneWrapper target)
    {
      if (base.Element == null)
        throw new ArgumentNullException("Element");
      Element.ForceMakeInactive(EntitiesWrapper.Entities, ((MilestoneWrapper)target).Element);
    }
    /// <summary>
    /// Moves the specified target.
    /// </summary>
    /// <param name="target">The target.</param>
    /// <exception cref="System.ArgumentNullException">Element</exception>
    public void Move(IMilestoneWrapper target)
    {
      if (base.Element == null)
        throw new ArgumentNullException("Element");
      Element.Move(EntitiesWrapper.Entities, ((MilestoneWrapper)target).Element);
    }
    #endregion

    #region object
    /// <summary>
    /// Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
    public override string ToString()
    {
      if (Element == null)
        return base.ToString();
      return String.Format("{2}:{0} Active tasks {1}", Element.Title, ActiveTasks, ProjectTitle);
    }
    #endregion

    #region private
    private bool b_ForceInactivAllowed = false;
    private string b_Description = String.Empty;
    private int b_ActiveTasks = 0;
    private bool b_NotInProgress = false;
    private string b_ProjectTitle = "N/A";
    #endregion

  }
}
