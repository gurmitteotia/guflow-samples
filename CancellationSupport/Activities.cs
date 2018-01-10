using System.Threading;
using System.Threading.Tasks;
using Guflow.Worker;

namespace CancellationSupport
{

    [ActivityDescription("1.0", DefaultHeartbeatTimeoutInSeconds = 10)]
    [EnableHeartbeat()] // Enable heartbeat to support cancellation.
    public class ReserveOrder : Activity
    {
        [ActivityMethod]
        public async Task<string> Reserve(string input, CancellationToken cancellationToken)
        {
            //reserve
            cancellationToken.ThrowIfCancellationRequested();
            await Task.Delay(0);
            return "NotAvailable";
        }
    }

    [ActivityDescription("1.0")]
    [EnableHeartbeat] // Enable heartbeat to support cancellation.
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

    [ActivityDescription("1.0")]
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