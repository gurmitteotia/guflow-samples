using System.Threading;
using System.Threading.Tasks;
using Guflow.Worker;

namespace CancellationSupport
{
    [ActivityDescription("1.0", DefaultHeartbeatTimeoutInSeconds = 1, DefaultScheduleToCloseTimeoutInSeconds = 50,
        DefaultScheduleToStartTimeoutInSeconds = 20, DefaultStartToCloseTimeoutInSeconds = 80,
        DefaultTaskListName = "sometask", DefaultTaskPriority = 10)]
    [EnableHeartbeat(IntervalInMilliSeconds = 500)] // Enable heartbeat to support cancellation.
    public class ReserveOrder : Activity
    {
        [ActivityMethod]
        public async Task<string> Reserve(string input, CancellationToken cancellationToken)
        {
            //reserve
            await Task.Delay(20000);
            cancellationToken.ThrowIfCancellationRequested();
            return "done";
        }
    }
}