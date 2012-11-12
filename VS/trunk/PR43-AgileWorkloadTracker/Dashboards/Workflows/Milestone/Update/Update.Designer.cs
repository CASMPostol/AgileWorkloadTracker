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

namespace CAS.AgileWorkloadTracker.Dashboards.Workflows.Milestone.Update
{
  public sealed partial class Update
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
      System.Workflow.ComponentModel.ActivityBind activitybind6 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
      System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
      this.FinishLgToHistoryListActivity = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.UpdateMilestoneCodeActivity = new System.Workflow.Activities.CodeActivity();
      this.m_StartLogToHistoryListActivity = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.m_OnWorkflowActivated = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
      // 
      // FinishLgToHistoryListActivity
      // 
      this.FinishLgToHistoryListActivity.Description = "Logs message to the history log.";
      this.FinishLgToHistoryListActivity.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this.FinishLgToHistoryListActivity.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowCompleted;
      activitybind1.Name = "Update";
      activitybind1.Path = "FinishLgToHistoryListActivity_HistoryDescription";
      activitybind2.Name = "Update";
      activitybind2.Path = "FinishLgToHistoryListActivity_HistoryOutcome";
      this.FinishLgToHistoryListActivity.Name = "FinishLgToHistoryListActivity";
      this.FinishLgToHistoryListActivity.OtherData = "";
      activitybind3.Name = "Update";
      activitybind3.Path = "workflowProperties.OriginatorUser.ID";
      this.FinishLgToHistoryListActivity.MethodInvoking += new System.EventHandler(this.FinishLgToHistoryListActivity_MethodInvoking);
      this.FinishLgToHistoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
      this.FinishLgToHistoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
      this.FinishLgToHistoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
      // 
      // UpdateMilestoneCodeActivity
      // 
      this.UpdateMilestoneCodeActivity.Description = "Updates the milestone and all interconnected objects.";
      this.UpdateMilestoneCodeActivity.Name = "UpdateMilestoneCodeActivity";
      this.UpdateMilestoneCodeActivity.ExecuteCode += new System.EventHandler(this.UpdateMilestoneCodeActivity_ExecuteCode);
      // 
      // m_StartLogToHistoryListActivity
      // 
      this.m_StartLogToHistoryListActivity.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this.m_StartLogToHistoryListActivity.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowStarted;
      this.m_StartLogToHistoryListActivity.HistoryDescription = "Start updating the Milestone";
      this.m_StartLogToHistoryListActivity.HistoryOutcome = "Starting";
      this.m_StartLogToHistoryListActivity.Name = "m_StartLogToHistoryListActivity";
      this.m_StartLogToHistoryListActivity.OtherData = "";
      activitybind4.Name = "Update";
      activitybind4.Path = "workflowProperties.OriginatorUser.ID";
      this.m_StartLogToHistoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
      activitybind6.Name = "Update";
      activitybind6.Path = "workflowId";
      // 
      // m_OnWorkflowActivated
      // 
      correlationtoken1.Name = "workflowToken";
      correlationtoken1.OwnerActivityName = "Update";
      this.m_OnWorkflowActivated.CorrelationToken = correlationtoken1;
      this.m_OnWorkflowActivated.Description = "Update the list.";
      this.m_OnWorkflowActivated.EventName = "OnWorkflowActivated";
      this.m_OnWorkflowActivated.Name = "m_OnWorkflowActivated";
      activitybind5.Name = "Update";
      activitybind5.Path = "workflowProperties";
      this.m_OnWorkflowActivated.Invoked += new System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs>(this.m_OnWorkflowActivated_Invoked);
      this.m_OnWorkflowActivated.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
      this.m_OnWorkflowActivated.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
      // 
      // Update
      // 
      this.Activities.Add(this.m_OnWorkflowActivated);
      this.Activities.Add(this.m_StartLogToHistoryListActivity);
      this.Activities.Add(this.UpdateMilestoneCodeActivity);
      this.Activities.Add(this.FinishLgToHistoryListActivity);
      this.Name = "Update";
      this.CanModifyActivities = false;

    }

    #endregion

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity FinishLgToHistoryListActivity;

    private CodeActivity UpdateMilestoneCodeActivity;

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity m_StartLogToHistoryListActivity;

    private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated m_OnWorkflowActivated;
















  }
}
