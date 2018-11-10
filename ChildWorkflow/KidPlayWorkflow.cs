using System.Linq;
using Guflow.Decider;

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

        [SignalEvent(Name = "Hello kid")]
        public WorkflowAction HelloKidSignalAction(WorkflowSignaledEvent @event)
            => Signal("HelloParent", "").ReplyTo(@event);

        //You can also handle signal using generic event handler.
        [WorkflowEvent(EventName.Signal)]
        public WorkflowAction OnSignal(WorkflowSignaledEvent @event)
        {
            //I'm a nice kid I will cancel whichever the activity in progress.
            if (@event.SignalName == "Let us have dinner")
                return CancelRequest.For(WorkflowItems.Where(i => i.IsActive));

            //I will ignore rest of the signals
            return Ignore;
        }
    }
}