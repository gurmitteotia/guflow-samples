// /Copyright (c) Gurmit Teotia. Please see the LICENSE file in the project root folder for license information.

using System;
using Guflow.Decider;

namespace RecursionTillEternity
{
    [WorkflowDescription("1.0", DefaultChildPolicy = ChildPolicy.Terminate,
        DefaultExecutionStartToCloseTimeoutInSeconds = 10000, DefaultTaskListName = "tasklist",
        DefaultTaskStartToCloseTimeoutInSeconds = 20)]
    public class DriverWorkflow : Workflow
    {
        public DriverWorkflow()
        {
            ScheduleChildWorkflow<LogProcessWorkflow>()
                .WithInput(WorkflowInput)
                .OnCompletion(RestartAfterTimeout)
                .OnTimedout(RestartImmediately)
                .OnTerminated(RestartImmediately)
                .OnFailure(RestartImmediately);
        }

        private string WorkflowInput(IChildWorkflowItem item)
        {
            var lastEvent = item.LastEvent() as ChildWorkflowEvent;
            return lastEvent != null ? lastEvent.Input : StartedEvent.Input;
        }

        private WorkflowAction RestartAfterTimeout(WorkflowItemEvent @event)
        {
            // Restart this workflow when total events in history are more than 24000. Amazon SWF allows 25000 events in workflow history.
            if (LatestEventId > 24000) return RestartWithDefaultValues(@event); 
            return Reschedule(@event).After(TimeSpan.FromHours(1));
        }

        private WorkflowAction RestartImmediately(WorkflowItemEvent @event)
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