//<summary>
//  Title   : class ElementWrapper
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate:$
//  $Rev:$
//  $LastChangedBy:$
//  $URL:$
//  $Id:$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAS.AgileWorkloadTracker.DataModel.Linq;

namespace CAS.AgileWorkloadTracker.SiteManagement.Linq
{
  /// <summary>
  /// Wrapper of the <see cref="Element"/> to be used by the user interface
  /// </summary>
  /// <typeparam name="ElementType">The type of the lement type.</typeparam>
  internal class ElementWrapper<ElementType> : INotifyPropertyChanged
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
        PropertyChanged.RaiseHandler<ElementType>(value, ref b_Element, "Element", this);
      }
    }
    #endregion

    #region override  object
    public override string ToString()
    {
      return Element.Title;
    }

    #endregion
    #region INotifyPropertyChanged Members
    public event PropertyChangedEventHandler PropertyChanged;
    #endregion

    #region private
    private ElementType b_Element;
    #endregion

  }
}
