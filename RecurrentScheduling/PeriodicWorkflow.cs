using System;
using Guflow.Decider;

namespace RecurrentScheduling
{
    /// <summary>
    /// Following worklfow will schedule the ProcessLog activity every hour. Please make sure activity is finished within an hour. 
    /// Once the history events reach around 20000 it restart a new workflow.
    /// </summary>
    public class PeriodicWorkflow : Workflow
    {
        public PeriodicWorkflow()
        {

            ScheduleTimer("PeriodicTimer")
                .FireAfter(TimeSpan.FromHours(1))
                .OnFired(e=>LatestEventId>20000 ? RestartWorkflow() : Jump.ToActivity<ProcessLog>() + Reschedule(e));

            ScheduleActivity<ProcessLog>().AfterTimer("PeriodicTimer")
                .OnCompletion(e => Ignore);
        }
    }
}