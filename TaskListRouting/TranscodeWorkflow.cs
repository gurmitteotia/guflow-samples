using System;
using Guflow.Decider;
using TaskListRouting.Activities;

namespace TaskListRouting
{
    [WorkflowDescription("1.0", DefaultChildPolicy = ChildPolicy.Terminate,
        DefaultExecutionStartToCloseTimeoutInSeconds =10000, DefaultTaskListName = "tasklist",
        DefaultTaskStartToCloseTimeoutInSeconds = 20)]
    public class TranscodeWorkflow : Workflow
    {
        public TranscodeWorkflow()
        {
            ScheduleActivity<DownloadActivity>()
                .OnFailure(e => Reschedule(e).After(TimeSpan.FromSeconds(2)).UpTo(times:4));

            ScheduleActivity<TranscodeActivity>()
                .AfterActivity<DownloadActivity>()
                .OnTaskList(a => ParentResult(a).PollingQueue)
                .WithInput(a => new {InputFile = ParentResult(a).DownloadedFile, Format = "MP4"})
                .OnTimedout(_ => RestartWorkflow());

            ScheduleActivity<UploadToS3Activity>()
                .AfterActivity<TranscodeActivity>()
                .OnTaskList(_ => Activity<DownloadActivity>().Result().PollingQueue)
                .WithInput(a => new {InputFile = ParentResult(a).TranscodedFile})
                .OnTimedout(_ => RestartWorkflow());
                

           ScheduleActivity<SendConfirmationActivity>().AfterActivity<UploadToS3Activity>();
        }

        private static dynamic ParentResult(IActivityItem a) => a.ParentActivity().Result();
    }
}