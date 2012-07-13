using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.ITRDataAccess
{
  public static class Extensions
  {
    public static string SPValidSubstring( this string _value )
    {
      string _normalizedString = Microsoft.SharePoint.Utilities.SPStringUtility.RemoveControlChars( _value );
      int _gdl = _normalizedString.Length;
      do
      {
        _normalizedString = _normalizedString.Replace( "  ", " " );
        if ( _gdl == _normalizedString.Length )
          break;
        _gdl = _normalizedString.Length;
      } while ( true );
      int SPStringMAxLength = 250;
      if ( _gdl >= SPStringMAxLength )
        _normalizedString = _normalizedString.Remove( SPStringMAxLength );
      return _normalizedString;
    }
  }
}
