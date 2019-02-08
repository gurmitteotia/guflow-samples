using System;
using Guflow.Decider;

namespace ServerlessManualApproval.Workflows
{
    [WorkflowDescription("1.1", DefaultChildPolicy = ChildPolicy.Terminate,
        DefaultExecutionStartToCloseTimeoutInSeconds = 10000, DefaultTaskListName = "manualapproval",
        DefaultTaskStartToCloseTimeoutInSeconds = 20, DefaultLambdaRole = "provide lambda role")]
    public class UserActivateWorkflow : Workflow
    {
        public UserActivateWorkflow()
        {

            ScheduleLambda("ConfirmEmail")
                .WithInput(_ => new { Id })
                .OnCompletion(e => e.WaitForSignal("EmailConfirmed") + Jump.ToTimer("EmailConfirmedTimedout"));

            ScheduleTimer("EmailConfirmedTimedout").FireAfter(TimeSpan.FromHours(2)).When(_ => false)
                .OnFired(_ => Lambda("ConfirmEmail").IsWaitingForAnySignal() ? FailWorkflow("EmailNotConfirmed","") : Ignore);

            ScheduleLambda("ActivateAccount").AfterLambda("ConfirmEmail");
        }

        [SignalEvent]
        public WorkflowAction EmailConfirmed(WorkflowSignaledEvent @event)
            => IsSignalDelayed(@event) ? Ignore : ResumeConfirmEmail(@event.SignalName);

        private WorkflowAction ResumeConfirmEmail(string signalName) =>
            Lambda("ConfirmEmail").IsWaitingForSignal(signalName)
                ? Lambda("ConfirmEmail").Resume(signalName)
                : Ignore;

        private bool IsSignalDelayed(WorkflowSignaledEvent @event)
        {
            var timer = Timer("EmailConfirmedTimedout");
            var timerEvent = timer.LastEvent();
            if (timerEvent != null && !timerEvent.IsActive)
                return timerEvent < @event;

            return false;
        }
    }
}