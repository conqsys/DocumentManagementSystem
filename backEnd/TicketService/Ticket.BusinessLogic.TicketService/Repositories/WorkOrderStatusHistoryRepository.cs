
using Ticket.Base.Entities;
using Ticket.Base.Repositories;
using Ticket.DataAccess.Common;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.BusinessLogic.TicketService
{
    public class WorkOrderStatusHistoryRepository<TWorkOrderStatusHistory> : ModuleBaseRepository<TWorkOrderStatusHistory>, IWorkOrderStatusHistoryRepository
        where TWorkOrderStatusHistory : class, IWorkOrderStatusHistory, new()
    {
        public WorkOrderStatusHistoryRepository(BaseValidationErrorCodes errorCodes,
                                                        DatabaseContext dbContext,
                                                        IUser loggedUser
                                                       )
            : base(errorCodes, dbContext, loggedUser)
        {
        }


        public async Task<IWorkOrderStatusHistory> AddNew(IWorkOrderStatusHistory entity)
        {
            TWorkOrderStatusHistory tEntity = entity as TWorkOrderStatusHistory;
            var errors = await this.ValidateEntityToAdd(tEntity);
            if (errors.Count() > 0)
                await this.ThrowEntityException(errors);
            try
            {
                this.StartTransaction();
                var savedEntity = await base.AddNew(entity as TWorkOrderStatusHistory);

                this.CommitTransaction();

                return savedEntity;
            }
            catch (PostgresException ex)
            {
                throw new EntityUpdateException(ex);
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<IWorkOrderStatusHistory>> GetWorkOrderStatusHistoryByWorkOrderId(long workOrderId)
        {
            return await this.Connection.SelectAsync<TWorkOrderStatusHistory>(i => i.WorkOrderId == workOrderId);
        }

        public override async Task<IEnumerable<IValidationResult>> ValidateEntityToAdd(TWorkOrderStatusHistory entity)
        {
            ICollection<IValidationResult> errors = (await this.ValidateEntity(entity)).ToList();
            return errors;
        }


    }
}
