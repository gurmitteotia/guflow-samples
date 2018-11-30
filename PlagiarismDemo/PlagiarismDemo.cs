using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Guflow;
using Guflow.Decider;

namespace PlagiarismDemo
{
    [WorkflowDescription("1.0")]
    public class PlagiarismDemo : Workflow
    {
        public PlagiarismDemo()
        {
            ScheduleLambda("RegisterIncident");

            ScheduleLambda("ScheduleExam").AfterLambda("RegisterIncident")
                .OnCompletion(e => Jump.ToLambda("SendNotification"))
                .OnFailure(e =>e.Reason == "StudentExceededAllowableExamRetries"? Jump.ToLambda("TakeAdministrativeAction"): DefaultAction(e));

            ScheduleLambda("SendNotification").AfterLambda("ScheduleExam");

            ScheduleTimer("ExamConfirmationPending").AfterLambda("SendNotification")
                .FireAfter(Input.ExamDate - DateTime.UtcNow.Date);

            ScheduleLambda("ValidateExamResults").AfterLambda("ExamConfirmationPending")
                .OnCompletion(r=>r.Result=="1"?Jump.ToLambda("ScheduleLambda"):DefaultAction(r));
                
            ScheduleLambda("ResolveIncident").AfterLambda("ValidateExamResult")
                .When(l => l.ParentLambda().Result() == "0");
            ScheduleLambda("TakeAdministrativeAction").AfterLambda("ValidateExamResult")
                .When(l => l.ParentLambda().Result() == "2");
        }
    }
}
