using System.Threading.Tasks;
using Guflow.Worker;

namespace CancellationSupport
{
    [ActivityDescription("1.0", DefaultHeartbeatTimeoutInSeconds = 100, DefaultScheduleToCloseTimeoutInSeconds = 50,
        DefaultScheduleToStartTimeoutInSeconds = 20, DefaultStartToCloseTimeoutInSeconds = 80,
        DefaultTaskListName = "sometask", DefaultTaskPriority = 10)]
    public class ShipOrder : Activity
    {
        // it is too late to support cancellation here.
        [ActivityMethod]
        public async Task<string> Ship(string input)
        {
            await Task.Delay(0);
            return "shipped";
        }
    }
}