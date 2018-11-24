// /Copyright (c) Gurmit Teotia. Please see the LICENSE file in the project root folder for license information.

using System;
using Guflow.Decider;

namespace RecursionTillEternity
{
    /// <summary>
    /// Main function this workflow:
    /// 1. Recursive logic lives here. "LogProcessWorkflow" does not know anything about recursion.
    /// 2. Use custom polling strategy to read only first page (maximum 1000) history events.
    /// 3. Passes its input to ChildWorkflow
    /// 4. Immediately reschedule the child workflow if it failed,timedout or terminated.
    /// </summary>

    [WorkflowDescription("1.0", DefaultChildPolicy = ChildPolicy.Terminate,
        DefaultExecutionStartToCloseTimeoutInSeconds = 10000, DefaultTaskListName = "tasklist",
        DefaultTaskStartToCloseTimeoutInSeconds = 20)]
    public class DriverWorkflow : Workflow
    {
        public DriverWorkflow()
        {
            ScheduleChildWorkflow<LogProcessWorkflow>()
                .WithInput(WorkflowInput)
                .OnCompletion(RescheduleAfterTimeout)
                .OnTimedout(RescheduleImmediately)
                .OnTerminated(RescheduleImmediately)
                .OnFailure(RescheduleImmediately);
        }

        private string WorkflowInput(IChildWorkflowItem item)
        {
            var lastEvent = item.LastEvent() as ChildWorkflowEvent;
            return lastEvent != null ? lastEvent.Input : StartedEvent.Input;
        }

        private WorkflowAction RescheduleAfterTimeout(WorkflowItemEvent @event)
        {
            // Restart this workflow when total events in history are more than 24000. Amazon SWF allows 25000 events in workflow history.
            if (LatestEventId > 24000) return RestartWithDefaultValues(@event); 
            return Reschedule(@event).After(TimeSpan.FromHours(1));
        }

        private WorkflowAction RescheduleImmediately(WorkflowItemEvent @event)
        {
            if (LatestEventId > 24000) return RestartWithDefaultValues(@event);
            return Reschedule(@event);
        }

        private WorkflowAction RestartWithDefaultValues(WorkflowItemEvent @event)
        {
            var action = RestartWorkflow(true);
            action.Input = WorkflowInput(ChildWorkflow(@event));
            return action;
        }
    }
}