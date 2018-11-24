using System.Threading.Tasks;
using Guflow.Worker;

namespace RecursionTillEternity
{
    [ActivityDescription("1.0", DefaultHeartbeatTimeoutInSeconds = 100, DefaultScheduleToCloseTimeoutInSeconds = 50,
        DefaultScheduleToStartTimeoutInSeconds = 20, DefaultStartToCloseTimeoutInSeconds = 80,
        DefaultTaskListName = "sometask", DefaultTaskPriority = 10)]
    public class DownloadLogActivity : Activity
    {
        [ActivityMethod]
        public async Task<ActivityResponse> Execute()
        {
            await Task.Delay(10);
            return Complete(new {DownloadedFile = "Some downloaded file"});
        }
    }
}