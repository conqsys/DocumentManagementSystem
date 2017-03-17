using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.Base.Entities
{
    public interface IClientFacility : IEntity
    {
        long FacilityId { get; set; }
        long ClientId { get; set; }
    }
}
