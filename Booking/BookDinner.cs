using System.Threading.Tasks;
using Guflow.Worker;

namespace Booking
{
    [ActivityDescription("1.0")]
    public class BookDinner : Activity
    {
        [ActivityMethod]
        public async Task<string> Book(string input)
        {
            //do some work
            await Task.Delay(10);
            return "done";
        }
    }
}