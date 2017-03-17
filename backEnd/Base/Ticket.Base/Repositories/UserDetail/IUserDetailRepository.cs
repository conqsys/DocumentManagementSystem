using Ticket.Base.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.Base.Repositories
{
    public interface IUserDetailRepository : IDepRepository
    {
        Task<IUserDetail> AddNew(IUserDetail entity);

        Task<IUserDetail> Update(IUserDetail entity);

        
        Task Delete(long id);
    }
}
