using System;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.SimpleWorkflow;
using Guflow;
using RecursionTillEternity;

namespace RecursionForEternity
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
            await domain.RegisterWorkflowAsync<DriverWorkflow>();
            await domain.RegisterWorkflowAsync<LogProcessWorkflow>();
            var activityTypes = new[] {typeof(DownloadLogActivity), typeof(UpdateMetricesActivity)};
            foreach (var activityType in activityTypes)
                await domain.RegisterActivityAsync(activityType);

            using (var activitiHost = domain.Host(activityTypes))
            using (var driverWorkflowHost = domain.Host(new[] { new DriverWorkflow()}))
            using (var workflowHost = domain.Host(new[] { new LogProcessWorkflow()}))
            {
                activitiHost.StartExecution();
                var taskList = new TaskList("tasklist");
                taskList.ReadStrategy = TaskList.ReadFirstPage;
                driverWorkflowHost.StartExecution(taskList);
                workflowHost.StartExecution();
                Console.WriteLine("Press any key to terminate");
                Console.ReadKey();
            }
        }
    }
}
