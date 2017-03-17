using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.Base.Entities
{
    public interface IProcessRequestType : IEntity
    {
        long ProcessId { get; set; }
        long RequestTypeId { get; set; }
    }
}
