using Ticket.Base.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.Base.Repositories
{
    public interface IFileIndexesRepository : IDepRepository
    {
        Task<bool> AddNew(IEnumerable<IFileIndexes> entity);

        Task<IFileIndexes> Update(IFileIndexes entity);

        Task<IEnumerable<IFileIndexes>> GetListOfFileIndexes();
        Task Delete(long id);

    }
}
