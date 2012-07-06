using System;
using System.Linq;
using CAS.AgileWorkloadTracker.Dashboards;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Linq;

namespace CAS.AgileWorkloadTracker.Linq
{
  internal partial class Entities
  {
    public Entities() : base(SPContext.Current.Web.Url) { }
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
    internal static t TryGetAtIndex<t>(EntityList<t> _list, string _ID)
      where t : Element
    {
      if (_ID.IsNullOrEmpty())
        return null;
      return GetAtIndex<t>(_list, _ID);
    }
    /// <summary>
    /// Gets at index.
    /// </summary>
    /// <typeparam name="t"></typeparam>
    /// <param name="_list">The _list.</param>
    /// <param name="_ID">The _ ID.</param>
    /// <exception cref="ApplicationException">_ID is nuul or element cannot be found.</exception>
    /// <returns>An instance of the <see cref="t"/> for the selected index.</returns>
    internal static t GetAtIndex<t>(EntityList<t> _list, string _ID)
      where t : Element
    {
      int? _index = _ID.String2Int();
      if (!_index.HasValue)
        throw new ApplicationException(typeof(t).Name + " index is null"); ;
      try
      {
        return GetAtIndex<t>(_list, _index.Value);
      }
      catch (Exception)
      {
        throw new ApplicationException(String.Format("{0} cannot be found at specified index{1}", typeof(t).Name, _index.Value));
      }
    }
    /// <summary>
    /// Gets an entitz at index.
    /// </summary>
    /// <typeparam name="t"></typeparam>
    /// <param name="_list">The list.</param>
    /// <param name="_index">The index of the item to be returned.</param>
    /// <returns>The list item of the <typeparamref name="t"/> type</returns>
    internal static t GetAtIndex<t>(EntityList<t> _list, int _index) where t : Element
    {
      return (from idx in _list where idx.Identyfikator == _index select idx).First();
    }
    internal static t FindAtIndex<t>(EntityList<t> _list, string _ID)
      where t : Element
    {
      int? _index = _ID.String2Int();
      if (!_index.HasValue)
        return null;
      try
      {
        return (
              from idx in _list
              where idx.Identyfikator == _index.Value
              select idx).FirstOrDefault();
      }
      catch (Exception)
      {
        return null;
      }
    }
  }
  internal partial class Resources
  {
    internal static Resources FindForUser(Entities edc, SPUser _user)
    {
      EntityList<CAS.AgileWorkloadTracker.Linq.Resources> _res = edc.Resources;
      if (_res == null || _res.Count() == 0)
        return null;
      return (
                from idx in _res
                let _account = idx.EmployeeADAccount
                where _account.Tytuł.Contains(_user.Name)
                select idx
              ).FirstOrDefault();
    }
  }
}
