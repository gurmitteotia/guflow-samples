using Guflow.Decider;

namespace EmptyWorkflow
{
    //This workflow will be finished immediately
    [WorkflowDescription("1.0", DefaultChildPolicy = ChildPolicy.Terminate,
        DefaultExecutionStartToCloseTimeoutInSeconds = 10000, DefaultTaskListName = "tasklist",
        DefaultTaskStartToCloseTimeoutInSeconds = 20)]
    public class EmptyWorkflow : Workflow
    {
        
    }
}