using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.Base.Entities
{
    public interface IStamp: ICreatedStamp
    {
        long? ModifiedBy { get; set; }

        DateTime? ModifiedDate { get; set; }
    }
}
