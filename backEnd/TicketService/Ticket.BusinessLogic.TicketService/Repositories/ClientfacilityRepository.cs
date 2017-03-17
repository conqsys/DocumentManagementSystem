
using Dapper;
using Ticket.Base;
using Ticket.Base.Entities;
using Ticket.Base.Repositories;
using Ticket.DataAccess.Common;
using Npgsql;
using SimpleStack.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ticket.BusinessLogic.TicketService
{
    public class ClientFacilityRepository<TClientFacility> : ModuleBaseRepository<TClientFacility>, IClientFacilityRepository
        where TClientFacility : class, IClientFacility, new()
    {
        public ClientFacilityRepository(BaseValidationErrorCodes errorCodes, DatabaseContext dbContext, IUser loggedUser)
            : base(errorCodes, dbContext, loggedUser)
        {
        }


        public async Task<IClientFacility> AddNew(IClientFacility entity)
        {
            try
            {
                this.StartTransaction();
                var savedEntity = await base.AddNew(entity as TClientFacility);
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

        public async Task<int> DeleteClientFacilityByClientId(long clientId)
        {
            try
            {
                this.StartTransaction();
                int deletedRecords = await this.Connection.DeleteAllAsync<TClientFacility>(i => i.ClientId == clientId);
                this.CommitTransaction();
                return deletedRecords;
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


        public async Task<IEnumerable<IClientFacility>> FindClientFacilitiesByClientId(long clientId)
        {
            var clientFacilities = await this.Connection.SelectAsync<TClientFacility>(i => i.ClientId == clientId);
            return clientFacilities;
        }


    }
}
