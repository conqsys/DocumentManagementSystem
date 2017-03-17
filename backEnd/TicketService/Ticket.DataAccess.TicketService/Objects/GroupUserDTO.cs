using Ticket.Base.Entities;
using Ticket.Base.Objects;
using System.Collections.Generic;

namespace Ticket.DataAccess.TicketService
{
    public class GroupUserDTO : IGroupUserDTO
    {
        public IEnumerable<IGroupUser> GroupUsers { get; set; }
    }
}
