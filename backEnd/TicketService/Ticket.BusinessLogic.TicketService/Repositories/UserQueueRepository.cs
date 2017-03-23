using Ticket.DataAccess.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Extensions.Configuration;
using Dapper;
using Ticket.Base.Entities;
using Ticket.Base.Repositories;
using Ticket.DataAccess.TicketService;
using Npgsql;
using SimpleStack.Orm;
using SimpleStack.Orm.Expressions;
using Ticket.BusinessLogic.Common;
using Ticket.Base;
using Ticket.Base.Objects;

namespace Ticket.BusinessLogic.TicketService
{
    public class UserQueueRepository<TUserQueue> : ModuleBaseRepository<TUserQueue>, IUserQueueRepository
        where TUserQueue : class, IUserQueue, new()
    {
        public UserQueueRepository(BaseValidationErrorCodes errorCodes, DatabaseContext dbContext, IUser loggedUser)
            : base(errorCodes, dbContext, loggedUser)
        {
        }
        public async Task<bool> AddNew(IEnumerable<IUserQueue> entities, long userId, long userDetailId)
        {
            foreach (IUserQueue entity in entities)
            {
                TUserQueue tEntity = entity as TUserQueue;
                try
                {
                    entity.UserId = userId;
                    entity.UserDetailId = userDetailId;
                    this.StartTransaction();
                    var savedEntity = await base.AddNew(entity as TUserQueue);
                    this.CommitTransaction();

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
            return true;
        }

        public async Task<IUserQueue> Update(IUserQueue entity)
        {
            TUserQueue tEntity = entity as TUserQueue;

            var errors = await this.ValidateEntityToCheckExistsAtUpdate(tEntity);
            if (errors.Count() > 0)
                await this.ThrowEntityException(errors);

            try
            {
                this.StartTransaction();
                await base.Update(tEntity, x => new
                {
                    x.UserId,
                    x.QueueId
                });

                this.CommitTransaction();
                return tEntity;
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
        public async Task Delete(long id)
        {
            try
            {
                this.StartTransaction();
                var entity = await this.Connection.FirstOrDefaultAsync<TUserQueue>(i => i.UserId == id);
                await this.Connection.DeleteAsync<TUserQueue>(entity);
                this.CommitTransaction();
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
    }
}
