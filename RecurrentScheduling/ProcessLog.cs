using System.Threading.Tasks;
using Guflow.Decider;
using Guflow.Worker;

namespace RecurrentScheduling
{
    [WorkflowDescription("1.0")]
    public class ProcessLog : Activity
    {
        [ActivityMethod]
        public async Task<string> Process(string input)
        {
            //download and process log
            await Task.Delay(10);

            return "done";
        }
    }
}