using Guflow.Decider;

namespace RecursionTillEternity
{
    [WorkflowDescription("2.0", DefaultChildPolicy = ChildPolicy.Terminate,
        DefaultExecutionStartToCloseTimeoutInSeconds = 10000, DefaultTaskListName = "differnt_task_list",
        DefaultTaskStartToCloseTimeoutInSeconds = 20)]
    public class LogProcessWorkflow : Workflow
    {
        public LogProcessWorkflow()
        {
            ScheduleActivity<DownloadLogActivity>().WithInput(_ => new {Folder = Input.Source});
            ScheduleActivity<UpdateMetricesActivity>().AfterActivity<DownloadLogActivity>()
                .WithInput(a => new {InputFile = a.ParentActivity().Result().DownloadedFile});
        }
    }
}