using System.Threading.Tasks;
using Guflow.Worker;

namespace TaskListRouting.Activities
{
    [ActivityDescription("1.0", DefaultHeartbeatTimeoutInSeconds = 100, DefaultScheduleToCloseTimeoutInSeconds = 50,
        DefaultScheduleToStartTimeoutInSeconds = 20, DefaultStartToCloseTimeoutInSeconds = 80,
        DefaultTaskListName = "sometask", DefaultTaskPriority = 10)]
    public class SendConfirmationActivity : Activity
    {
        [Execute]
        public async Task<string> SendEmail(string input)
        {
            await Task.Delay(30);
            return "done";
        }
    }
}