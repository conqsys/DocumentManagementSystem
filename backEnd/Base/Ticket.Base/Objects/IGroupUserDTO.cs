using Ticket.Base.Entities;
using Ticket.Base.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Ticket.Base.Objects
{
    public interface IGroupUserDTO 
    {
       IEnumerable<IGroupUser> GroupUsers { get; set; }
    }
}
