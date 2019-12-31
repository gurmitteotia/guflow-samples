using System;
using Guflow.Decider;

namespace ServerlessManualApproval.Workflows
{
    /// <summary>
    /// This workflow start three parallel branches and wait for the signals for specific period in each branch. When everyone approve the permit application permit is approved otherwise it is rejected.
    /// Workflow is failed when wait for any signal is timedout.
    /// </summary>
    [WorkflowDescription("1.1", DefaultChildPolicy = ChildPolicy.Terminate,
        DefaultExecutionStartToCloseTimeoutInSeconds = 10000, DefaultTaskListName = "manualapproval",
        DefaultTaskStartToCloseTimeoutInSeconds = 20, DefaultLambdaRole = "lambda role")]

    public class PermitIssueWorkflowWithTimeout : Workflow
    {
        public PermitIssueWorkflowWithTimeout()
        {
            ScheduleLambda("ApplyToCouncil").WithInput(_ => new { Id })
                .OnCompletion(e => e.WaitForAnySignal("CApproved", "CRejected").For(TimeSpan.FromDays(2)));

            ScheduleLambda("ApplyToFireDept").WithInput(_ => new { Id })
                .OnCompletion(e => e.WaitForAnySignal("FApproved", "FRejected").For(TimeSpan.FromDays(3)));

            ScheduleLambda("ApplyToForestDept").WithInput(_ => new { Id })
                .OnCompletion(e => e.WaitForAnySignal("FrApproved", "FrRejected").For(TimeSpan.FromDays(4)));

            ScheduleLambda("IssuePermit").AfterLambda("ApplyToCouncil").AfterLambda("ApplyToFireDept")
                .AfterLambda("ApplyToForestDept")
                .When(EveryOneAgree);

            ScheduleLambda("RejectPermit").AfterLambda("ApplyToCouncil").AfterLambda("ApplyToFireDept")
                .AfterLambda("ApplyToForestDept")
                .When(AnyOneDisagree);

            ScheduleAction(_ => FailWorkflow("Permit Timedout","")).AfterLambda("ApplyToCouncil")
                .AfterLambda("ApplyToFireDept")
                .AfterLambda("ApplyToForestDept")
                .When(AnyOneTimedout);

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

        private bool AnyOneTimedout(IWorkflowItem item)
        {
            return item.ParentLambda("ApplyToCouncil").IsSignalTimedout("CRejected")
                   || item.ParentLambda("ApplyToFireDept").IsSignalTimedout("FRejected")
                   || item.ParentLambda("ApplyToForestDept").IsSignalTimedout("FrRejected");
        }
    }
}