
**Before running these samples: **:
* Deploy the lambda functions to your AWS account
* Give access to SWF to invoke lambda functions as described [here](https://docs.aws.amazon.com/amazonswf/latest/awsflowguide/lambda-task.html)
* Update the lambda role name, created in above step, in each workflow.
* Provide "AccessKey" and "SecretKey" in Program.cs

Now run this project and it will register the workflows with Amazon SWF. Login to SWF console and start the execution of a registered workflow. 

All the workflows in this project uses lambda functions but you can easily replace lambda function with either activity or child workflow. This project implements following three workflows:

1.  ExponseWorkflow: In this example after executing the "ApproveExpense" lambda function workflow waits for either "Accepted" or "Rejected" signal and based on the received signal it further schedule the children of "ApproveExpense" lambda function.

```

              ApproveExpense          
                  |
                  |
                  v
         |````````````````````|
    <Accepted>            <Rejected>
         |                    |
         |                    |
         v                    v
    SubmitToAccount     SendRejectEmail  
```            
            
1. PromotionWorkflow: After executing the "PromoteEmployee" lambda function workflow execution is paused and it wait for two siganls- "HRApproved" and "ManagerApproved" to be received to continue to the execution.

```

            PromoteEmployee          
                  |
                  |
                  v
        WaitForAllSignals("HRApproved", "ManagerApproved")
                  |
                  |
                  v
               Promoted
                  |
                  |
                  v
              SendForReview
   
```

1. PermitIssueWorkflow: In this example workflow schedule three lambda functions in parallel and waits for siganls after execution of each lambda function in parallel branches. This example deliveratly schedule the lamdba functions in parallel branches to show you how easily you can create complex dependencies.

```
       ApplyToCouncil                  ApplyToFireDept              ApplyToForestDept
            |                                  |                           |
            |                                  |                           |
            v                                  v                           v
  <"CApproved", "CRejected">     <"FApproved", "FRejected">   <"FrApproved", "FrRejected">  
            |                                  |                           |
            |                                  |                           |
            ````````````````````````````````````````````````````````````````
                    |                                       |
               EveryOneApproved                         AnyOneReject
                    |                                       |
                    v                                       v
               IssuePermit                             RejectPermit


```

