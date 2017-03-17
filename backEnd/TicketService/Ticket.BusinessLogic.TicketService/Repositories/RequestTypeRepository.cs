
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
    public class RequestTypeRepository<TRequestType, TProcessRequestType, TWorkOrder, TProcess> : ModuleBaseRepository<TRequestType>, IRequestTypeRepository
        where TRequestType : class, IRequestType, new()
        where TProcessRequestType : class, IProcessRequestType, new()
        where TWorkOrder : class, IWorkOrder, new()
        where TProcess : class, IProcess, new()

    {
        public RequestTypeRepository(BaseValidationErrorCodes errorCodes, DatabaseContext dbContext, IUser loggedUser)
            : base(errorCodes, dbContext, loggedUser)
        {
        }


        public async Task<IRequestType> AddNew(IRequestType entity)
        {
            TRequestType tEntity = entity as TRequestType;

            var errors = await this.ValidateEntityToCheckExists(tEntity);
            if (errors.Count() > 0)
                await this.ThrowEntityException(errors);

            try
            {
                this.StartTransaction();
                var savedEntity = await base.AddNew(entity as TRequestType);
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

        public async Task<IRequestType> Update(IRequestType entity)
        {
            TRequestType tEntity = entity as TRequestType;

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

        public async Task<IRequestType> ChangeActiveState(long requestTypeId, bool isEnabled, long modifiedById)
        {
            try
            {
                var requestType = await base.FindById(requestTypeId) as TRequestType;
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


        public async Task<IEnumerable<IRequestType>> FindAllRequestTypes(bool enabledRequired)
        {
            if (enabledRequired)
            {
                return (await this.Connection.SelectAsync<TRequestType>()).OrderByDescending(i => i.Id).Where(i => i.Enabled == true);
            }
            else
            {
                return (await this.Connection.SelectAsync<TRequestType>()).OrderByDescending(i => i.Id);
            }

        }


        public async Task<IPaginationQuery<IRequestType>> FindAllRequestTypesList()
        {
            string orderBy = " order by id desc";
            var caseInfo = await this.Connection.QueryPagination<TRequestType>(null, "", "", null, null, null, null, null);

            return new PaginationQuery<IRequestType>
            {
                Data = caseInfo.Data,
                TotalRecords = caseInfo.TotalRecords
            };

        }


        public async Task<IEnumerable<IRequestType>> GetRequestTypeByProcessId(long processId)
        {

            var sqlQuery = new JoinSqlBuilder<TRequestType, TProcessRequestType>(this.Connection.DialectProvider)
                               .Join<TProcessRequestType, TRequestType>(i => i.RequestTypeId, i => i.Id)
                               .SelectAll<TRequestType>()
                                .Where<TProcessRequestType>(ri => ri.ProcessId == processId)
                               .ToSql();

            var result = await this.Connection.QueryAsync<TRequestType>(sqlQuery, new { p_0 = processId });
            return result;
        }

        public override async Task<IEnumerable<IValidationResult>> ValidateEntityToCheckExists(TRequestType entity)
        {
            ICollection<IValidationResult> errors = (await this.ValidateEntity(entity)).ToList();
            if (await this.Exists(entity.Name))
                errors.Add(new ValidationCodeResult(ErrorCodes[EnumErrorCode.RequesTypeExists, entity.Name]));

            return errors;
        }

        public override async Task<bool> Exists(string name)
        {
            var existedProcess = await this.FindFirstByProcessName(name);
            return existedProcess != null;
        }

        public async Task<TRequestType> FindFirstByProcessName(string name)
        {
            var user = await this.Connection.FirstOrDefaultAsync<TRequestType>(i => i.Name == name);
            return user;
        }


        public override async Task<IEnumerable<IValidationResult>> ValidateEntityToCheckExistsAtUpdate(TRequestType entity)
        {
            ICollection<IValidationResult> errors = (await this.ValidateEntity(entity)).ToList();
            if (await this.ExistByNameAndId(entity.Name, entity.Id))
                errors.Add(new ValidationCodeResult(ErrorCodes[EnumErrorCode.RequesTypeExists, entity.Name]));

            return errors;
        }

        public async Task<bool> ExistByNameAndId(string name, long Id)
        {
            var existedProcess = await this.FindFirstByProcessNameAndId(name, Id);
            return existedProcess != null;
        }

        public async Task<TRequestType> FindFirstByProcessNameAndId(string name, long Id)
        {
            var user = await this.Connection.FirstOrDefaultAsync<TRequestType>(i => i.Name == name && i.Id != Id);
            return user;
        }

        public async Task Delete(long id)
        {
            try
            {
                ICollection<IValidationResult> errors = new List<IValidationResult>();

                if (await this.UsedOrNotInProcess(id) != "")
                {
                    errors.Add(new ValidationCodeResult(ErrorCodes[EnumErrorCode.EntityUsedInProcess]));
                }
                if (await this.UsedOrNotInWorkOrder(id) != "")
                {
                    errors.Add(new ValidationCodeResult(ErrorCodes[EnumErrorCode.EntityUsedInWorkOrder]));
                }

                if (errors.Count() > 0)
                    await this.ThrowEntityException(errors);

                this.StartTransaction();
                var entity = await this.Connection.FirstOrDefaultAsync<TRequestType>(i => i.Id == id);
                await this.Connection.DeleteAsync<TRequestType>(entity);
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

        public async Task<string> UsedOrNotInProcess(long id)
        {
            string returnValue = "";
            var sqlQuery = new JoinSqlBuilder<TRequestType, TProcessRequestType>(this.Connection.DialectProvider)
                                     .Join<TRequestType, TProcessRequestType>(i => i.Id, i => i.RequestTypeId)
                                     .Join<TProcessRequestType, TProcess>(i => i.ProcessId, i => i.Id)
                                     .Select<TProcess>(p => new { Name = p.Name })
                                     .Where<TProcessRequestType>(ri => ri.RequestTypeId == id)
                                     .ToSql();

            var result = await this.Connection.QueryFirstOrDefaultAsync<TProcess>(sqlQuery, new { p_0 = id });
            if (result != null)
            {
                returnValue = result.Name;
            }
            return returnValue;
        }

        public async Task<string> UsedOrNotInWorkOrder(long id)
        {
            string returnValue = "";
            try
            {

                var sqlQuery = new JoinSqlBuilder<TRequestType, TWorkOrder>(this.Connection.DialectProvider)
                                         .Join<TRequestType, TWorkOrder>(i => i.Id, i => i.RequestTypeId)
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
