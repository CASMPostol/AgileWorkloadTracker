using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Linq;
using System.Web.UI;

namespace CAS.SharePoint.Linq
{
  public class DataContextManagement<type> : IDisposable
    where type : DataContext, new()
  {
    public DataContextManagement(UserControl _parent)
    {
      _parent.PreRender += new EventHandler(_parent_PreRender);
    }
    public type DataContext
    {
      get
      {
        if (disposed)
          throw new ObjectDisposedException("The object DataContext has been already disposed.");
        if (_DataContext == null)
          _DataContext = new type();
        return _DataContext;
      }
    }
    private void _parent_PreRender(object sender, EventArgs e)
    {
      Dispose();
    }
    #region IDisposable
    // Other managed resource this class uses.
    private type _DataContext = null;
    // Track whether Dispose has been called.
    private bool disposed = false;
    // Implement IDisposable.
    // Do not make this method virtual.
    // A derived class should not be able to override this method.
    public void Dispose()
    {
      Dispose(true);
      // This object will be cleaned up by the Dispose method.
      // Therefore, you should call GC.SupressFinalize to
      // take this object off the finalization queue
      // and prevent finalization code for this object
      // from executing a second time.
      GC.SuppressFinalize(this);
    }
    // Dispose(bool disposing) executes in two distinct scenarios.
    // If disposing equals true, the method has been called directly
    // or indirectly by a user's code. Managed and unmanaged resources
    // can be disposed.
    // If disposing equals false, the method has been called by the
    // runtime from inside the finalizer and you should not reference
    // other objects. Only unmanaged resources can be disposed.
    private void Dispose(bool disposing)
    {
      // Check to see if Dispose has already been called.
      if (!this.disposed)
      {
        // If disposing equals true, dispose all managed
        // and unmanaged resources.
        if (disposing && _DataContext != null)
        {
          // Dispose managed resources.
          _DataContext.Dispose();
        }
        // Call the appropriate methods to clean up unmanaged resources here.
        // If disposing is false, only the following code is executed.
        // Note disposing has been done.
        disposed = true;
      }
    }
    /// <summary>
    /// Releases unmanaged resources and performs other cleanup operations before the
    /// <see cref="DataContextManagement&lt;type&gt;"/> is reclaimed by garbage collection.
    /// </summary>
    /// <remarks>Use C# destructor syntax for finalization code. This destructor will run only if the Dispose method does not get called. It gives your base class the opportunity to finalize. 
    /// Do not provide destructors in types derived from this class.
    /// </remarks>
    ~DataContextManagement()
    {
      // Do not re-create Dispose clean-up code here.
      // Calling Dispose(false) is optimal in terms of
      // readability and maintainability.
      Dispose(false);
    }
    #endregion
  }
}
