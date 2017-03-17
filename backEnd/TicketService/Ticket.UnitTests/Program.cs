
using System.Threading.Tasks;

namespace Ticket.UnitTests
{
    public class Program
    {

        public static void Main(string[] args)
        {

            var ticketTests = new TicketTests();
            var ticketClarificationTests = new TicketClarificationTests();
            Task.Run(async () =>
            {
                //  await ticketTests.PostWorkOrder();
                // await ticketTests.PostWorkOrderWithRequiredValidationError();
                await ticketTests.GetWorkOrderById();
                //await ticketTests.PutWorkOrder();
                // await ticketTests.DeleteWorkOrder();
                // await ticketTests.FindAllWorkOrders();

                //await ticketClarificationTests.PostWorkOrderClarification();
                //await ticketClarificationTests.PostWorkOrderClarificationWithRequiredValidationError();
                //await ticketClarificationTests.PutWorkOrderClarification();
                //await ticketClarificationTests.PutWorkOrderVerification();
                //await ticketClarificationTests.GetWorkOrderClarificationById();
                //await ticketClarificationTests.GetWorkOrderClarificationByWorkOrderId();

            }).GetAwaiter().GetResult();
        }



    }
}
