using Ticket.Base.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.Base.Repositories
{
    public interface IClientRepository : IDepRepository
    {
        Task<IClient> AddNew(IClient entity);

        Task<IClient> Update(IClient entity);

        Task<IClient> ChangeActiveState(long clientId, bool isEnabled, long modifiedById);

        Task<IEnumerable<IClient>> FindAllClients(bool enabledRequired);

        Task Delete(long id);
    }
}
