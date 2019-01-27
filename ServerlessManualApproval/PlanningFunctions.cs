// /Copyright (c) Gurmit Teotia. Please see the LICENSE file in the project root folder for license information.

using System;
using Amazon.Lambda.Core;

namespace ServerlessManualApproval
{
    public class PlanningFunctions
    {
        public string ApplyToCouncil(Input input, ILambdaContext context)
        {
            Console.WriteLine($"Store passed workflow id {input.Id} in to database to later use it to send signal");
            return "Done";
        }

        public string ApplyToFireDept(Input input, ILambdaContext context)
        {
            Console.WriteLine($"Store passed workflow id {input.Id} in to database to later use it to send signal");
            return "Done";
        }

        public string ApplyToForestDept(Input input, ILambdaContext context)
        {
            Console.WriteLine($"Store passed workflow id {input.Id} in to database to later use it to send signal");
            return "Done";
        }

        public string IssuePermit(string input, ILambdaContext context)
        {
            return "Issued";
        }

        public string RejectPermit(string input, ILambdaContext context)
        {
            return "Rejected";
        }

        public class Input
        {
            public string Id;
        }
    }
}