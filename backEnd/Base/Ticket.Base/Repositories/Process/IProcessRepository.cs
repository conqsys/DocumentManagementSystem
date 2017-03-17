using Ticket.Base.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.Base.Repositories
{
    public interface IProcessRepository : IDepRepository
    {
        Task<IProcess> AddNew(IProcess entity);

        Task<IProcess> Update(IProcess entity);

        Task<IProcess> ChangeActiveState(long processId, bool isEnabled, long modifiedById);

        Task<IEnumerable<IProcess>> FindAllProcess(bool enabledRequired);

        Task<IEnumerable<IProcess>> GetListOfProcesses();

        Task Delete(long id);

    }
}
