using System;
using System.Threading.Tasks;
using Guflow.Worker;
using ThirdParty.BouncyCastle.Asn1;

namespace TaskListRouting.Activities
{
    [ActivityDescription("1.0")]
    public class TranscodeActivity : Activity
    {
        [Execute]
        public async Task<Response> Execute(Input input)
        {
            //simulate transcoding
            Console.WriteLine($"Trancoding input file{input.InputFilePath} to format {input.Format}");
            await Task.Delay(30);
            return new Response(){TranscodedFilePath = "transcoded file path"};
        }

        public class Response
        {
            public string TranscodedFilePath;
        }

        public class Input
        {
            public string Format;

            public string InputFilePath;
        }
    }
}