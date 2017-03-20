using Ticket.Base.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.Base.Repositories
{
    public interface IUserQueueRepository : IDepRepository
    {
        Task<IUserQueue> AddNew(IUserQueue entity);

        Task<IUserQueue> Update(IUserQueue entity);

        
        Task Delete(long id);
    }
}
