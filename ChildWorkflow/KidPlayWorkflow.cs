using System.Linq;
using Guflow.Decider;
using Guflow.Worker;

namespace ChildWorkflow
{
    [WorkflowDescription("1.0", DefaultChildPolicy = ChildPolicy.Terminate,
        DefaultExecutionStartToCloseTimeoutInSeconds = 10000, DefaultTaskListName = "childworkflow",
        DefaultTaskStartToCloseTimeoutInSeconds = 20)]
    public class KidPlayWorkflow : Workflow
    {
        public KidPlayWorkflow()
        {
            ScheduleActivity<PlayOnSwing>();

            ScheduleActivity<PlayOnZipWire>().AfterActivity<PlayOnSwing>();
        }

        [WorkflowEvent(EventName.Signal)]
        public WorkflowAction OnSignal(WorkflowSignaledEvent @event)
        {
            if (@event.SignalName == "Hello kid")
            {
                return Signal("Hello parent", "").ReplyTo(@event);
            }

            if (@event.SignalName == "Let us have dinner")
            {
                return CancelRequest.For(WorkflowItems.Where(i => i.IsActive));
            }

            //I will ignore rest of the signals
            return Ignore;
        }
    }
}