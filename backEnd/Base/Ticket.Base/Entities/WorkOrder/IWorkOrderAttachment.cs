using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.Base.Entities
{
    public interface IWorkOrderAttachment : IEntity, ICreatedStamp
    {
        string AttachmentName { get; set; }
        long WorkOrderId { get; set; }
    }
}
