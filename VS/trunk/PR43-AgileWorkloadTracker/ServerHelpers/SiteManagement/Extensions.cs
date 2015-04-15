//<summary>
//  Title   : Extensions
//  System  : Microsoft VisualStudio 2013 / C#
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>
      
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAS.SmartFactory.Shepherd.Client.Management
{
  internal static class Extensions
  {
    /// <summary>
    /// Convert the <see cref="Nullable{DateTime}"/> to localized string.
    /// </summary>
    /// <param name="value">The value to be converted.</param>
    /// <returns>Localized <see cref="System.String"/>.</returns>
    internal static string LocalizedString(this Nullable<DateTime> value)
    {
      return value.HasValue ? value.Value.ToString("G", System.Globalization.CultureInfo.CurrentCulture) : Properties.Settings.Default.RunDateUnknown;
    }
    /// <summary>
    /// Gets the value or default.
    /// </summary>
    /// <param name="value">The value if not bull or empty.</param>
    /// <param name="defaultString">The default string returned if <paramref name="value"/> is null or default.</param>
    internal static string GetValueOrDefault(this string value, string defaultString)
    {
      return string.IsNullOrEmpty(value) ? defaultString : value;
    }

  }
}
