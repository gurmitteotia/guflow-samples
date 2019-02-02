// /Copyright (c) Gurmit Teotia. Please see the LICENSE file in the project root folder for license information.

using Guflow.Decider;

namespace ServerlessManualApproval.Workflows
{
    [WorkflowDescription("1.1", DefaultChildPolicy = ChildPolicy.Terminate,
        DefaultExecutionStartToCloseTimeoutInSeconds = 10000, DefaultTaskListName = "manualapproval",
        DefaultTaskStartToCloseTimeoutInSeconds = 20, DefaultLambdaRole = "provide lambda role")]

    public class PromotionWorkflow : Workflow
    {
        public PromotionWorkflow()
        {
            ScheduleLambda("PromoteEmployee").WithInput(_=>new{Id})
                .OnCompletion(e => e.WaitForAllSignals("HRApproved", "ManagerApproved"));
            ScheduleLambda("Promoted").AfterLambda("PromoteEmployee");
            ScheduleLambda("SendForReviewToHr").AfterLambda("Promoted");
        }
    }
}