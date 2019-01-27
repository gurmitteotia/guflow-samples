// /Copyright (c) Gurmit Teotia. Please see the LICENSE file in the project root folder for license information.

using System;
using Amazon.Lambda.Core;

namespace ServerlessManualApproval
{
    public class PromotionFunctions
    {
        public string PromoteEmployee(Input input, ILambdaContext context)
        {
            Console.WriteLine($"Store passed workflow id {input.Id} in to database to later use it to send signal");
            Console.WriteLine("Send emails to managers to promote the employee.");
            return "Done";
        }

        public string Promoted(string input, ILambdaContext context)
        {
            return "Promoted";
        }

        public string SendForReviewToHr(string input, ILambdaContext context)
        {
            return "HRDone";
        }

        public class Input
        {
            public string Id;
        }
    }
}