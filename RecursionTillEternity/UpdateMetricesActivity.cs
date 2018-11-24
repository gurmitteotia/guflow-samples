using System;
using System.Threading.Tasks;
using Guflow.Worker;

namespace RecursionTillEternity
{
    [ActivityDescription("1.0", DefaultHeartbeatTimeoutInSeconds = 100, DefaultScheduleToCloseTimeoutInSeconds = 50,
        DefaultScheduleToStartTimeoutInSeconds = 20, DefaultStartToCloseTimeoutInSeconds = 80,
        DefaultTaskListName = "sometask", DefaultTaskPriority = 10)]
    public class UpdateMetricesActivity : Activity
    {
        [ActivityMethod]
        public async Task<ActivityResponse> Execute(Input input)
        {
            Console.WriteLine($"Generating metrices from file {input.InputFile}");
            await Task.Delay(10);
            return Complete("done");
        }

        public class Input
        {
            public string InputFile;
        }
    }
}