using System;
using Guflow.Decider;

namespace ServerlessManualApproval.Workflows
{
    /// <summary>
    /// In this example workflow execution will be paused on sending the email to users. It will expect the "Emailconfirmed" signal to received within 12 hours.
    /// If this signal does not arrive with-in 12 hours then workflow will be failed.
    /// </summary>
    [WorkflowDescription("1.1", DefaultChildPolicy = ChildPolicy.Terminate,
        DefaultExecutionStartToCloseTimeoutInSeconds = 10000, DefaultTaskListName = "manualapproval",
        DefaultTaskStartToCloseTimeoutInSeconds = 20, DefaultLambdaRole = LambdaRole.Name)]
    public class UserActivateWorkflowWithTimeout : Workflow
    {
        public UserActivateWorkflowWithTimeout()
        {

            ScheduleLambda("ConfirmEmail")
                .WithInput(_ => new { Id })
                .OnCompletion(e => e.WaitForSignal("EmailConfirmed").For(TimeSpan.FromSeconds(40)));

            ScheduleLambda("ActivateAccount").AfterLambda("ConfirmEmail")
                .When(_ => Signal("EmailConfirmed").IsTriggered());

            ScheduleAction(_ => FailWorkflow("Email not confirmed", "")).AfterLambda("ConfirmEmail")
                .When(_ => Signal("EmailConfirmed").IsTimedout());
        }
    }
}