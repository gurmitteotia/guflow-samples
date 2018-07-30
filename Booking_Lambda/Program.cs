using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Guflow;

namespace Booking_Lambda
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
          
            using (var workflowHost = domain.Host(new[] { new BookingWorkflow() }))
            {
                workflowHost.StartExecution();

                Console.WriteLine("Press a key to terminate");
                Console.ReadKey();
            }
        }
    }
}
