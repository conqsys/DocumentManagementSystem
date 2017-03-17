using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.Base.Entities
{
    public interface IFileIndexes : IEntity
    {
        string IndexName { get; set; }

        int IndexType { get; set; }

        string DefaultValue { get; set; }

        string ListValue { get; set; }


      
    }
}
