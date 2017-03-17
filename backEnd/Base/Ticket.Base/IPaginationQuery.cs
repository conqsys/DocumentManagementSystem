using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.Base
{
    public interface IPaginationQuery<T>
    {
        IEnumerable<T> Data { get; set; }

        long TotalRecords { get; set; }
    }
}
