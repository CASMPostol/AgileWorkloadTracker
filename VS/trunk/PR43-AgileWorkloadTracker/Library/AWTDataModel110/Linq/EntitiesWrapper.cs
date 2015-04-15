//_______________________________________________________________
//  Title   : EntitiesWrapper
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

using CAS.AgileWorkloadTracker.DataModel.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CAS.AgileWorkloadTracker.DataModel110.Linq
{
  /// <summary>
  /// Class EntitiesWrapper.
  /// </summary>
  public static class EntitiesWrapper
  {
    /// <summary>
    /// Gets the milestones collection.
    /// </summary>
    /// <param name="siteURL">The site URL.</param>
    /// <returns>System.Collections.ObjectModel.ObservableCollection&lt;CAS.AgileWorkloadTracker.DataModel110.Linq.MilestoneWrapper&gt;.</returns>
    public static ObservableCollection<IMilestoneWrapper> GetMilestonesCollection(string siteURL)
    {
      using (CAS.AgileWorkloadTracker.DataModel.Linq.Entities m_Context = new Entities(siteURL))
      {
        List<Milestone> _mls = (from _milestone in m_Context.Milestone
                                where _milestone.Active.GetValueOrDefault(true)
                                select _milestone).ToList();
        _mls.Sort();
        List<IMilestoneWrapper> _empty = new List<IMilestoneWrapper>();
        _empty.Add(new MilestoneWrapper(null));
        foreach (Milestone _fx in _mls)
          _empty.Add(new MilestoneWrapper(_fx));
        return new ObservableCollection<IMilestoneWrapper>(_empty);

      }
    }

  }
}
