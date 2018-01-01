using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Guflow;
using TaskListRouting.Activities;

namespace TaskListRouting
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).Wait();
        }

        static async Task MainAsync(string[] args)
        {
            var domain = new Domain("learning", RegionEndpoint.EUWest2);
            await domain.RegisterAsync(10);
            var activities = new[]
            {
                typeof(DownloadActivity), typeof(TranscodeActivity), typeof(UploadToS3Activity),
                typeof(SendConfirmationActivity)
            };
            foreach (var a in activities)
                await domain.RegisterActivityAsync(a);

            await domain.RegisterWorkflowAsync<TranscodeWorkflow>();

            using (var workflowHost = domain.Host(new[] { new TranscodeWorkflow() }))
            using (var activityHost = domain.Host(activities))
            {
                workflowHost.StartExecution();
                activityHost.StartExecution(new TaskQueue(PollingQueue.Download));
                Console.WriteLine("Press any key to terminate");
                Console.ReadKey();
            }
        }
    }
}
