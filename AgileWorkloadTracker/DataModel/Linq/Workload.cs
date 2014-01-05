﻿//<summary>
//  Title   : Entity partial class Workload
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
using CAS.SharePoint;

namespace CAS.AgileWorkloadTracker.DataModel.Linq
{
  /// <summary>
  /// Entity partial class Workload
  /// </summary>
  public partial class Workload
  {
    internal void Update(ref double hours, ref DateTime start, ref DateTime end)
    {
      MakeConsistent();
      DataModelExtensions.UpdateWorkload(ref hours, ref start, ref end, MyHours, MyDate, MyDate);
    }
    internal void MakeConsistent()
    {
      if (this.Workload2TaskTitle == null)
        throw new ArgumentNullException("Workload2TaskTitle", this.Title);
      if (this.Workload2ProjectTitle != this.Workload2TaskTitle.Task2ProjectTitle)
        this.Workload2ProjectTitle = this.Workload2TaskTitle.Task2ProjectTitle;
      if (this.Workload2StageTitle == null)
        this.Workload2StageTitle = this.Workload2ProjectTitle.Project2StageTitle;
    }
    private DateTime MyDate
    {
      get { return this.WorkloadDate.ValueOrException<DateTime>("DataModel.Linq.Workload", "Start", "WorkloadDate"); }
    }
    private double MyHours
    {
      get { return this.Hours.ValueOrException<double>("DataModel.Linq.Workload", "MyHours", "Hours"); }
    }
    private string WrongProjectMessage()
    {
      return String.Format("Workload {0} lookup on task {1} of the project has wrong project lookup {3}", Title, Workload2TaskTitle.Title, Workload2TaskTitle.Task2ProjectTitle.Title, Workload2ProjectTitle.Title);
    }
  }
}
