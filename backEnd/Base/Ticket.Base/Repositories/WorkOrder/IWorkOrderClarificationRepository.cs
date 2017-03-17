using Ticket.Base.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Ticket.Base.Repositories
{
    public interface IWorkOrderClarificationRepository : IDepRepository
    {
        Task<IWorkOrderClarification> AddNew(IWorkOrderClarification entity, long loggedUserId );

        Task<IWorkOrderClarification> Update(IWorkOrderClarification entity, long loggedUserId);

        Task<IWorkOrderClarification> VerifyClarification(IWorkOrderClarification entity , long loggedUserId);

        Task<IEnumerable<IWorkOrderClarification>> FindClarificationsByWorkOrderId(long workOrderId);

        Task<IWorkOrderClarification> ReAssignClarificationAnwserer(IWorkOrderClarification entity, long loggedUserId);
    }
}
