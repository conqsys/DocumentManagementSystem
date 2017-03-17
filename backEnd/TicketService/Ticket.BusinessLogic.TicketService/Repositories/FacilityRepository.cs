

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
    public class FacilityRepository<TFacility, TClientFacility, TClient, TWorkOrder> : ModuleBaseRepository<TFacility>, IFacilityRepository
        where TFacility : class, IFacility, new()
       where TClientFacility : class, IClientFacility, new()
        where TClient : class, IClient, new()
        where TWorkOrder : class, IWorkOrder, new()
    {
        public FacilityRepository(BaseValidationErrorCodes errorCodes, DatabaseContext dbContext, IUser loggedUser)
            : base(errorCodes, dbContext, loggedUser)
        {
        }


        public async Task<IFacility> AddNew(IFacility entity)
        {
            TFacility tEntity = entity as TFacility;

            var errors = await this.ValidateEntityToCheckExists(tEntity);
            if (errors.Count() > 0)
                await this.ThrowEntityException(errors);

            try
            {
                this.StartTransaction();
                var savedEntity = await base.AddNew(entity as TFacility);
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

        public async Task<IFacility> Update(IFacility entity)
        {
            TFacility tEntity = entity as TFacility;

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

        public async Task<IFacility> ChangeActiveState(long requestTypeId, bool isEnabled, long modifiedById)
        {
            try
            {
                var requestType = await base.FindById(requestTypeId) as TFacility;
                if (requestType != null)
                {
                    requestType.Enabled = isEnabled;
                    await base.Update(requestType, x => new
                    {
                        x.Enabled,
                        x.ModifiedBy,
                        x.ModifiedDate
                    });
                }
                return requestType;
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


        public async Task<IEnumerable<IFacility>> FindAllFacilities(bool enabledRequired)
        {
            if (enabledRequired)
            {
                return (await this.Connection.SelectAsync<TFacility>()).OrderByDescending(i => i.Id).Where(i => i.Enabled == true);
            }
            else
            {
                return (await this.Connection.SelectAsync<TFacility>()).OrderByDescending(i => i.Id);
            }
        }

        public async Task<IEnumerable<IFacility>> GetFacilitiesByAccountId(long accountId)
        {

            var sqlQuery = new JoinSqlBuilder<TFacility, TClientFacility>(this.Connection.DialectProvider)
                               .Join<TClientFacility, TFacility>(i => i.FacilityId, i => i.Id)
                               .SelectAll<TFacility>()
                                .Where<TClientFacility>(ri => ri.ClientId == accountId)
                               .ToSql();

            var result = await this.Connection.QueryAsync<TFacility>(sqlQuery, new { p_0 = accountId });
            return result;
        }


        public override async Task<IEnumerable<IValidationResult>> ValidateEntityToCheckExists(TFacility entity)
        {
            ICollection<IValidationResult> errors = (await this.ValidateEntity(entity)).ToList();
            if (await this.Exists(entity.Name))
                errors.Add(new ValidationCodeResult(ErrorCodes[EnumErrorCode.FacilityNameExists, entity.Name]));

            return errors;
        }

        public override async Task<bool> Exists(string name)
        {
            var existedClient = await this.FindFirstByName(name);
            return existedClient != null;
        }

        public async Task<TFacility> FindFirstByName(string name)
        {
            var client = await this.Connection.FirstOrDefaultAsync<TFacility>(i => i.Name == name);
            return client;
        }

        public override async Task<IEnumerable<IValidationResult>> ValidateEntityToCheckExistsAtUpdate(TFacility entity)
        {
            ICollection<IValidationResult> errors = (await this.ValidateEntity(entity)).ToList();
            if (await this.ExistByNameAndId(entity.Name, entity.Id))
                errors.Add(new ValidationCodeResult(ErrorCodes[EnumErrorCode.FacilityNameExists, entity.Name]));

            return errors;
        }

        public async Task<bool> ExistByNameAndId(string name, long Id)
        {
            var existedProcess = await this.FindFirstByProcessNameAndId(name, Id);
            return existedProcess != null;
        }

        public async Task<TFacility> FindFirstByProcessNameAndId(string name, long Id)
        {
            var user = await this.Connection.FirstOrDefaultAsync<TFacility>(i => i.Name == name && i.Id != Id);
            return user;
        }

        public async Task Delete(long id)
        {
            try
            {
                ICollection<IValidationResult> errors = new List<IValidationResult>();

                if (await this.UsedOrNotInClient(id) != "")
                {
                    errors.Add(new ValidationCodeResult(ErrorCodes[EnumErrorCode.EntityUsedInClient]));
                }
                if (await this.UsedOrNotInWorkOrder(id) != "")
                {
                    errors.Add(new ValidationCodeResult(ErrorCodes[EnumErrorCode.EntityUsedInWorkOrder]));
                }

                if (errors.Count() > 0)
                    await this.ThrowEntityException(errors);

                this.StartTransaction();
                var entity = await this.Connection.FirstOrDefaultAsync<TFacility>(i => i.Id == id);
                await this.Connection.DeleteAsync<TFacility>(entity);
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

        public async Task<string> UsedOrNotInClient(long id)
        {
            string returnValue = "";
            var sqlQuery = new JoinSqlBuilder<TFacility, TClientFacility>(this.Connection.DialectProvider)
                                     .Join<TFacility, TClientFacility>(i => i.Id, i => i.FacilityId)
                                     .Join<TClientFacility, TClient>(i => i.ClientId, i => i.Id)
                                     .Select<TClient>(p => new { Name = p.ClientName })
                                     .Where<TClientFacility>(ri => ri.FacilityId == id)
                                     .ToSql();

            var result = await this.Connection.QueryFirstOrDefaultAsync<TClient>(sqlQuery, new { p_0 = id });
            if (result != null)
            {
                returnValue = result.ClientName;
            }
            return returnValue;
        }

        public async Task<string> UsedOrNotInWorkOrder(long id)
        {
            string returnValue = "";
            try
            {

                var sqlQuery = new JoinSqlBuilder<TFacility, TWorkOrder>(this.Connection.DialectProvider)
                                         .Join<TFacility, TWorkOrder>(i => i.Id, i => i.FacilityId)
                                         .Select<TWorkOrder>(p => new { Id = p.Id })
                                         .Where<TWorkOrder>(ri => ri.RequestTypeId == id)
                                         .ToSql();

                var result = await this.Connection.QueryFirstOrDefaultAsync<TWorkOrder>(sqlQuery, new { p_0 = id });
                if (result != null)
                {
                    returnValue = result.Id.ToString();
                }
            }
            catch (PostgresException ex)
            {
                throw new EntityUpdateException(ex);
            }
            catch (Exception ex)
            {

                throw;
            }

            return returnValue;
        }

    }
}
