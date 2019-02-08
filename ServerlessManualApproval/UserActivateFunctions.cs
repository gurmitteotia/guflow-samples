using System;
using Amazon.Lambda.Core;

namespace ServerlessManualApproval
{
    public class UserActivateFunctions
    {
        public string ConfirmEmail(PromotionFunctions.Input input, ILambdaContext context)
        {
            Console.WriteLine($"Store passed workflow id {input.Id} in to database to later use it to send signal");
            Console.WriteLine("Send email to user with links to call back in to your system- another lambda function.");
            return "Done";
        }

        public string ActivateUser(string input, ILambdaContext context)
        {
            return "Activated";
        }
        public class Input
        {
            public string Id;
        }
    }
}