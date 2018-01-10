using System.Threading.Tasks;
using Guflow.Worker;

namespace TaskListRouting.Activities
{
    [ActivityDescription("1.0")]
    public class UploadFileActivity : Activity
    {
        [Execute]
        public async Task<Response> Execute(Input input)
        {
            
        }

        public class Response
        {
            
        }

        public class Input
        {
            
        }
    }
}