using Ticket.Base.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.Base.Repositories
{
    public interface IRequestTypeRepository : IDepRepository
    {
        Task<IRequestType> AddNew(IRequestType entity);

        Task<IRequestType> Update(IRequestType entity);

        Task<IRequestType> ChangeActiveState(long requestTypeId, bool isEnabled, long modifiedById);

        Task<IEnumerable<IRequestType>> FindAllRequestTypes(bool enabledRequired);

        Task<IPaginationQuery<IRequestType>> FindAllRequestTypesList();

        Task<IEnumerable<IRequestType>> GetRequestTypeByProcessId(long processId);

        Task Delete(long id);
    }
}
