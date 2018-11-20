// /Copyright (c) Gurmit Teotia. Please see the LICENSE file in the project root folder for license information.

using System;
using Guflow.Decider;

namespace TimerReset
{
    /* In following example when order is place it does not ship it immediately, instead it starts a "GracePeriod" Timer.
     * You can cancel the order with grace period and you can also reset the grace period on receiving the signal.
     */

    [WorkflowDescription("1.0", DefaultChildPolicy = ChildPolicy.Terminate,
        DefaultExecutionStartToCloseTimeoutInSeconds = 10000, DefaultTaskListName = "orderlist",
        DefaultTaskStartToCloseTimeoutInSeconds = 20)]
    public class OrderWorkflow : Workflow
    {
        public OrderWorkflow()
        {
            ScheduleTimer("GracePeriod").FireAfter(TimeSpan.FromMinutes(10))
                .OnCancel(_ => Jump.ToLambda("OrderCancelled"));

            ScheduleLambda("ShipOrder").AfterTimer("GracePeriod");

            ScheduleLambda("OrderCancelled").AfterTimer("GracePeriod")
                .When(e => false);
        }

        [SignalEvent]
        public WorkflowAction ResetGracePeriod() =>
            Timer("GracePeriod").IsActive ? Timer("GracePeriod").Reset() : Ignore;

        [SignalEvent]
        public WorkflowAction CancelOrder()
            =>Timer("GracePeriod").IsActive ? CancelRequest.For(Timer("GracePeriod")) : Ignore;
    }
}