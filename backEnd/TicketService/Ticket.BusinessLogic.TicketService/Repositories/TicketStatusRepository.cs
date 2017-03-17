
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
    public class TicketStatusRepository<TTicketStatus, TWorkOrder> : ModuleBaseRepository<TTicketStatus>, ITicketStatusRepository
        where TTicketStatus : class, ITicketStatus, new()
        where TWorkOrder : class, IWorkOrder, new()
    {
        public TicketStatusRepository(BaseValidationErrorCodes errorCodes, DatabaseContext dbContext, IUser loggedUser)
            : base(errorCodes, dbContext, loggedUser)
        {
        }


        public async Task<ITicketStatus> AddNew(ITicketStatus entity)
        {
            TTicketStatus tEntity = entity as TTicketStatus;

            var errors = await this.ValidateEntityToCheckExists(tEntity);
            if (errors.Count() > 0)
                await this.ThrowEntityException(errors);

            try
            {
                this.StartTransaction();
                var savedEntity = await base.AddNew(entity as TTicketStatus);
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

        public async Task<ITicketStatus> Update(ITicketStatus entity)
        {
            TTicketStatus tEntity = entity as TTicketStatus;

            var errors = await this.ValidateEntityToCheckExistsAtUpdate(tEntity);
            if (errors.Count() > 0)
                await this.ThrowEntityException(errors);

            try
            {
                this.StartTransaction();
                await base.Update(tEntity, x => new
                {
                    x.Name,
                    x.Description,
                    x.Enabled,
                    x.ModifiedBy,
                    x.ModifiedDate
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

        public async Task<ITicketStatus> ChangeActiveState(long ticketStatusId, bool isEnabled, long modifiedById)
        {
            try
            {
                var ticketStatus = await base.FindById(ticketStatusId) as TTicketStatus;
                if (ticketStatus != null)
                {
                    ticketStatus.Enabled = isEnabled;
                    await base.Update(ticketStatus, x => new
                    {
                        x.Enabled,
                        x.ModifiedBy,
                        x.ModifiedDate
                    });
                }
                return ticketStatus;
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


        public async Task<IEnumerable<ITicketStatus>> FindAllTicketStatus(bool enabledRequired)
        {
            if (enabledRequired)
            {
                return (await this.Connection.SelectAsync<TTicketStatus>()).OrderByDescending(i => i.Id).Where(i => i.Enabled == true);
            }
            else
            {
                return (await this.Connection.SelectAsync<TTicketStatus>()).OrderByDescending(i => i.Id);
            }

        }


        public override async Task<IEnumerable<IValidationResult>> ValidateEntityToCheckExists(TTicketStatus entity)
        {
            ICollection<IValidationResult> errors = (await this.ValidateEntity(entity)).ToList();
            if (await this.Exists(entity.Name))
                errors.Add(new ValidationCodeResult(ErrorCodes[EnumErrorCode.TicketStatusExists, entity.Name]));

            return errors;
        }

        public override async Task<bool> Exists(string name)
        {
            var existedTicketStatus = await this.FindFirstByTicketName(name);
            return existedTicketStatus != null;
        }

        public async Task<TTicketStatus> FindFirstByTicketName(string name)
        {
            var ticketStatus = await this.Connection.FirstOrDefaultAsync<TTicketStatus>(i => i.Name == name);
            return ticketStatus;
        }

        public override async Task<IEnumerable<IValidationResult>> ValidateEntityToCheckExistsAtUpdate(TTicketStatus entity)
        {
            ICollection<IValidationResult> errors = (await this.ValidateEntity(entity)).ToList();
            if (await this.ExistByNameAndId(entity.Name, entity.Id))
                errors.Add(new ValidationCodeResult(ErrorCodes[EnumErrorCode.TicketStatusExists, entity.Name]));

            return errors;
        }

        public async Task<bool> ExistByNameAndId(string name, long Id)
        {
            var existedProcess = await this.FindFirstByTicketStatusNameAndId(name, Id);
            return existedProcess != null;
        }

        public async Task<TTicketStatus> FindFirstByTicketStatusNameAndId(string name, long Id)
        {
            var user = await this.Connection.FirstOrDefaultAsync<TTicketStatus>(i => i.Name == name && i.Id != Id);
            return user;
        }

        public async Task Delete(long id)
        {
            try
            {
                this.StartTransaction();
                var entity = await this.Connection.FirstOrDefaultAsync<TTicketStatus>(i => i.Id == id);
                if (entity != null)
                    await this.Connection.DeleteAsync<TTicketStatus>(entity);
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

        public async Task<string> UsedOrNotInWorkOrder(long id)
        {

            string returnValue = "";
            var sqlQuery = new JoinSqlBuilder<TTicketStatus, IWorkOrder>(this.Connection.DialectProvider)
                                     .Join<TTicketStatus, IWorkOrder>(i => i.Id, i => i.RequestTypeId)
                                     .Select<IWorkOrder>(p => new { Id = p.Id })
                                     .Where<IWorkOrder>(ri => ri.RequestTypeId == id)
                                     .ToSql();

            var result = await this.Connection.QueryFirstOrDefaultAsync<IWorkOrder>(sqlQuery, new { p_0 = id });
            if (result != null)
            {
                returnValue = result.Id.ToString();
            }
            return returnValue;
        }
    }
}
