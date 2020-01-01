// /Copyright (c) Gurmit Teotia. Please see the LICENSE file in the project root folder for license information.

using Guflow.Decider;

namespace ServerlessManualApproval.Workflows
{
    /// <summary>
    /// This workflow start three parallel branches and wait for the signals in each of them. When everyone approve the permit application permit is approved otherwise it is rejected.
    /// </summary>
    [WorkflowDescription("1.1", DefaultChildPolicy = ChildPolicy.Terminate,
        DefaultExecutionStartToCloseTimeoutInSeconds = 10000, DefaultTaskListName = "manualapproval",
        DefaultTaskStartToCloseTimeoutInSeconds = 20, DefaultLambdaRole = LambdaRole.Name)]

    public class PermitIssueWorkflow : Workflow
    {
        public PermitIssueWorkflow()
        {
            ScheduleLambda("ApplyToCouncil").WithInput(_ => new {Id})
                .OnCompletion(e => e.WaitForAnySignal("CApproved", "CRejected"));

            ScheduleLambda("ApplyToFireDept").WithInput(_ => new { Id })
                .OnCompletion(e => e.WaitForAnySignal("FApproved", "FRejected"));

            ScheduleLambda("ApplyToForestDept").WithInput(_ => new { Id })
                .OnCompletion(e => e.WaitForAnySignal("FrApproved", "FrRejected"));

            ScheduleLambda("IssuePermit").AfterLambda("ApplyToCouncil").AfterLambda("ApplyToFireDept")
                .AfterLambda("ApplyToForestDept")
                .When(EveryOneAgree);

            ScheduleLambda("RejectPermit").AfterLambda("ApplyToCouncil").AfterLambda("ApplyToFireDept")
                .AfterLambda("ApplyToForestDept")
                .When(AnyOneDisagree);

        }

        private bool AnyOneDisagree(ILambdaItem arg)
        {
            return arg.ParentLambda("ApplyToCouncil").IsSignalled("CRejected")
                   || arg.ParentLambda("ApplyToFireDept").IsSignalled("FRejected")
                   || arg.ParentLambda("ApplyToForestDept").IsSignalled("FrRejected");
        }

        private bool EveryOneAgree(ILambdaItem arg)
        {
            return arg.ParentLambda("ApplyToCouncil").IsSignalled("CApproved")
                   && arg.ParentLambda("ApplyToFireDept").IsSignalled("FApproved")
                   && arg.ParentLambda("ApplyToForestDept").IsSignalled("FrApproved");
        }
    }
}