﻿// /Copyright (c) Gurmit Teotia. Please see the LICENSE file in the project root folder for license information.

using System;
using Amazon.Lambda.Core;

namespace ServerlessManualApproval
{
    public class ExpenseLambdaFunctions
    {
        public string ApproveExpenses(Input input, ILambdaContext context)
        {
            Console.WriteLine($"Store pass workflow id {input.Id} in to database to later use it to send signal");
            Console.WriteLine("Send emails to manager to approve the expense and emails have approve and reject links");
            return "Done";
        }

        public string SendToAccount(string input, ILambdaContext context)
        {
            return "AccountDone";
        }

        public string SendBackToEmp(string input, ILambdaContext context)
        {
            return "EmpAction";
        }

        public class Input
        {
            public string Id;
        }
    }
}