using System;
using System.ComponentModel;
using System.ComponentModel.Design;
//<summary>
//  Title   : Calculate Project Sequential Workflow Activity
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL:$
//  $Id$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.WorkflowActions;
using CAS.AgileWorkloadTracker.DataModel.Linq;

namespace CAS.AgileWorkloadTracker.Workflows.CountProject
{
  /// <summary>
  /// Calculate Project Sequential Workflow Activity
  /// </summary>
  public sealed partial class CalculateProject: SequentialWorkflowActivity
  {
    public CalculateProject()
    {
      InitializeComponent();
    }
    public Guid WorkflowId = new Guid( "98EA7E72-122C-4B8C-8359-2335B4992800" );
    public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();

    private void CalculateProjectActivity_ExecuteCode( object sender, EventArgs e )
    {
      using ( Entities _edc = new Entities( this.workflowProperties.Web.Url ) )
      {
        Projects _project = Element.GetAtIndex<Projects>( _edc.Projects, workflowProperties.ItemId );
        _project.Adjust( _edc );
        _edc.SubmitChanges();
      }
    }
    public String StartCountingLogToHistoryList_HistoryDescription = "Start calculate the Project";
    public String StartCountingLogToHistoryList_HistoryOutcome = "StartedCounting";
    public String ResultLogToHistoryList_HistoryDescription = default( System.String );
    public String ResultLogToHistoryList_HistoryOutcome = "FinishedCounting";
  }
}
