using System;
using Guflow.Decider;

namespace ServerlessManualApproval.Workflows
{
    /// <summary>
    /// In this example workflow will wait for the 2 days for the signals. If it does not receive the signals within 2 days period then it will
    /// escalate the expenses.
    /// </summary>
    [WorkflowDescription("1.1", DefaultChildPolicy = ChildPolicy.Terminate,
        DefaultExecutionStartToCloseTimeoutInSeconds = 10000, DefaultTaskListName = "manualapproval",
        DefaultTaskStartToCloseTimeoutInSeconds = 20, DefaultLambdaRole = LambdaRole.Name)]

    public class ExpenseWorkflowWithTimeout : Workflow
    {
        public ExpenseWorkflowWithTimeout()
        {
            ScheduleLambda("ApproveExpenses")
                .WithInput(_ => new { Id })
                .OnCompletion(e => e.WaitForAnySignal("Accepted", "Rejected").For(TimeSpan.FromSeconds(40)));
            
            ScheduleLambda("SendToAccount").AfterLambda("ApproveExpenses").When(_ => Signal("Accepted").IsTriggered());
            ScheduleLambda("SendBackToEmp").AfterLambda("ApproveExpenses").When(_ => Signal("Rejected").IsTriggered());

            ScheduleLambda("EscalateExpenses").AfterLambda("ApproveExpenses")
                .When(_ => AnySignal("Accepted", "Rejected").IsTimedout());
        }
        
    }
}