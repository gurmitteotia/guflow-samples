using System.Threading.Tasks;
using Guflow.Worker;

namespace VideoTranscoding.Activities
{
    [ActivityDescription("1.0", DefaultHeartbeatTimeoutInSeconds = 100, DefaultScheduleToCloseTimeoutInSeconds = 50,
        DefaultScheduleToStartTimeoutInSeconds = 20, DefaultStartToCloseTimeoutInSeconds = 80,
        DefaultTaskListName = "sometask", DefaultTaskPriority = 10)]
    public class TranscodeActivity : Activity
    {
        [ActivityMethod]
        public async Task<Response> Execute(string input)
        {
            await Task.Delay(20);
            return new Response() { TranscodedFile = "ouput file path"};
        }

        public class Response
        {
            public string TranscodedFile;
        }
    }
}