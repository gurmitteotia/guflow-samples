using System;
using System.Threading;
using System.Threading.Tasks;
using Guflow.Worker;

namespace ChildWorkflow
{
    [ActivityDescription("1.0", DefaultHeartbeatTimeoutInSeconds = 100, DefaultScheduleToCloseTimeoutInSeconds = 50,
        DefaultScheduleToStartTimeoutInSeconds = 20, DefaultStartToCloseTimeoutInSeconds = 80,
        DefaultTaskListName = "childworkflow", DefaultTaskPriority = 10)]
    public class PlayOnSwing : Activity
    {
        [ActivityMethod]
        public async Task<string> Execute(CancellationToken token)
        {
            await Task.Delay(TimeSpan.FromSeconds(20), token);
            return "enjoyed";
        }
    }
}