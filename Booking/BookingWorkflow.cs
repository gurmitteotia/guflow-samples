using Guflow.Decider;

namespace Booking
{
    /*In following example there two parallel branches which will execute
     in parallel when workflow is started.
           
     Following diagram shows the arrangement in BookingWorkflow
     
        BookFlight          BookHotel
            |                   |
            |                   |
            v                   v
        ChoseFlightMeal     BookDinner
            |                   |
            |                   |
            `````````````````````
                    |
                    |
                    v
              SendConfirmation
    */

    [WorkflowDescription("1.0", DefaultChildPolicy = ChildPolicy.Terminate,
        DefaultExecutionStartToCloseTimeoutInSeconds = 10000, DefaultTaskListName = "tasklist",
        DefaultTaskStartToCloseTimeoutInSeconds = 20)]
    public class BookingWorkflow : Workflow
    {
        public BookingWorkflow()
        {
            ScheduleActivity<BookFlight>().When(a=>Input.BookFlight);
            ScheduleActivity<ChooseFlightMeal>().AfterActivity<BookFlight>();

            ScheduleActivity<BookHotel>().When(_ => Input.BookHotel);
            ScheduleActivity<BookDinner>().AfterActivity<BookHotel>();

            ScheduleActivity<SendConfirmation>().AfterActivity<ChooseFlightMeal>()
                .AfterActivity<BookDinner>();
        }
        /* This workflow has three possible execution scenarios:
         * 1. User has choosen to book both flight and hotel: In this case SendConfirmation activity will be scheduled only after completion
         * of ChooseFlightMeal and BookDinner activities.
         * 2. User has choosen to book the flight only: In this case SendConfirmation activity will be scheduled after ChooseFlightMeal activity.
         * 3. User has choosen to book the hotel only: In this case SendConfirmation activity will be scheduled after BookDinner activity.  
         */
    }
}
