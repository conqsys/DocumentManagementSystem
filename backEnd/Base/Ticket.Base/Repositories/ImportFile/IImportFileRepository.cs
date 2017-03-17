using Ticket.Base.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.Base.Repositories
{
    public interface IImportFileRepository : IDepRepository
    {
        Task<IImportFile> AddNew(IImportFile entity);

        Task<IImportFile> Update(IImportFile entity);
        
        Task Delete(long id);
    }
}
