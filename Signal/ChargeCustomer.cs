using System.Threading.Tasks;
using Guflow.Worker;

namespace Signal
{
    [ActivityDescription("1.0", DefaultHeartbeatTimeoutInSeconds = 100, DefaultScheduleToCloseTimeoutInSeconds = 50,
        DefaultScheduleToStartTimeoutInSeconds = 20, DefaultStartToCloseTimeoutInSeconds = 80,
        DefaultTaskListName = "sometask", DefaultTaskPriority = 10)]
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
}