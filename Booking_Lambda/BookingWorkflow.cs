// /Copyright (c) Gurmit Teotia. Please see the LICENSE file in the project root folder for license information.

using System.Data.SqlClient;
using Guflow.Decider;

namespace Booking_Lambda
{
    /*
     *  BookFlight          BookHotel
            |                   |
            |                   |
            v                   v
        ChoosSeat          BookDinner
            |                   |
            |                   |
            `````````````````````
                    |
                    v
              ChargeCustomer
                    |
                    v
               SendEmail
     *
     *
     *
     */
    public class BookingWorkflow : Workflow
    {
        public BookingWorkflow()
        {
            ScheduleLambda("BookFlight").When(_ => Input.BookFlight);
            ScheduleLambda("ChooseSeat").AfterLambda("BookFlight");

            ScheduleLambda("BookHotel").When(_ => Input.BookHotel);
            ScheduleLambda("BookDinner").AfterLambda("BookHotel");

            ScheduleLambda("ChargeCustomer").AfterLambda("ChoosSeat").AfterLambda("BookDinner");

            ScheduleLambda("SendEmail").AfterLambda("ChargeCustomer");
        }

        [WorkflowEvent(EventName.WorkflowStarted)]
        private WorkflowAction OnStart()
        {
            if (!Input.BookFlight && !Input.BookHotel)
                return CompleteWorkflow("Nothing to do");

            return StartWorkflow();
        }
    }

    /*
    Above workflow has four possible execution scenarios:
        1. User has choosen to book both flight and hotel: In this case ChargeCustomer lambda function will be scheduled only after completion of ChoosSeat and BookDinner lambda functions.
        2. User has choosen to book the flight only: In this case ChargeCustomer lambda will be scheduled after ChoosSeat lambda.
        3. User has choosen to book the hotel only: In this case ChargeCustomer lambda will be scheduled after BookDinner lambda. 
        4. User has choosen neither to book flight nor hotel: In this case OnStart method will complete the workflow.
     *
     */
}