using System;
using System.Threading.Tasks;
using Amazon;
using Guflow;

namespace LoopSupport
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
            await domain.RegisterWorkflowAsync<ProcessLogWorkflow>();
            using (var hostedWorkflows = domain.Host(new[] { new ProcessLogWorkflow(), }))
            {
                hostedWorkflows.StartExecution();
                Console.WriteLine("Press any key to terminate");
                Console.ReadKey();
            }
        }
    }
}
