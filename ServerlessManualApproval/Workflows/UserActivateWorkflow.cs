using Guflow.Decider;

namespace ServerlessManualApproval.Workflows
{
    /// <summary>
    /// In this example workflow execution will be paused indefinitely on sending the email to users.
    /// </summary>
    [WorkflowDescription("1.1", DefaultChildPolicy = ChildPolicy.Terminate,
        DefaultExecutionStartToCloseTimeoutInSeconds = 10000, DefaultTaskListName = "manualapproval",
        DefaultTaskStartToCloseTimeoutInSeconds = 20, DefaultLambdaRole = "provide lambda role")]
    public class UserActivateWorkflow : Workflow
    {
        public UserActivateWorkflow()
        {

            ScheduleLambda("ConfirmEmail")
                .WithInput(_ => new { Id })
                .OnCompletion(e => e.WaitForSignal("EmailConfirmed"));

            ScheduleLambda("ActivateAccount").AfterLambda("ConfirmEmail");
        }
    }
}