using System;
using System.Linq;
using Guflow.Decider;

namespace RecurrentScheduling
{
    [WorkflowDescription("1.0", DefaultChildPolicy = ChildPolicy.Terminate,
        DefaultExecutionStartToCloseTimeoutInSeconds = 10000, DefaultTaskListName = "tasklist",
        DefaultTaskStartToCloseTimeoutInSeconds = 20)]
    public class CustomRetryLogicWorkflow : Workflow
    {
        public CustomRetryLogicWorkflow()
        {
            ScheduleActivity<ProcessLog>()
                .OnFailure(e =>
                {
                    var totalFailedEvents = Activity(e).Events<ActivityFailedEvent>().Count();
                    return Reschedule(e).After(TimeSpan.FromMinutes(totalFailedEvents * 1.5));
                });
        }
    }
}