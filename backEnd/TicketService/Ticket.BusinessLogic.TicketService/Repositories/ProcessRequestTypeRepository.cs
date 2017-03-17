
using Ticket.Base.Entities;
using Ticket.Base.Repositories;
using Ticket.DataAccess.Common;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ticket.BusinessLogic.TicketService
{
    public class ProcessRequestTypeRepository<TProcessRequestType> : ModuleBaseRepository<TProcessRequestType>, IProcessRequestTypeRepository
        where TProcessRequestType : class, IProcessRequestType, new()
    {
        public ProcessRequestTypeRepository(BaseValidationErrorCodes errorCodes, DatabaseContext dbContext, IUser loggedUser)
            : base(errorCodes, dbContext, loggedUser)
        {
        }


        public async Task<IProcessRequestType> AddNew(IProcessRequestType entity)
        {
            try
            {
                this.StartTransaction();
                var savedEntity = await base.AddNew(entity as TProcessRequestType);
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

        public async Task<int> DeleteProcessRequestTypeByProcessId(long processId)
        {
            try
            {
                this.StartTransaction();

                await this.Connection.DeleteAllAsync<TProcessRequestType>(i => i.ProcessId == processId);

                this.CommitTransaction();
                return 1;
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


        public async Task<IEnumerable<IProcessRequestType>> FindProcessRequestTypeByProcessId(long processId)
        {
            var processRequestTypes = await this.Connection.SelectAsync<TProcessRequestType>(i => i.ProcessId == processId);
            return processRequestTypes;
        }

    }
}
