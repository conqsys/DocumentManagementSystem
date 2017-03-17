using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.Base.Repositories
{
    public interface IDepRepository
    {
        Task<object> FindById(long id);
    }
}
