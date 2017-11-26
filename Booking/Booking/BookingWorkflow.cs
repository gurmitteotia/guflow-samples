using Guflow.Decider;

namespace Booking
{
    /*In following example there two parallel branches which will execute
     in parallel when workflow is started. However either of them may not start
     if user has choosed not book it.

        BookFlight      ChooseFlightMeal
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
    
    [WorkflowDescription("1.0")]
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
    }
}