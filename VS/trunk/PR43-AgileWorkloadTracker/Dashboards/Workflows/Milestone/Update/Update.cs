using System;
using System.ComponentModel;
using System.ComponentModel.Design;
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
using CAS.AgileWorkloadTracker.Linq;

namespace CAS.AgileWorkloadTracker.Dashboards.Workflows.Milestone.Update
{
  public sealed partial class Update: SequentialWorkflowActivity
  {
    public Update()
    {
      InitializeComponent();
    }

    public Guid workflowId = default( System.Guid );
    [CLSCompliantAttribute( false )]
    public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();

    private void m_OnWorkflowActivated_Invoked( object sender, ExternalDataEventArgs e ) { }
    private void UpdateMilestoneCodeActivity_ExecuteCode( object sender, EventArgs e )
    {
      using ( Entities _edc = new Entities( this.workflowProperties.SiteUrl ) )
      {
        CAS.AgileWorkloadTracker.Linq.Milestone _mlstne = Element.GetAtIndex<CAS.AgileWorkloadTracker.Linq.Milestone>( _edc.Milestone, workflowProperties.ItemId.ToString() );
        _mlstne.MilestoneHours = _mlstne.Requirements.Sum<Requirements>( a => a.Hours );
        _edc.SubmitChanges();
        _hours = _mlstne.MilestoneHours.Value;
        _tasks = _mlstne.Tasks.Count;
        _actoveTasks = _mlstne.Tasks.Where<Tasks>( tsk => tsk.Active.GetValueOrDefault( false ) ).Count<Tasks>();
      }
    }
    private void ReportException( Entities edc, string _source, Exception ex )
    {
      try
      {
        Anons _entry = new Anons() { Title = _source, Treść = ex.Message, Wygasa = DateTime.Now + new TimeSpan( 2, 0, 0, 0 ) };
        edc.Announcements.InsertOnSubmit( _entry );
        edc.SubmitChanges();
      }
      catch ( Exception ) { }
    }
    private double _hours = default( double );
    private int _tasks = default( int );
    private int _actoveTasks = default(int);
    public String FinishLgToHistoryListActivity_HistoryDescription = default(string);
    public String FinishLgToHistoryListActivity_HistoryOutcome = "Update Finished";

    private void FinishLgToHistoryListActivity_MethodInvoking( object sender, EventArgs e )
    {
      string _dscr = "The milestone has reported {0} hours and {1} tasks ({2} are active).";
      FinishLgToHistoryListActivity_HistoryDescription = String.Format(_dscr, _hours, _tasks, _actoveTasks);
    }
  }
}
