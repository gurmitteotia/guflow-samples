// /Copyright (c) Gurmit Teotia. Please see the LICENSE file in the project root folder for license information.

using Guflow.Decider;

namespace ServerlessManualApproval.Workflows
{
    [WorkflowDescription("1.1", DefaultChildPolicy = ChildPolicy.Terminate,
        DefaultExecutionStartToCloseTimeoutInSeconds = 10000, DefaultTaskListName = "manualapproval",
        DefaultTaskStartToCloseTimeoutInSeconds = 20, DefaultLambdaRole = LambdaRole.Name)]
    public class ExpenseWorkflow : Workflow
    {
        public ExpenseWorkflow()
        {
            ScheduleLambda("ApproveExpenses")
                .WithInput(_=>new {Id})
                .OnCompletion(e => e.WaitForAnySignal("Accepted", "Rejected"));

            ScheduleLambda("SendToAccount").AfterLambda("ApproveExpenses").When(_ => Signal("Accepted").IsTriggered());
            ScheduleLambda("SendBackToEmp").AfterLambda("ApproveExpenses").When(_ => Signal("Rejected").IsTriggered());
        }
    }
}