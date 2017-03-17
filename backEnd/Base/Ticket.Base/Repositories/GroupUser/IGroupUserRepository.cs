using Ticket.Base.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.Base.Repositories
{
    public interface IGroupUserRepository : IDepRepository
    {
        Task<IGroupUser> AddNew(IGroupUser entity);

        Task<int> DeleteGroupUserByUserId(long userId);

        Task<IEnumerable<IGroupUser>> FindGroupUsersByUserId(long userId);
    }
}
