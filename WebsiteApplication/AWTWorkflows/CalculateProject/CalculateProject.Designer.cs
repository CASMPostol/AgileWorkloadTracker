using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Reflection;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace CAS.AgileWorkloadTracker.Workflows.CountProject
{
  public sealed partial class CalculateProject
  {
    #region Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    [System.Diagnostics.DebuggerNonUserCode]
    private void InitializeComponent()
    {
      this.CanModifyActivities = true;
      System.Workflow.ComponentModel.ActivityBind activitybind1 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind3 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind6 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind8 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
      System.Workflow.ComponentModel.ActivityBind activitybind7 = new System.Workflow.ComponentModel.ActivityBind();
      this.ResultLogToHistoryList = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.CalculateProjectActivity = new System.Workflow.Activities.CodeActivity();
      this.StartCountingLogToHistoryList = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.onWorkflowActivated = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
      // 
      // ResultLogToHistoryList
      // 
      this.ResultLogToHistoryList.Duration = System.TimeSpan.Parse( "-10675199.02:48:05.4775808" );
      this.ResultLogToHistoryList.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowCompleted;
      activitybind1.Name = "CalculateProject";
      activitybind1.Path = "ResultLogToHistoryList_HistoryDescription";
      activitybind2.Name = "CalculateProject";
      activitybind2.Path = "ResultLogToHistoryList_HistoryOutcome";
      this.ResultLogToHistoryList.Name = "ResultLogToHistoryList";
      this.ResultLogToHistoryList.OtherData = "";
      activitybind3.Name = "onWorkflowActivated";
      activitybind3.Path = "WorkflowProperties.OriginatorUser.ID";
      this.ResultLogToHistoryList.SetBinding( Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind1 ) ) );
      this.ResultLogToHistoryList.SetBinding( Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind2 ) ) );
      this.ResultLogToHistoryList.SetBinding( Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind3 ) ) );
      // 
      // CalculateProjectActivity
      // 
      this.CalculateProjectActivity.Name = "CalculateProjectActivity";
      this.CalculateProjectActivity.ExecuteCode += new System.EventHandler( this.CalculateProjectActivity_ExecuteCode );
      // 
      // StartCountingLogToHistoryList
      // 
      this.StartCountingLogToHistoryList.Duration = System.TimeSpan.Parse( "-10675199.02:48:05.4775808" );
      this.StartCountingLogToHistoryList.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowStarted;
      activitybind4.Name = "CalculateProject";
      activitybind4.Path = "StartCountingLogToHistoryList_HistoryDescription";
      activitybind5.Name = "CalculateProject";
      activitybind5.Path = "StartCountingLogToHistoryList_HistoryOutcome";
      this.StartCountingLogToHistoryList.Name = "StartCountingLogToHistoryList";
      this.StartCountingLogToHistoryList.OtherData = "";
      activitybind6.Name = "onWorkflowActivated";
      activitybind6.Path = "WorkflowProperties.OriginatorUser.ID";
      this.StartCountingLogToHistoryList.SetBinding( Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind4 ) ) );
      this.StartCountingLogToHistoryList.SetBinding( Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind5 ) ) );
      this.StartCountingLogToHistoryList.SetBinding( Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind6 ) ) );
      activitybind8.Name = "CalculateProject";
      activitybind8.Path = "WorkflowId";
      // 
      // onWorkflowActivated
      // 
      correlationtoken1.Name = "CalculateProjectWorkflowToken";
      correlationtoken1.OwnerActivityName = "CalculateProject";
      this.onWorkflowActivated.CorrelationToken = correlationtoken1;
      this.onWorkflowActivated.EventName = "OnWorkflowActivated";
      this.onWorkflowActivated.Name = "onWorkflowActivated";
      activitybind7.Name = "CalculateProject";
      activitybind7.Path = "workflowProperties";
      this.onWorkflowActivated.SetBinding( Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind8 ) ) );
      this.onWorkflowActivated.SetBinding( Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind7 ) ) );
      // 
      // CalculateProject
      // 
      this.Activities.Add( this.onWorkflowActivated );
      this.Activities.Add( this.StartCountingLogToHistoryList );
      this.Activities.Add( this.CalculateProjectActivity );
      this.Activities.Add( this.ResultLogToHistoryList );
      this.Name = "CalculateProject";
      this.CanModifyActivities = false;

    }

    #endregion

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity StartCountingLogToHistoryList;

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity ResultLogToHistoryList;

    private CodeActivity CalculateProjectActivity;

    private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated onWorkflowActivated;













  }
}
