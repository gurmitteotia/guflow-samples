using System;
using Guflow.Decider;
using VideoTranscoding.Activities;

namespace VideoTranscoding
{
    /* This example enhance the task routing example. Transcode workflow schedule the activities as shown below

                    DownloadActivity
                           |
                           v
         |````````````````````````````````````|
         |                                    |
         v                                    v
   TranscodeActivity(MP4)            TranscodeActivity(WAV)
         |                                    |  
         v                                    v
   UploadToS3Activity               UploadToS3Activity
         |                                    |
         ``````````````````|```````````````````
                           |
                           v
                      SendConfirmation

    */
    [WorkflowDescription("1.0", DefaultChildPolicy = ChildPolicy.Terminate,
        DefaultExecutionStartToCloseTimeoutInSeconds =10000, DefaultTaskListName = "tasklist",
        DefaultTaskStartToCloseTimeoutInSeconds = 20)]
    public class TranscodeWorkflow : Workflow
    {
        public TranscodeWorkflow()
        {
            ScheduleActivity<DownloadActivity>()
                .OnFailure(e => Reschedule(e).After(TimeSpan.FromSeconds(2)).UpTo(Limit.Count(4)));

            ScheduleActivity<TranscodeActivity>("MPEG")
                .AfterActivity<DownloadActivity>()
                .OnTaskList(a => ParentResult(a).PollingQueue)
                .WithInput(a => new {InputFile = ParentResult(a).DownloadedFile, Format = "MP4"})
                .OnTimedout(_ => RestartWorkflow());

            ScheduleActivity<TranscodeActivity>("WAV")
                .AfterActivity<DownloadActivity>()
                .OnTaskList(a => ParentResult(a).PollingQueue)
                .WithInput(a => new { InputFile = ParentResult(a).DownloadedFile, Format = "WAV" })
                .OnTimedout(_ => RestartWorkflow());

            ScheduleActivity<UploadToS3Activity>("MPEGUpload")
                .AfterActivity<TranscodeActivity>("MPEG")
                .OnTaskList(_ => Activity<DownloadActivity>().Result().PollingQueue)
                .WithInput(a => new {InputFile = ParentResult(a).TranscodedFile})
                .OnTimedout(_ => RestartWorkflow());

            ScheduleActivity<UploadToS3Activity>("WAVUpload")
                .AfterActivity<TranscodeActivity>("WAV")
                .OnTaskList(_ => Activity<DownloadActivity>().Result().PollingQueue)
                .WithInput(a => new { InputFile = ParentResult(a).TranscodedFile })
                .OnTimedout(_ => RestartWorkflow());


            ScheduleActivity<SendConfirmationActivity>()
                .AfterActivity<UploadToS3Activity>("MPEGUpload")
                .AfterActivity<UploadToS3Activity>("WAVUpload");
        }

        private static dynamic ParentResult(IActivityItem a) => a.ParentActivity().Result();
    }
}