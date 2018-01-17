using System.Threading;
using System.Threading.Tasks;
using Guflow.Worker;

namespace CancellationSupport
{
    [ActivityDescription("1.0", DefaultHeartbeatTimeoutInSeconds = 1, DefaultScheduleToCloseTimeoutInSeconds = 50,
        DefaultScheduleToStartTimeoutInSeconds = 20, DefaultStartToCloseTimeoutInSeconds = 80,
        DefaultTaskListName = "sometask", DefaultTaskPriority = 10)]
    [EnableHeartbeat(IntervalInMilliSeconds = 500)] // Enable heartbeat to support cancellation.
    public class ChargeCustomer : Activity
    {
        [ActivityMethod]
        public async Task<string> Charge(string input, CancellationToken cancellationToken)
        {
            //reserve
            cancellationToken.ThrowIfCancellationRequested();
            await Task.Delay(0);
            return "success";
        }
    }
}