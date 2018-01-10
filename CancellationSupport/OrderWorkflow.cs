using System.Linq;
using Guflow.Decider;

namespace CancellationSupport
{
    //This will will support the cancellation. On receiving the cancel request it will send the cancel request to all 
    //active activities and try to cancel them.
    [WorkflowDescription("1.0", DefaultChildPolicy = ChildPolicy.Terminate,
        DefaultExecutionStartToCloseTimeoutInSeconds = 10000, DefaultTaskListName = "tasklist",
        DefaultTaskStartToCloseTimeoutInSeconds = 20)]
    public class OrderWorkflow : Workflow
    {
        public OrderWorkflow()
        {
            ScheduleActivity<ReserveOrder>();

            ScheduleActivity<ChargeCustomer>().AfterActivity<ReserveOrder>();

            ScheduleActivity<ShipOrder>().AfterActivity<ChargeCustomer>();
        }

        [WorkflowEvent(EventName.CancelRequest)]
        public WorkflowAction Cancel()
        {
            return CancelRequest.For(Activities.Where(a => a.IsActive));
        }
    }
}