using System;
using System.Threading;
using System.Threading.Tasks;
using Guflow.Worker;

namespace ChildWorkflow
{
    [ActivityDescription("1.0", DefaultHeartbeatTimeoutInSeconds = 100, DefaultScheduleToCloseTimeoutInSeconds = 50,
        DefaultScheduleToStartTimeoutInSeconds = 20, DefaultStartToCloseTimeoutInSeconds = 80,
        DefaultTaskListName = "childworkflow", DefaultTaskPriority = 10)]
    [EnableHeartbeat(IntervalInMilliSeconds = 1000)]
    public class PlayOnZipWire : Activity
    {
        [ActivityMethod]
        public async Task<string> Execute(CancellationToken token)
        {
            await Task.Delay(TimeSpan.FromSeconds(40), token);
            return "enjoyed zipwire";
        }
    }
}