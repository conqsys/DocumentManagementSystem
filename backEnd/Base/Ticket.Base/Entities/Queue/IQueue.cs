
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ticket.Base.Objects;

namespace Ticket.Base.Entities
{
    public interface IQueue : IEntity
    {
       
        string Name { get; set; }
        string Description { get; set; }
        

    }
}
