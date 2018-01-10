using Guflow.Decider;

namespace EmptyWorkflow
{
    //This workflow will be finished immediately
    [WorkflowDescription("1.0", DefaultTaskListName = "myqueue")]
    public class EmptyWorkflow : Workflow
    {
        
    }
}