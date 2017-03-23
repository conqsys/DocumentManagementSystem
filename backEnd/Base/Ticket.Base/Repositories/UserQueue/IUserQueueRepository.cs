using Ticket.Base.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.Base.Repositories
{
    public interface IUserQueueRepository : IDepRepository
    {
        Task<bool> AddNew(IEnumerable<IUserQueue> entity, long userId, long userDetailId);

        Task<IUserQueue> Update(IUserQueue entity);
                
        Task Delete(long id);
    }
}
