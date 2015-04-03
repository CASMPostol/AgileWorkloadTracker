//_______________________________________________________________
//  Title   : UserDescriptor
//  System  : Microsoft VisualStudio 2013 / C#
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL: $
//  $Id$
//
//  Copyright (C) 2015, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________

using CAS.AgileWorkloadTracker.DataModel.Linq;
using CAS.SharePoint.Linq;
using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Data;
using System.Web.UI.WebControls.WebParts;

namespace CAS.AgileWorkloadTracker.Dashboards.Webparts.CurrentUserWebPart
{
  public class UserDescriptor : DataTable, IWebPartRow, IUserDescriptor
  {
    #region IWebPartRow
    public PropertyDescriptorCollection Schema { get; private set; }
    public void GetRowData(RowCallback callback)
    {
      callback(this.Row0);
    }
    #endregion

    #region public
    internal UserDescriptor(SPUser _user)
      : this()
    {
      DataRow row = this.NewRow();
      row[_EmailColumnName] = _user.Email;
      row[_IDColumnName] = _user.ID;
      row[_LoginNameColumnName] = _user.LoginName;
      row[_NameColumnName] = this.User = _user.Name;
      using (Entities edc = new Entities(SPContext.Current.Web.Url))
      {
        Item _Partner = Users.FindSPUser(edc, _user);
        if (_Partner != null)
        {
          row[_CompanyIDColumnName] = _Partner.Id.ToString();
          row[_CompanyTitleColumnName] = Company = _Partner.Title;
        }
        else
        {
          row[_CompanyIDColumnName] = String.Empty;
          row[_CompanyTitleColumnName] = String.Empty;
        }
      }
      this.Rows.Add(row);
      Schema = TypeDescriptor.GetProperties(this.Row0);
    }
    internal UserDescriptor(DataRowView _drw)
      : this()
    {
      DataRow row = this.NewRow();
      row[_EmailColumnName] = _drw.Row[_EmailColumnName];
      row[_IDColumnName] = _drw.Row[_IDColumnName];
      row[_LoginNameColumnName] = _drw.Row[_LoginNameColumnName];
      row[_NameColumnName] = this.User = (string)_drw.Row[_NameColumnName];
      row[_CompanyIDColumnName] = _drw.Row[_CompanyIDColumnName];
      row[_CompanyTitleColumnName] = Company = (string)_drw.Row[_CompanyTitleColumnName];
      this.Rows.Add(row);
      Schema = TypeDescriptor.GetProperties(this.Row0);
    }

    #region IUserDescriptor
    public string User { get; private set; }
    public string Company { get; private set; }
    #endregion

    #endregion

    #region private

    #region ctors
    private UserDescriptor()
      : base(m_TableName)
    {
      AddColumn(_EmailColumnName);
      AddColumn(_IDColumnName);
      AddColumn(_LoginNameColumnName);
      AddColumn(_NameColumnName);
      AddColumn(_NotesColumnName);
      AddColumn(_CompanyIDColumnName);
      AddColumn(_CompanyTitleColumnName);
    }
    #endregion

    private const string m_TableName = "User Descriptor";
    private const string _EmailColumnName = "Email";
    private const string _IDColumnName = "MemberID";
    private const string _LoginNameColumnName = "LoginName";
    private const string _NameColumnName = "Name";
    private const string _NotesColumnName = "Notes";
    private const string _CompanyTitleColumnName = SPLinqExtensions.TitleColumnName;
    private const string _CompanyIDColumnName = SPLinqExtensions.IDColumnName;
    private DataRowView Row0 { get { return DefaultView[0]; } }
    private void AddColumn(string _name)
    {
      DataColumn col = new DataColumn();
      col.DataType = typeof(string);
      col.ColumnName = _name;
      this.Columns.Add(col);
    }
    #endregion
  }
}
