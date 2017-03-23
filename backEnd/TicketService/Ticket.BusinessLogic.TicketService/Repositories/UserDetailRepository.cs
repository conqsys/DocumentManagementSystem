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
    public class UserDetailRepository<TUserDetail> : ModuleBaseRepository<TUserDetail>, IUserDetailRepository
        where TUserDetail : class, IUserDetail, new()
        
    {
        private IUserQueueRepository _userQueueRepository;
        public UserDetailRepository(BaseValidationErrorCodes errorCodes, DatabaseContext dbContext, IUser loggedUser,
            IUserQueueRepository userQueueRepository)
            : base(errorCodes, dbContext, loggedUser)
        {
            this._userQueueRepository = userQueueRepository;
        }


        public async Task<IUserDetail> AddNew(IUserDetail entity)
        {
            TUserDetail tEntity = entity as TUserDetail;

            try
            {                
                this.StartTransaction();
                var savedEntity = await base.AddNew(entity as TUserDetail);
                var userId = entity.UserId;
                var userDetailId = savedEntity.Id;
                await this._userQueueRepository.AddNew(entity.SelectedQueues, userId, userDetailId);             
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

        public async Task<IUserDetail> Update(IUserDetail entity)
        {
            TUserDetail tEntity = entity as TUserDetail;
            try
            {
                this.StartTransaction();
                await base.Update(tEntity, x => new
                {
                    x.EmailAddress,
                    x.FirstName,
                    x.LastName,
                    x.ChangePasswordAtLogOn,
                    x.ChangePasswordAtSixtyDays,
                    x.EnforcePasswordHistory,
                    x.UserName
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
                var entity = await this.Connection.FirstOrDefaultAsync<TUserDetail>(i => i.Id == id);
                await this.Connection.DeleteAsync<TUserDetail>(entity);
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
