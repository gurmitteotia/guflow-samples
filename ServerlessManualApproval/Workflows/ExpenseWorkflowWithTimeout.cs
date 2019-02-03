using System;
using Guflow.Decider;

namespace ServerlessManualApproval.Workflows
{
    /// <summary>
    /// In this example workflow will wait only for given period the signals. If it does not receive the signals with-in wait period then it will
    /// escalate the expenses.
    /// This workflow will be simplified greatly after completion of the feature- https://github.com/gurmitteotia/guflow/issues/32
    /// </summary>
    [WorkflowDescription("1.1", DefaultChildPolicy = ChildPolicy.Terminate,
        DefaultExecutionStartToCloseTimeoutInSeconds = 10000, DefaultTaskListName = "manualapproval",
        DefaultTaskStartToCloseTimeoutInSeconds = 20, DefaultLambdaRole = "provide lambda role")]

    public class ExpenseWorkflowWithTimeout : Workflow
    {
        public ExpenseWorkflowWithTimeout()
        {
            ScheduleLambda("ApproveExpenses")
                .WithInput(_ => new { Id })
                .OnCompletion(e => e.WaitForAnySignal("Accepted", "Rejected")+ Jump.ToTimer("ApproveExpensesTimeout"));

            ScheduleTimer("ApproveExpensesTimeout").FireAfter(TimeSpan.FromDays(2)).When(_ => false)
                .OnFired(_ =>Lambda("ApproveExpenses").IsWaitingForAnySignal() ? (WorkflowAction) Jump.ToLambda("EscalateExpenses") : Ignore);

            ScheduleLambda("SendToAccount").AfterLambda("ApproveExpenses").When(_ => Signal("Accepted").IsTriggered());
            ScheduleLambda("SendBackToEmp").AfterLambda("ApproveExpenses").When(_ => Signal("Rejected").IsTriggered());

            ScheduleLambda("EscalateExpenses").AfterLambda("ApproveExpense").When(_ => false);
        }

        [SignalEvent]
        public WorkflowAction Accepted(WorkflowSignaledEvent @event)
        {
            if (IsSignalDelayed(@event)) return Ignore;
            return ResumeApproveExpenses(@event.SignalName);
        }

        [SignalEvent]
        public WorkflowAction Rejected(WorkflowSignaledEvent @event)
        {
            if (IsSignalDelayed(@event)) return Ignore;
            return ResumeApproveExpenses(@event.SignalName);
        }

        private WorkflowAction ResumeApproveExpenses(string signalName)=>
            Lambda("ApproveExpense").IsWaitingForSignal(signalName)
                ? Lambda("ApproveExpense").Resume(signalName)
                : Ignore;

        private bool IsSignalDelayed(WorkflowSignaledEvent @event)
        {
            var timer = Timer("ApproveExpensesTimeout");
            var timerEvent = timer.LastEvent();
            if (timerEvent != null && !timerEvent.IsActive)
                return timerEvent < @event;

            return false;
        }
    }
}