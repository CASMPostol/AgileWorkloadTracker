using System;

namespace CAS.AgileWorkloadTracker.Dashboards
{
  public static class Extensions
  {
    /// <summary>
    ///  Indicates whether the specified System.String object is null or an System.String.Empty string.
    /// </summary>
    /// <param name="_val"> A System.String reference.</param>
    /// <returns>
    ///   true if the value parameter is null or an empty string (""); otherwise, false.</c>.
    /// </returns>
    public static bool IsNullOrEmpty( this string value )
    {
      return String.IsNullOrEmpty( value );
    }
    /// <summary>
    /// String2s the int.
    /// </summary>
    /// <param name="value">The _val.</param>
    /// <returns></returns>
    public static int? String2Int( this string value )
    {
      int _ret;
      if ( int.TryParse( value, out _ret ) )
      {
        return _ret;
      }
      return new Nullable<int>();
    }
  }
}
