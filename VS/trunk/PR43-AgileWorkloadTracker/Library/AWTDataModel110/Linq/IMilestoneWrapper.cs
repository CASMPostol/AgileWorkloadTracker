//_______________________________________________________________
//  Title   : IMilestoneWrapper
//  System  : Microsoft VisualStudio 2013 / C#
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2015, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________

using System.ComponentModel;

namespace CAS.AgileWorkloadTracker.DataModel110.Linq
{
  /// <summary>
  /// Interface IMilestoneWrapper - 
  /// </summary>
  public interface IMilestoneWrapper : INotifyPropertyChanged
  {
    
    /// <summary>
    /// Gets or sets the active tasks.
    /// </summary>
    /// <value>The active tasks.</value>
    int ActiveTasks { get; set; }
    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    /// <value>The description.</value>
    string Description { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether [force inactive allowed].
    /// </summary>
    /// <value><c>true</c> if [force inactive allowed]; otherwise, <c>false</c>.</value>
    bool ForceInactiveAllowed { get; set; }
    /// <summary>
    /// Forces the make inactive.
    /// </summary>
    /// <param name="target">The target.</param>
    void ForceMakeInactive(IMilestoneWrapper target);
    /// <summary>
    /// Moves the specified target.
    /// </summary>
    /// <param name="target">The target.</param>
    void Move(IMilestoneWrapper target);
    /// <summary>
    /// Gets or sets a value indicating whether [not in progress].
    /// </summary>
    /// <value><c>true</c> if [not in progress]; otherwise, <c>false</c>.</value>
    bool NotInProgress { get; set; }
    /// <summary>
    /// Gets or sets the project title.
    /// </summary>
    /// <value>The project title.</value>
    string ProjectTitle { get; set; }
    /// <summary>
    /// Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
    string ToString();
    /// <summary>
    /// Updates this instance.
    /// </summary>
    void Update();

  }
}
