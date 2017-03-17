using Ticket.Base.Entities;
using Ticket.DataAccess;
using Ticket.DataAccess.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.API.TicketService
{
    public class Token
    {
        public string access_token { get; set; }
        public string userName { get; set; }
       
        public string Name { get; set; }
        public int expires_in { get; set; }
        public IUser User { get; set; }
       
    }
}
