using Ticket.Base.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.Base.Repositories
{
    public interface IProcessRequestTypeRepository : IDepRepository
    {
        Task<IProcessRequestType> AddNew(IProcessRequestType entity);

        Task<int> DeleteProcessRequestTypeByProcessId(long processId);

        Task<IEnumerable<IProcessRequestType>> FindProcessRequestTypeByProcessId(long processId);
    }
}
