//<summary>
//  Title   : SharePoint Entity Element
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

using CAS.SharePoint.Linq;

namespace CAS.AgileWorkloadTracker.DataModel.Linq
{
  /// <summary>
  /// SharePoint Entity Element
  /// </summary>
  partial class Item : IItem
  {
    ///// <summary>
    ///// The ID colunm name
    ///// </summary>
    //public const string IDColunmName = "ID";
    ///// <summary>
    ///// The title colunm name
    ///// </summary>
    //public const string TitleColunmName = "Title";
    ///// <summary>
    ///// Try to get at index.
    ///// </summary>
    ///// <typeparam name="t"></typeparam>
    ///// <param name="_list">The _list.</param>
    ///// <param name="_ID">The _ ID.</param>
    ///// <returns>
    ///// An instance of the <typeparamref name="t"/> for the selected index or null if <paramref name="_ID" /> is null or empty.
    ///// </returns>
    ///// <exception cref="ApplicationException">Element cannot be found.</exception>
    //public static t TryGetAtIndex<t>( EntityList<t> _list, string _ID )
    //  where t: Item
    //{
    //  if ( _ID.IsNullOrEmpty() )
    //    return null;
    //  return GetAtIndex<t>( _list, _ID );
    //}
    ///// <summary>
    ///// Gets at index.
    ///// </summary>
    ///// <typeparam name="t"></typeparam>
    ///// <param name="_list">The _list.</param>
    ///// <param name="_ID">The _ ID.</param>
    ///// <returns>
    ///// An instance of the <typeparamref name="t" /> for the selected index.
    ///// </returns>
    ///// <exception cref="System.ApplicationException">
    ///// </exception>
    ///// <exception cref="ApplicationException">_ID is nuul or element cannot be found.</exception>
    //public static t GetAtIndex<t>( EntityList<t> _list, string _ID )
    //  where t: Item
    //{
    //  int? _index = _ID.String2Int();
    //  if ( !_index.HasValue )
    //    throw new ApplicationException( typeof( t ).Name + " index is null" );
    //  try
    //  {
    //    return GetAtIndex<t>( _list, _index.Value );
    //  }
    //  catch ( Exception )
    //  {
    //    throw new ApplicationException( String.Format( "{0} cannot be found at specified index{1}", typeof( t ).Name, _index.Value ) );
    //  }
    //}
    ///// <summary>
    ///// Gets an entitz at index.
    ///// </summary>
    ///// <typeparam name="t"></typeparam>
    ///// <param name="_list">The list.</param>
    ///// <param name="_index">The index of the item to be returned.</param>
    ///// <returns>The list item of the <typeparamref name="t"/> type</returns>
    //public static t GetAtIndex<t>( EntityList<t> _list, int _index ) where t: Item
    //{
    //  return ( from idx in _list where idx.Id == _index select idx ).First();
    //}
    //internal static t FindAtIndex<t>( EntityList<t> _list, string _ID )
    //  where t: Item
    //{
    //  int? _index = _ID.String2Int();
    //  if ( !_index.HasValue )
    //    return null;
    //  try
    //  {
    //    return (
    //          from idx in _list
    //          where idx.Id == _index.Value
    //          select idx ).FirstOrDefault();
    //  }
    //  catch ( Exception )
    //  {
    //    return null;
    //  }
    //}

  }
}
