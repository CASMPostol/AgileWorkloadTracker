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
using CAS.SharePoint.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

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
    /// <param name="signalPreRenderCycle">To signal pre-render cycle and dispose the Linq Entities.</param>
    /// <returns>System.Collections.ObjectModel.ObservableCollection&lt;CAS.AgileWorkloadTracker.DataModel110.Linq.MilestoneWrapper&gt;.</returns>
    public static ObservableCollection<IMilestoneWrapper> GetMilestonesCollection(string siteURL, IPreRender signalPreRenderCycle)
    {
      m_URL = siteURL;
      m_Context = new DataContextManagement<LocalDataContext>(signalPreRenderCycle);
      List<Milestone> _mls = (from _milestone in m_Context.DataContext.Milestone
                                             where _milestone.Active.GetValueOrDefault(true)
                                             select _milestone).ToList();
      _mls.Sort();
      List<IMilestoneWrapper> _empty = new List<IMilestoneWrapper>();
      _empty.Add(new MilestoneWrapper(null));
      foreach (Milestone _fx in _mls)
        _empty.Add(new MilestoneWrapper(_fx));
      return new ObservableCollection<IMilestoneWrapper>(_empty);
    }
    /// <summary>
    /// Gets a value indicating whether this <see cref="EntitiesWrapper"/> is connected to the SharePoint.
    /// </summary>
    /// <value><c>true</c> if connected; otherwise, <c>false</c>.</value>
    public static bool Connected { get { return m_Context.DataContext != null; } }
    /// <summary>
    /// Persists to the content database changes made by the current user to one 
    /// or more lists using the specified failure mode; or, if a concurrency conflict 
    /// is found, populates the Microsoft.SharePoint.Linq.DataContext.ChangeConflicts property.
    /// </summary>
    public static void SubmitChanges()
    {
      Entities.SubmitChanges();
    }
    internal static Entities Entities
    {
      get
      {
        if (!Connected)
          throw new ArgumentOutOfRangeException("SharePoint DataContext is not created.");
        return m_Context.DataContext;
      }
    }
    //private
    private class LocalDataContext : Entities
    {
      public LocalDataContext() : base(m_URL) { }
    }
    private static DataContextManagement<LocalDataContext> m_Context;
    private static string m_URL = String.Empty;

  }
}
