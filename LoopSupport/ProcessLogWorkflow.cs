using System;
using Guflow.Decider;

namespace LoopSupport
{

    /// <summary>
    /// Following workflow will execute the lambda tasks- ProcessLog and UpdateMatrices in loop and will restart a new workflow before it hit history events limit with SWF
    /// </summary>
    public class ProcessLogWorkflow : Workflow
    {
        public ProcessLogWorkflow()
        {
            ScheduleLambda("ProcessLog").OnFailure(e => Reschedule(e).After(TimeSpan.FromSeconds(5)));

            ScheduleLambda("UpdateMatrices")
                .AfterLambda("ProcessLog")
                .WithInput(l => l.ParentLambda().Result().Metrices)
                .OnCompletion(JumptToStart);

            ScheduleAction(_ => RestartWorkflow()).AfterLambda("UpdateMetrices");
        }

        private WorkflowAction JumptToStart(LambdaCompletedEvent @event)
        {
            return LatestEventId > 20000 ? Continue(@event) : Jump.ToLambda("ProcessLog");
        }
    }
}