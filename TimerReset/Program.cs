using System;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.SimpleWorkflow;
using Guflow;
using Guflow.Decider;

namespace TimerReset
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).Wait();
        }

        private static async Task MainAsync(string[] args)
        {
            var client = new AmazonSimpleWorkflowClient(new BasicAWSCredentials("access key", "secret key"), RegionEndpoint.EUWest2);
            var domain = new Domain("GuflowTestDomain", client);
            await domain.RegisterAsync(10, "test guflow");
            await domain.RegisterWorkflowAsync<OrderWorkflow>();

           

            using (var workflowHost = domain.Host(new Workflow[] { new OrderWorkflow() }))
            {
                workflowHost.OnError(e =>
                {
                    Console.WriteLine(e.Exception); return ErrorAction.Continue;
                });
                workflowHost.StartExecution();
                Console.WriteLine("Press a key to terminate");
                Console.ReadKey();
            }
        }
    }
}
