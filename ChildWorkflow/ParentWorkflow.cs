using System.Linq;
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

        //In this case signal name can't be written as method name.
        [SignalEvent(Name = "Come for dinner")]
        public WorkflowAction OnSignalFromHome()
           =>   ChildWorkflow<KidPlayWorkflow>().IsActive
                ? Signal("Hello kid", "").ForChildWorkflow<KidPlayWorkflow>()
                : CompleteWorkflow("okay coming");

        //In this case signal name will be matched with method name.
        [SignalEvent]
        public WorkflowAction HelloParent()
            => ChildWorkflow<KidPlayWorkflow>().IsActive
                ? Signal("Let us have dinner", "").ForChildWorkflow<KidPlayWorkflow>()
                : CompleteWorkflow("okay coming");
    }
}