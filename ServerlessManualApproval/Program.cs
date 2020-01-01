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
            var client = new AmazonSimpleWorkflowClient(new BasicAWSCredentials("provide key", "provide secret key"), RegionEndpoint.EUWest2);
            var domain = new Domain("GuflowTestDomain", client);
            await domain.RegisterAsync(10, "test guflow");
            await domain.RegisterWorkflowAsync<ExpenseWorkflow>();
            await domain.RegisterWorkflowAsync<ExpenseWorkflowWithTimeout>();
            await domain.RegisterWorkflowAsync<PromotionWorkflow>();
            await domain.RegisterWorkflowAsync<PromotionWorkflowWithTimeout>();
            await domain.RegisterWorkflowAsync<PermitIssueWorkflow>();
            await domain.RegisterWorkflowAsync<PermitIssueWorkflowWithTimeout>();
            await domain.RegisterWorkflowAsync<UserActivateWorkflow>();
            await domain.RegisterWorkflowAsync<UserActivateWorkflowWithTimeout>();

            using (var workflowHost = domain.Host(new Workflow[] { new ExpenseWorkflow(), new ExpenseWorkflowWithTimeout(), 
                new PromotionWorkflow(), new PromotionWorkflowWithTimeout(),  new PermitIssueWorkflow(), new PermitIssueWorkflowWithTimeout(),
                new UserActivateWorkflow(), new UserActivateWorkflowWithTimeout()}))
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
