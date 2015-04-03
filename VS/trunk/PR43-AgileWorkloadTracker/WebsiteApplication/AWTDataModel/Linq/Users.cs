//_______________________________________________________________
//  Title   : Users
//  System  : Microsoft VisualStudio 2013 / C#
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2015, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________

using Microsoft.SharePoint;
using Microsoft.SharePoint.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CAS.AgileWorkloadTracker.DataModel.Linq
{
  public partial class Users
  {
    public static Users FindSPUser(Entities edc, SPUser _user)
    {
      if (edc == null)
        throw new ArgumentNullException("FindForUser requires the edc parameter to be assigned");
      EntityList<Item> _res = edc.UserInformationList;
      if (_res == null || _res.Count() == 0)
        return null;
      Item _ret = (from idx in _res
                   let _account = idx.Title
                   where _account.Contains(_user.Name)
                   select idx
                  ).FirstOrDefault();
      return _ret == null ? null : (Users)_ret;
    }
    public IEnumerable<Workload> Workload(Entities edc)
    {
      return from idx in edc.Workload
             where idx.AssignedTo.Id == this.Id
             select idx
      ;
    }
    public IQueryable<Estimation> Estimation(Entities edc)
    {
      return from idx in edc.Estimation
             where idx.AssignedTo.Id == this.Id
             select idx
      ;
    }
  }
}
