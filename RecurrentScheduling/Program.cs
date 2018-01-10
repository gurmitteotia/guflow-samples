using System;
using System.Threading.Tasks;
using Amazon;
using Guflow;

namespace RecurrentScheduling
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
            await domain.RegisterWorkflowAsync<ProcessLogWorkflow>();
            await domain.RegisterActivityAsync<ProcessLog>();
            using (var hostedActivities = domain.Host(new Type[] { typeof(ProcessLog)}))
            using (var hostedWorkflows = domain.Host(new[] { new ProcessLogWorkflow(),  }))
            {
                hostedActivities.StartExecution(new TaskList("activity-queue"));
                hostedWorkflows.StartExecution(new TaskList("workflow-queue"));
                Console.WriteLine("Press any key to terminate");
                Console.ReadKey();
            }
        }
    }
}
