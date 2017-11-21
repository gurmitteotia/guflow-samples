using System;
using Guflow.Decider;

namespace RecurrentScheduling
{
    //Following workflow will keep rescheduling the activities after it is completed.
    
    [WorkflowDescription("1.0")]
    public class ProcessLogWorkflow : Workflow
    {
        public ProcessLogWorkflow()
        {
            //On completion reschedule immediately.
            ScheduleActivity<ProcessLog>().OnCompletion(Reschedule);

            //OR On completion reschedule after a timeout.
            //ScheduleActivity<ProcessLog>().OnCompletion(e=>Reschedule(e).After(TimeSpan.FromMinutes(1)));

            //OR On completion reschedule after a timeout and only 100 times
            //ScheduleActivity<ProcessLog>().OnCompletion(e => Reschedule(e).After(TimeSpan.FromMinutes(1)).UpTo(Limit.Count(100)));
        }
    }
}