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

using System;
using System.ComponentModel;
using CAS.AgileWorkloadTracker.DataModel.Linq;
using CAS.Common.ComponentModel;

namespace CAS.AgileWorkloadTracker.DataModel110.Linq
{
  /// <summary>
  /// Wrapper of the <see cref="Element"/> to be used by the user interface
  /// </summary>
  /// <typeparam name="ElementType">The type of <see cref="ElementWrapper{ElementType}"/>.</typeparam>
  public class ElementWrapper<ElementType> : INotifyPropertyChanged
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

    #region INotifyPropertyChanged Members
    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;
    #endregion

    #region private
    private ElementType b_Element;
    private const string m_EmptyString = " -- Select milestone -- ";
    /// <summary>
    /// Extension method that sets a new value in a variable and then executes the event handler if the new value
    /// differs from the old one.  Used to easily implement INotifyPropertyChanged.
    /// </summary>
    /// <typeparam name="T">The type of values being handled (usually the type of the property).</typeparam>
    /// <param name="newValue">The new value to set.</param>
    /// <param name="oldValue">The old value to replace (and the value holder).</param>
    /// <param name="propertyName">The property's name as required by <see cref="System.ComponentModel.PropertyChangedEventArgs"/>.</param>
    /// <param name="sender">The object to be appointed as the executioner of the handler.</param>
    /// <returns>
    /// A boolean value that indicates if the new value was truly different from the old value according to <code>object.Equals()</code>.
    /// </returns>
    protected bool RaiseHandler<T>(T newValue, ref T oldValue, string propertyName, object sender)
    {
      bool changed = !Object.Equals(oldValue, newValue);
      if (changed)
      {
        //Save the new value. 
        oldValue = newValue;
        //Raise the event 
        if (PropertyChanged != null)
        {
          PropertyChanged(sender, new PropertyChangedEventArgs(propertyName));
        }
      }
      //Signal what happened. 
      return changed;
    }

    #endregion

  }
}
