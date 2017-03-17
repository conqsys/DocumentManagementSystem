using Ticket.Base.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Ticket.Base.Repositories
{
    public interface IWorkOrderStatusHistoryRepository : IDepRepository
    {
        Task<IWorkOrderStatusHistory> AddNew(IWorkOrderStatusHistory entity);

        Task<IEnumerable<IWorkOrderStatusHistory>> GetWorkOrderStatusHistoryByWorkOrderId(long workOrderId);
    }
}
