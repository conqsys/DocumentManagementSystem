using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.Base.Entities
{
    public interface ICreatedStamp
    {
        long? CreatedBy { get; set; }

        DateTime? CreatedDate { get; set; }
    }
}
