using System.Threading.Tasks;
using Guflow.Worker;

namespace Booking
{
    [ActivityDescription("1.0")]
    public class ChooseFlightMeal : Activity
    {
        [ActivityMethod]
        public async Task<string> ChooseMeal(string input)
        {
            //doe some work
            await Task.Delay(10);
            return "done";
        }
    }
}