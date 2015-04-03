//<summary>
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
    public void Update(ref double hours, ref DateTime start, ref DateTime end)
    {
      MakeConsistent();
      DataModelExtensions.UpdateWorkload(ref hours, ref start, ref end, MyHours, MyDate, MyDate);
    }
    public void MakeConsistent()
    {
      if (this.Workload2TaskID == null)
        throw new ArgumentNullException("Workload2TaskTitle", this.Title);
      if (this.Workload2ProjectTitle != this.Workload2TaskID.Task2ProjectTitle)
        this.Workload2ProjectTitle = this.Workload2TaskID.Task2ProjectTitle;
      if (this.Workload2StageTitle == null)
        this.Workload2StageTitle = this.Workload2ProjectTitle.Project2StageTitle;
    }
    public DateTime MyDate
    {
      get { return this.StartDate.ValueOrException<DateTime>("DataModel.Linq.Workload", "Start", "WorkloadDate").Date; }
    }
    public double MyHours
    {
      get { return (this.EndDate.Value - this.StartDate.Value).Duration().TotalMinutes / 60.0; }
    }
    private string WrongProjectMessage()
    {
      return String.Format("Workload {0} lookup on task {1} of the project has wrong project lookup {3}", Title, Workload2TaskID.Title, Workload2TaskID.Task2ProjectTitle.Title, Workload2ProjectTitle.Title);
    }
  }
}
