using Ticket.Base.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Ticket.Base.Repositories
{
    public interface ITicketStatusRepository : IDepRepository
    {
        Task<ITicketStatus> AddNew(ITicketStatus entity);

        Task<ITicketStatus> Update(ITicketStatus entity);

        Task<ITicketStatus> ChangeActiveState(long ticketStatusId, bool isEnabled, long modifiedById);

        Task<IEnumerable<ITicketStatus>> FindAllTicketStatus(bool enabledRequired);

        Task Delete(long id);
    }
}
