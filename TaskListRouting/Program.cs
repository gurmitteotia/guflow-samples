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
            Log.Register(Log.ConsoleLogger);
            var domain = new Domain("learning", RegionEndpoint.EUWest2);
            await domain.RegisterAsync(10);
            var activities1 = new[]{typeof(DownloadActivity),typeof(SendConfirmationActivity)};
            var activities2 = new [] {typeof(TranscodeActivity), typeof(UploadToS3Activity)};

            foreach (var a in activities1.Concat(activities2))
                await domain.RegisterActivityAsync(a);

            await domain.RegisterWorkflowAsync<TranscodeWorkflow>();

            using (var workflowHost = domain.Host(new[] { new TranscodeWorkflow() }))
            using (var activityHost1 = domain.Host(activities1))
            using (var activityHost2 = domain.Host(activities2))
            {
                ConfigureLogging(workflowHost, activityHost1, activityHost2);
                workflowHost.StartExecution();
                activityHost1.StartExecution();
                activityHost2.StartExecution(new TaskList(PollingQueue.Download));
                Console.WriteLine("Press any key to terminate");
                Console.ReadKey();
            }
        }

        private static void ConfigureLogging(params IHost[] hosts)
        {
            foreach (var host in hosts)
            {
                host.OnError(e=> { Console.WriteLine(e.Exception);
                    return ErrorAction.Continue;
                });
            }
        }
    }
}
