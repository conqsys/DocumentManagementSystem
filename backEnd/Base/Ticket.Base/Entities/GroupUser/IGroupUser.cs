using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.Base.Entities
{
    public interface IGroupUser : IEntity
    {
        long UserId { get; set; }
        long GroupId { get; set; }
    }
}
