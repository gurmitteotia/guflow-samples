using System.Threading.Tasks;
using Guflow.Worker;

namespace Booking
{
    [ActivityDescription("1.0")]
    public class SendConfirmation : Activity
    {
        [ActivityMethod]
        public async Task<string> SendEmail(string input)
        {
            //do some work
            await Task.Delay(10);
            return "done";
        }
    }
}