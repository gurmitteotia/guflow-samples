using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Guflow;

namespace Booking
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).Wait();
        }

        private static async Task MainAsync(string[] args)
        {
            var domain = new Domain("learning", RegionEndpoint.EUWest2);
            await domain.RegisterAsync(10, "test guflow");
            await domain.RegisterWorkflowAsync<BookingWorkflow>();
            var activities = new[]
            {
                typeof(BookFlight), typeof(BookHotel), typeof(ChooseFlightMeal),
                typeof(BookDinner), typeof(SendConfirmation)
            };
            foreach (var activity in activities)
            {
                await domain.RegisterActivityAsync(activity);
            }

            using (var activityHost = domain.Host(activities))
            using (var workflowHost = domain.Host(new []{new BookingWorkflow()}))
            {
                activityHost.StartExecution();
                workflowHost.StartExecution();

                Console.WriteLine("Press a key to terminate");
                Console.ReadKey();
            }
        }
    }
}
