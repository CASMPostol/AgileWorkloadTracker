//<summary>
//  Title   : Entities partial classes
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL:$
//  $Id$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>
using System;
using System.Diagnostics;
using System.Linq;
using System.Web.UI.WebControls;
using CAS.AgileWorkloadTracker.Dashboards;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Linq;
using CAS.SharePoint;
using System.Collections.Generic;

namespace CAS.AgileWorkloadTracker.Linq
{
  /// <summary>
  /// Adds a message to the Event log list.
  /// </summary>
  internal partial class Anons
  {
    /// <summary>
    /// Creates an entry with the given message text and application-defined event identifier to the event log list.
    /// </summary>
    /// <param name="source">The source denominator of the message.</param>
    /// <param name="message">The string to write to the event log.</param>
    internal Anons( string source, string message )
    {
      Title = source;
      Treść = message;
      this.Wygasa = DateTime.Now + new TimeSpan( 2, 0, 0, 0 );
    }
    /// <summary>
    /// Writes an entry with the given message text and application-defined event identifier to the event log list. 
    /// </summary>
    /// <param name="edc">Provides LINQ (Language Integrated Query) access to, and change tracking for, 
    /// the lists and document libraries of a Windows SharePoint Services "14" Web site.</param>
    /// <param name="source">The source denominator of the message.</param>
    /// <param name="message">The string to write to the event log.</param>
    internal static void WriteEntry( string source, string message )
    {
      try
      {
        using ( Entities edc = new Entities( SPContext.Current.Web.Url ) )
        {
          Anons log = new Anons( source, message );
          edc.Announcements.InsertOnSubmit( log );
          edc.SubmitChanges();
        }
      }
      catch ( Exception _ex )
      {
        string _msg = String.Format( "Cannot write to \"Event Log List\" because of error: {0}", _ex.Message );
        EventLog.WriteEntry( "CAS.SmartFActory", _msg, EventLogEntryType.Error, 114 );
      }
      return;
    }
  }
  internal partial class Entities
  {
    public Entities() : base( SPContext.Current.Web.Url ) { }
    internal IQueryable<Requirements> ActiveRequirements( int projectID )
    {
      return from _rsrcx in this.Requirements
             //let _acv = _rsrcx.Requirements2MilestoneTitle == null || _rsrcx.Requirements2MilestoneTitle.Active.GetValueOrDefault( true )
             where _rsrcx.Requirements2ProjectsTitle.Identyfikator == projectID
             select _rsrcx;
    }
  }
  /// <summary>
  /// Partial classes for the Linq entities.
  /// </summary>
  internal partial class Element
  {
    internal const string IDColunmName = "ID";
    internal const string TitleColunmName = "Title";
    /// <summary>
    /// Try to get at index. 
    /// </summary>
    /// <typeparam name="t"></typeparam>
    /// <param name="_list">The _list.</param>
    /// <param name="_ID">The _ ID.</param>
    /// <exception cref="ApplicationException">Element cannot be found.</exception>
    /// <returns>An instance of the <see cref="t"/> for the selected index or null if <paramref name="_ID"/> is null or empty.</returns>
    internal static t TryGetAtIndex<t>( EntityList<t> _list, string _ID )
      where t: Element
    {
      if ( _ID.IsNullOrEmpty() )
        return null;
      return GetAtIndex<t>( _list, _ID );
    }
    /// <summary>
    /// Gets at index.
    /// </summary>
    /// <typeparam name="t"></typeparam>
    /// <param name="_list">The _list.</param>
    /// <param name="_ID">The _ ID.</param>
    /// <exception cref="ApplicationException">_ID is nuul or element cannot be found.</exception>
    /// <returns>An instance of the <see cref="t"/> for the selected index.</returns>
    internal static t GetAtIndex<t>( EntityList<t> _list, string _ID )
      where t: Element
    {
      int? _index = _ID.String2Int();
      if ( !_index.HasValue )
        throw new ApplicationException( typeof( t ).Name + " index is null" );
      try
      {
        return GetAtIndex<t>( _list, _index.Value );
      }
      catch ( Exception )
      {
        throw new ApplicationException( String.Format( "{0} cannot be found at specified index{1}", typeof( t ).Name, _index.Value ) );
      }
    }
    /// <summary>
    /// Gets an entitz at index.
    /// </summary>
    /// <typeparam name="t"></typeparam>
    /// <param name="_list">The list.</param>
    /// <param name="_index">The index of the item to be returned.</param>
    /// <returns>The list item of the <typeparamref name="t"/> type</returns>
    internal static t GetAtIndex<t>( EntityList<t> _list, int _index ) where t: Element
    {
      return ( from idx in _list where idx.Identyfikator == _index select idx ).First();
    }
    internal static t FindAtIndex<t>( EntityList<t> _list, string _ID )
      where t: Element
    {
      int? _index = _ID.String2Int();
      if ( !_index.HasValue )
        return null;
      try
      {
        return (
              from idx in _list
              where idx.Identyfikator == _index.Value
              select idx ).FirstOrDefault();
      }
      catch ( Exception )
      {
        return null;
      }
    }
  }
  internal partial class Resources
  {
    internal static Resources FindForUser( Entities edc, SPUser _user )
    {
      if ( edc == null )
        throw new ArgumentNullException( "FindForUser requres the edc parameter to be assigned" );
      EntityList<Resources> _res = edc.Resources;
      if ( _res == null || _res.Count() == 0 )
        return null;
      return (
                from idx in _res
                let _account = idx.EmployeeADAccountTitle
                where _account.Contains( _user.Name )
                select idx
              ).FirstOrDefault();
    }
  }
  internal static class LinqExtensions
  {
    public static TEntity GetSelected<TEntity>( this DropDownList dropDown, EntityList<TEntity> entities )
      where TEntity: Element
    {
      if ( dropDown.SelectedIndex < 0 )
        return null;
      return ( from _ix in entities where _ix.Identyfikator == dropDown.SelectedValue.String2Int() select _ix ).FirstOrDefault<TEntity>();
    }
    public static void EntityListDataSource<TEntity>( this DropDownList dropDown, IQueryable<TEntity> entities )
      where TEntity: Element
    {
      dropDown.DataSource = from _tpx in entities orderby _tpx.Title select new { ID = _tpx.Identyfikator, Title = _tpx.Title };
      dropDown.DataTextField = "Title";
      dropDown.DataValueField = "ID";
      dropDown.DataBind();
      dropDown.ClearSelection();
    }
    /// <summary>
    /// Selects the item for the element. If item cannot be selected (is not on the list) selection of the <paramref name="dropDown"/> is cleared.
    /// </summary>
    /// <param name="dropDown">The drop down to be updated.</param>
    /// <param name="element">The element pointing out the item to be selected.</param>
    public static void SelectItem4Element( this DropDownList dropDown, Element element )
    {
      dropDown.ClearSelection();
      if ( element != null )
      {
        ListItem _item = dropDown.Items.FindByValue( element.Identyfikator.IntToString() );
        if ( _item != null )
          _item.Selected = true;
      }
    }
  }
  internal partial class Tasks
  {
    internal void CalculateWorkload()
    {
      if ( this.Task2RequirementsTitle == null )
        return;
      this.Task2RequirementsTitle.CalculateWorkload();
    }
    internal double Hours
    {
      get
      {
        if ( this.Task2MilestoneResolvedInTitle == null || this.Task2MilestoneResolvedInTitle != this.Task2RequirementsTitle.Requirements2MilestoneTitle )
          this.Task2MilestoneResolvedInTitle = this.Task2RequirementsTitle.Requirements2MilestoneTitle;
        if ( this.Task2ProjectTitle == null || this.Task2ProjectTitle != this.Task2RequirementsTitle.Requirements2ProjectsTitle )
          this.Task2ProjectTitle = this.Task2RequirementsTitle.Requirements2ProjectsTitle;
        return Workload.Sum<Workload>( a => a.Hours.GetValueOrDefault( 0 ) );
      }
    }
  }
  internal partial class Requirements
  {
    internal double Hours
    {
      get
      {
        if ( this.Requirements2ProjectsTitle == null || this.Requirements2ProjectsTitle != this.Requirements2MilestoneTitle.Milestone2ProjectTitle )
          this.Requirements2ProjectsTitle = this.Requirements2MilestoneTitle.Milestone2ProjectTitle;
        double _hours = this.Tasks.Sum<Tasks>( a => a.Hours );
        RequirementPriority = Convert.ToInt32( Math.Round( _hours ) );
        return _hours;
      }
    }
    internal void CalculateWorkload()
    {
      if ( Requirements2MilestoneTitle == null )
        return;
      Requirements2MilestoneTitle.CalculateWorkload();
    }
  }
  internal partial class Milestone
  {
    internal void CalculateWorkload()
    {
      this.MilestoneHours = this.Requirements.Sum<Requirements>( a => a.Hours );
    }
  }
}
