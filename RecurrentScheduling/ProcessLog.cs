using System.Threading.Tasks;
using Guflow.Worker;

namespace RecurrentScheduling
{
    [ActivityDescription("1.0", DefaultHeartbeatTimeoutInSeconds = 100, DefaultScheduleToCloseTimeoutInSeconds = 50,
        DefaultScheduleToStartTimeoutInSeconds = 20, DefaultStartToCloseTimeoutInSeconds = 80,
        DefaultTaskListName = "sometask", DefaultTaskPriority = 10)]
    public class ProcessLog : Activity
    {
        [ActivityMethod]
        public async Task<string> Process(string input)
        {
            //download and process log
            await Task.Delay(10);

            return "done";
        }
    }
}