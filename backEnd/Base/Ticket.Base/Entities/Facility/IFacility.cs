using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.Base.Entities
{
    public interface IFacility : IEntity, IStamp
    {
        string Name { get; set; }

        string Description { get; set; }

        bool Enabled { get; set; }

    }
}
