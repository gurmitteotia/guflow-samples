
**Before running these samples:**:
* Deploy the lambda functions to your AWS account
* Give access to SWF to invoke lambda functions as described [here](https://docs.aws.amazon.com/amazonswf/latest/awsflowguide/lambda-task.html)
* Update the lambda role name, created in above step, in each workflow.
* Provide "AccessKey" and "SecretKey" in Program.cs

Now run this project and it will register the workflows with Amazon SWF. Login to SWF console and start the execution of a registered workflow. 

All the workflows in this project uses lambda functions but you can easily replace lambda function with either activity or child workflow. This project implements following three workflows:

* **[ExpenseWorkflow](Workflows/ExpenseWorkflow.cs):** In this example after executing the "ApproveExpense" lambda function workflow waits for either "Accepted" or "Rejected" signal indefinitely and based on the received signal it further schedule the children of "ApproveExpense" lambda function.

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
  
* **[ExpenseWorkflowWithTimeout](Workflows/ExpenseWorkflowWithTimeout.cs):** In this example after executing the "ApproveExpense" lambda function workflow waits for either "Accepted" or "Rejected" signal for 2 days.

  ```

              ApproveExpense          
                  |
                  |
                  v
         |````````````````````|``````````````````````````|
    <Accepted>            <Rejected>				<Timedout>
         |                    |							 | 	
         |                    |							 |
         v                    v							 v
    SubmitToAccount     SendRejectEmail             EscalateExpenses
  ```   

            
* **[PromotionWorkflow](Workflows/PromotionWorkflow.cs):** After executing the "PromoteEmployee" lambda function workflow execution is paused and it wait for two siganls- "HRApproved" and "ManagerApproved" indefinitely.

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

            
* **[PromotionWorkflowWithTimeout](Workflows/PromotionWorkflowWithTimeout.cs):** After executing the "PromoteEmployee" lambda function workflow execution is paused and it wait for two siganls- "HRApproved" and "ManagerApproved" for 5 days.

  ```

            PromoteEmployee          
                  |
                  |
                  v
        WaitForAllSignals("HRApproved", "ManagerApproved")
                  |
                  |
                  v
      |```````````````````````````|       
   Promoted						<Timedout>
      |							  |	
      |							  | 	
      v							  v 	
  SendForReview				Fail workflow
   
  ```

* **[PermitIssueWorkflow](Workflows/PermitIssueWorkflow.cs):** In this example workflow schedule three lambda functions in parallel and waits for the signals after execution of each lambda function in parallel branches. This example schedule the lamdba functions in parallel branches to show you how easily you can create complex dependencies.

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


* **[PermitIssueWorkflowWithTimeout][Workflows/PermitIssueWorkflowWithTimeout.cs]:** In this example workflow schedule three lambda functions in parallel and waits for the signals after execution of each lambda function in parallel branches. This example schedule the lamdba functions in parallel branches to show you how easily you can create complex dependencies.

  ```
       ApplyToCouncil                  ApplyToFireDept              ApplyToForestDept
            |                                  |                           |
            |                                  |                           |
            v                                  v                           v
  <"CApproved", "CRejected">     <"FApproved", "FRejected">   <"FrApproved", "FrRejected">  
            |                                  |                           |
            |                                  |                           |
            `````````````````````````````````````````````````````````````````````````````````````
                    |                                       |									|
               EveryOneApproved                         AnyOneReject						<AnySignalTimedout>	
                    |                                       |									|	
                    v                                       v									v
               IssuePermit                             RejectPermit							Fail workflow


  ```

  
