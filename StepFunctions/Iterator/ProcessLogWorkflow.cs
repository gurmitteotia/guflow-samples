using System;
using Guflow.Decider;

namespace Iterator
{
    [WorkflowDescription("1.0", DefaultLambdaRole = "provide your lambda role")]
    public class ProcessLogWorkflow : Workflow
    {
        public ProcessLogWorkflow()
        {
            ScheduleLambda("ProcessLog")
                .OnCompletion(e => Reschedule(e).After(TimeSpan.FromSeconds(1)).UpTo(times: 20));
        }
    }
}