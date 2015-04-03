//_______________________________________________________________
//  Title   : Entities
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

using Microsoft.SharePoint;
using System.Linq;

namespace CAS.AgileWorkloadTracker.DataModel.Linq
{

  /// <summary>
  /// Provides strict typing LINQ (Language Integrated Query) access to, and change tracking for, the lists and document libraries of a Windows SharePoint Services "14" Web site.
  /// </summary>
  public partial class Entities
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="Entities" /> class.
    /// </summary>
    public Entities() : this(SPContext.Current.Web.Url) { }

    /// <summary>
    /// Get all <see cref="Requirements"/> for selected project <paramref name="projectID"/>.
    /// </summary>
    /// <param name="projectID">The project identifier.</param>
    /// <returns>IQueryable&lt;Requirements&gt;.</returns>
    public IQueryable<Requirements> ActiveRequirements(int projectID)
    {
      return from _requirement in this.Requirements
             //let _acv = _requirement.Requirements2MilestoneTitle == null || _requirement.Requirements2MilestoneTitle.Active.GetValueOrDefault( true )
             where _requirement.Requirements2ProjectsTitle.Id == projectID
             select _requirement;
    }

  }
}
