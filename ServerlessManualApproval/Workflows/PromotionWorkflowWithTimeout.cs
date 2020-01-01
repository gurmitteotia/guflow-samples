using System;
using Guflow.Decider;

namespace ServerlessManualApproval.Workflows
{
    [WorkflowDescription("1.1", DefaultChildPolicy = ChildPolicy.Terminate,
        DefaultExecutionStartToCloseTimeoutInSeconds = 10000, DefaultTaskListName = "manualapproval",
        DefaultTaskStartToCloseTimeoutInSeconds = 20, DefaultLambdaRole = LambdaRole.Name)]

    public class PromotionWorkflowWithTimeout : Workflow
    {
        public PromotionWorkflowWithTimeout()
        {
            ScheduleLambda("PromoteEmployee").WithInput(_ => new { Id })
                .OnCompletion(e => e.WaitForAllSignals("HRApproved", "ManagerApproved").For(TimeSpan.FromSeconds(40)));

            ScheduleLambda("Promoted").AfterLambda("PromoteEmployee")
                .When(_ => AnySignal("HRApproved", "ManagerApproved").IsTriggered());

            ScheduleAction(_ => FailWorkflow("Promotion timedout","")).AfterLambda("PromoteEmployee")
                .When(_ => AnySignal("HRApproved", "ManagerApproved").IsTimedout());

            ScheduleLambda("SendForReviewToHr").AfterLambda("Promoted");
        }
    }
}