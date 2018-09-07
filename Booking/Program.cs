using System;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.SimpleWorkflow;
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
            var client = new AmazonSimpleWorkflowClient(new BasicAWSCredentials("secrete id",
                "secret"), RegionEndpoint.EUWest2);
            var domain = new Domain("GuflowTestDomain", client);
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
