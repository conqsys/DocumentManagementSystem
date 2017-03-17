using Ticket.Base.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.Base.Repositories
{
    public interface IFacilityRepository : IDepRepository
    {
        Task<IFacility> AddNew(IFacility entity);

        Task<IFacility> Update(IFacility entity);

        Task<IFacility> ChangeActiveState(long facilityId, bool isEnabled, long modifiedById);

        Task<IEnumerable<IFacility>> FindAllFacilities(bool enabledRequired);

        Task<IEnumerable<IFacility>> GetFacilitiesByAccountId(long accountId);

        Task Delete(long id);

    }
}
