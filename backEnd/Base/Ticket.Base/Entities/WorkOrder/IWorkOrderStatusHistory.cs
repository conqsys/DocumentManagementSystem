using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.Base.Entities
{
    public interface IWorkOrderStatusHistory : IEntity,ICreatedStamp
    {
        long WorkOrderId { get; set; }
        long WorkOrderStatusId { get; set; }

    }
}
