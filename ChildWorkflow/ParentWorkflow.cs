using Guflow.Decider;

namespace ChildWorkflow
{
    [WorkflowDescription("1.0", DefaultChildPolicy = ChildPolicy.Terminate,
        DefaultExecutionStartToCloseTimeoutInSeconds = 10000, DefaultTaskListName = "childworkflow",
        DefaultTaskStartToCloseTimeoutInSeconds = 20)]
    public class ParentWorkflow : Workflow
    {
        public ParentWorkflow()
        {
            ScheduleChildWorkflow<KidPlayWorkflow>();
        }

        [WorkflowEvent(EventName.Signal)]
        public WorkflowAction OnSignal(WorkflowSignaledEvent @event)
        {
            var kidsPlayWorkflow = ChildWorkflow<KidPlayWorkflow>();

            if (@event.SignalName == "Wife says: come for dinner")
            {
                return kidsPlayWorkflow.IsActive
                    ? Signal("Hello kid", "").ForChildWorkflow<KidPlayWorkflow>()
                    : CompleteWorkflow("Okay coming");
            }

            if (@event.SignalName == "Hello parent")
            {
                return kidsPlayWorkflow.IsActive
                    ? Signal("Let us have dinner", "").ForChildWorkflow<KidPlayWorkflow>()
                    : CompleteWorkflow("Okay coming");
            }

            //I will ignore rest of the signals
            return Ignore;

        }
    }
}