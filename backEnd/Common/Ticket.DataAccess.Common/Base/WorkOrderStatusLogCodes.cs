using System;
using System.Collections.Generic;

namespace Ticket.DataAccess.Common
{
    public enum WorkOrderStatusCode : Int32
    {
        NewTicket = 1,
        TicketOpen = 2,
        TicketReAssign = 3,
        TicketClosed = 4
    }

    public class WorkOrderStatusLogCodes
    {
        public WorkOrderStatusLogCodes()
        {
            this.StatusCode = new Dictionary<int, string>();
            InitializeStatusCodes();
        }

        protected virtual void InitializeStatusCodes()
        {
            this.AddStatusCode((int)WorkOrderStatusCode.NewTicket, "New Ticket Added");//4028
            this.AddStatusCode((int)WorkOrderStatusCode.TicketOpen, "Ticket Open");//4028
        }

        public virtual void AddStatusCode(Int32 statusCode, string message)
        {
            this.StatusCode.Add((int)statusCode, message);
        }

        public virtual string GetTicketStatus(Int32 statusCode)
        {
            string statusMessage = this.StatusCode[(int)statusCode];
            return statusMessage;
        }

        public virtual KeyValuePair<int, string> this[int statusCode, params object[] formatter]
        {
            get
            {
                string errorMessage = this.StatusCode[(int)statusCode];
                var returnValue = new KeyValuePair<int, string>((int)statusCode, formatter.Length > 0 ? string.Format(errorMessage, formatter) : errorMessage);
                return returnValue;
            }
        }



        public Dictionary<int, string> StatusCode { get; set; }
    }


}
