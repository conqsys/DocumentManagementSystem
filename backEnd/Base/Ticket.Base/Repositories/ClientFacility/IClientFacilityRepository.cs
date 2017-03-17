using Ticket.Base.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.Base.Repositories
{
    public interface IClientFacilityRepository : IDepRepository
    {
        Task<IClientFacility> AddNew(IClientFacility entity);

        Task<int> DeleteClientFacilityByClientId(long clientId);

        Task<IEnumerable<IClientFacility>> FindClientFacilitiesByClientId(long clientId);
    }
}
