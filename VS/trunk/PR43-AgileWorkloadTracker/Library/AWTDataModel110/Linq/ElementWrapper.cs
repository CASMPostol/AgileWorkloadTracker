//<summary>
//  Title   : class ElementWrapper
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

using CAS.AgileWorkloadTracker.DataModel.Linq;
using CAS.Common.ComponentModel;
using System;
using System.ComponentModel;

namespace CAS.AgileWorkloadTracker.DataModel110.Linq
{
  /// <summary>
  /// Wrapper of the <see cref="Element"/> to be used by the user interface
  /// </summary>
  /// <typeparam name="ElementType">The type of <see cref="ElementWrapper{ElementType}"/>.</typeparam>
  public class ElementWrapper<ElementType> : PropertyChangedBase
    where ElementType : Element
  {

    #region public
    internal ElementWrapper(ElementType value)
    {
      b_Element = value;
    }
    internal ElementType Element
    {
      get
      {
        return b_Element;
      }
      set
      {
        RaiseHandler<ElementType>(value, ref b_Element, "Element", this);
      }
    }
    #endregion

    #region override  object
    /// <summary>
    /// Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
    public override string ToString()
    {
      if (Element == null)
        return m_EmptyString;
      return Element.Title;
    }
    #endregion

    #region private
    private ElementType b_Element;
    private const string m_EmptyString = " -- Select milestone -- ";
    #endregion

  }
}
