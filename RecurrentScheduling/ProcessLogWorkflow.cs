using System;
using Guflow.Decider;

namespace RecurrentScheduling
{
    //Following workflow will keep rescheduling the activities after it is completed.

    [WorkflowDescription("1.0", DefaultChildPolicy = ChildPolicy.Terminate,
        DefaultExecutionStartToCloseTimeoutInSeconds = 10000, DefaultTaskListName = "tasklist",
        DefaultTaskStartToCloseTimeoutInSeconds = 20)]
    public class ProcessLogWorkflow : Workflow
    {
        public ProcessLogWorkflow()
        {
            //Approach 1: On completion reschedule immediately.
            ScheduleActivity<ProcessLog>().OnCompletion(Reschedule);

            //Approach 2: On completion reschedule after a timeout.
            //ScheduleActivity<ProcessLog>().OnCompletion(e=>Reschedule(e).After(TimeSpan.FromMinutes(1)));

            //Approach 3: On completion reschedule after a timeout and only 100 times
            //ScheduleActivity<ProcessLog>().OnCompletion(e => Reschedule(e).After(TimeSpan.FromMinutes(1)).UpTo(Limit.Count(100)));

            //Approach 4: On completion reschedule after a timeout and up to 100 times. Once it is scheduled 100 time then restart this workflow
            //This approach can help you in containing the events in history.
            //ScheduleActivity<ProcessLog>().OnCompletion(e => Reschedule(e).After(TimeSpan.FromMinutes(1)).UpTo(Limit.Count(100)));
            //ScheduleAction(a => RestartWorkflow()).AfterActivity<ProcessLog>();
        }
    }
}