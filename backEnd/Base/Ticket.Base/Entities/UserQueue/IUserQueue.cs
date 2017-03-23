
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ticket.Base.Objects;

namespace Ticket.Base.Entities
{
    public interface IUserQueue : IEntity
    {
        
        long UserId { get; set; }
        long QueueId { get; set; }

        long UserDetailId { get; set; }


    }
}
