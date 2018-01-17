using System;
using System.Threading.Tasks;
using Amazon;
using Guflow;

namespace EmptyWorkflow
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).Wait();
        }

        private static async Task MainAsync(string[]args)
        {
            var domain = new Domain("learning", RegionEndpoint.EUWest2);
            await domain.RegisterAsync(1);
            await domain.RegisterWorkflowAsync<EmptyWorkflow>();

            //Now start the workflw form amazon web console and it will be finished immdiately.
            using (var hostedworkflows = domain.Host(new[] { new EmptyWorkflow() }))
            {
                hostedworkflows.StartExecution();
                Console.WriteLine("Press any key to terminate");
                Console.ReadKey();
            }
        }
    }
}
