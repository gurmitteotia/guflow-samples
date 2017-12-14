using System.Threading.Tasks;
using Guflow.Worker;

namespace TaskListRouting.Activities
{
    [ActivityDescription("1.0")]
    public class DownloadActivity : Activity
    {

        [Execute]
        public async Task<Response> Execute(string input)
        {
            //simulate downloading of file
            await Task.Delay(10);
            return new Response() { DownloadedPath = "downloaded path", PollingQueue = }
        }


        public class Response
        {
            public string DownloadedPath;

            public string PollingQueue;
        }

    }
}