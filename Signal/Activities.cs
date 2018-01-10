using System.Threading.Tasks;
using Guflow.Decider;
using Guflow.Worker;

namespace Signal
{

    [ActivityDescription("1.0")]
    public class ReserveOrder : Activity
    {
        [ActivityMethod]
        public async Task<string> Reserve(string input)
        {
            //reserve
            await Task.Delay(0);
            return "NotAvailable";
        }
    }

    [ActivityDescription("1.0")]
    public class ChargeCustomer : Activity
    {
        [ActivityMethod]
        public async Task<string> Charge(string input)
        {
            //reserve
            await Task.Delay(0);
            return "success";
        }
    }

    [ActivityDescription("1.0")]
    public class ShipOrder : Activity
    {
        [ActivityMethod]
        public async Task<string> Ship(string input)
        {
            //reserve
            await Task.Delay(0);
            return "shipped";
        }
    }
}