using Ticket.Base.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Ticket.Base.Repositories
{
    public interface IWorkOrderRepository : IDepRepository
    {
        Task<IWorkOrder> AddNew(IWorkOrder entity , long loggedUserId);

        Task<IWorkOrder> Update(IWorkOrder entity , long loggedUserId);

        Task<IEnumerable<IWorkOrder>> FindAllWorkOrders( long loggedUserId , long loggedUserRoleId , long? loggedUserCoordinatorId , long? loddedUserAccountId);

        Task<IWorkOrder> FindWorkOrderById(long id);

        Task<int> Delete(long workOrderId); 
    }
}
