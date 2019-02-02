using System;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.SimpleWorkflow;
using Guflow;
using Guflow.Decider;
using ServerlessManualApproval.Workflows;

namespace ServerlessManualApproval
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).Wait();
        }

        private static async Task MainAsync(string[] args)
        {
            var client = new AmazonSimpleWorkflowClient(new BasicAWSCredentials("key", "secret"), RegionEndpoint.EUWest2);
            var domain = new Domain("GuflowTestDomain", client);
            await domain.RegisterAsync(10, "test guflow");
            await domain.RegisterWorkflowAsync<ExpenseWorkflow>();
            await domain.RegisterWorkflowAsync<PromotionWorkflow>();
            await domain.RegisterWorkflowAsync<PermitIssueWorkflow>();

            using (var workflowHost = domain.Host(new Workflow[] { new ExpenseWorkflow(), new PromotionWorkflow(), new PermitIssueWorkflow() }))
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
