using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Guflow;

namespace CancellationSupport
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).Wait();
        }

        private static async Task MainAsync(string[] args)
        {
            var domain = new Domain("Test", RegionEndpoint.EUWest1);
            await domain.RegisterWorkflowAsync<OrderWorkflow>();
            await domain.RegisterActivityAsync<ReserveOrder>();
            await domain.RegisterActivityAsync<ChargeCustomer>();
            await domain.RegisterActivityAsync<ShipOrder>();
            using (var hostedActivities = domain.Host(new Type[] { typeof(ReserveOrder), typeof(ChargeCustomer), typeof(ShipOrder) }))
            using (var hostedWorkflows = domain.Host(new[] { new OrderWorkflow() }))
            {
                hostedActivities.StartExecution(new TaskQueue("activity-queue"));
                hostedWorkflows.StartExecution(new TaskQueue("workflow-queue"));
                Console.WriteLine("Press any key to terminate");
                Console.ReadKey();
            }
        }
    }
}
