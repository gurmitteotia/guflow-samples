using Guflow.Decider;
using TaskListRouting.Activities;

namespace TaskListRouting
{
    [WorkflowDescription("1.0")]
    public class TranscodeWorkflow : Workflow
    {
        public TranscodeWorkflow()
        {
            ScheduleActivity<DownloadActivity>();

            ScheduleActivity<TranscodeActivity>().AfterActivity<DownloadActivity>()
                .OnTaskList(a => ParentResult(a).PollingQueue)
                .WithInput(a => ParentResult(a).DownloadedPath);

            ScheduleActivity<UploadToS3Activity>().AfterActivity<TranscodeActivity>()
                .OnTaskList(a => ParentResult(a).PollingQueue)
                .WithInput(a => ParentResult(a).TranscodedPath);

            ScheduleActivity<SendConfirmationActivity>().AfterActivity<UploadToS3Activity>();
        }


        private static dynamic ParentResult(IActivityItem a) => a.ParentActivity().Result();
    }
}