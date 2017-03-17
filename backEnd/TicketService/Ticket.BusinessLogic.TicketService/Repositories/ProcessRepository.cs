

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
    public class ProcessRepository<TProcess> : ModuleBaseRepository<TProcess>, IProcessRepository
        where TProcess : class, IProcess, new()
    {

        private IProcessRequestTypeRepository _processRequestTypeRepository;
        public ProcessRepository(BaseValidationErrorCodes errorCodes, DatabaseContext dbContext, IUser loggedUser, IProcessRequestTypeRepository processRequestTypeRepository)
            : base(errorCodes, dbContext, loggedUser)
        {
            this._processRequestTypeRepository = processRequestTypeRepository;
        }


        public async Task<IProcess> AddNew(IProcess entity)
        {
            TProcess tEntity = entity as TProcess;

            var errors = await this.ValidateEntityToCheckExists(tEntity);
            if (errors.Count() > 0)
                await this.ThrowEntityException(errors);

            try
            {
                this.StartTransaction();
                var savedEntity = await base.AddNew(entity as TProcess);
                long processId = savedEntity.Id;
                await this._processRequestTypeRepository.DeleteProcessRequestTypeByProcessId(processId);
                await this.SaveProcessRequestTypes(tEntity, processId);
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


        public async Task<bool> SaveProcessRequestTypes(IProcess tEntity, long processId)
        {
            foreach (var requestType in tEntity.ProcessRequestTypes)
            {
                IProcessRequestType processType = requestType;
                processType.ProcessId = processId;
                processType.RequestTypeId = requestType.Id;
                var savedUser = await this._processRequestTypeRepository.AddNew(requestType);
            }
            return true;
        }


        public async Task<IProcess> Update(IProcess entity)
        {
            TProcess tEntity = entity as TProcess;

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

                await this._processRequestTypeRepository.DeleteProcessRequestTypeByProcessId(tEntity.Id);
                await this.SaveProcessRequestTypes(tEntity, tEntity.Id);

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

        public async Task<IProcess> ChangeActiveState(long processId, bool isEnabled, long modifiedById)
        {
            try
            {
                var requestType = await base.FindById(processId) as TProcess;
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

        public override async Task<object> FindById(long id)
        {
            var process = await base.FindById(id) as TProcess;
            IEnumerable<IProcessRequestType> processRequestTypes = await this._processRequestTypeRepository.FindProcessRequestTypeByProcessId(id);
            if (process != null)
            {
                process.ProcessRequestTypes = processRequestTypes;
            }
            return process;
        }

        public async Task<IEnumerable<IProcess>> FindAllProcess(bool enabledRequired)
        {
            if (enabledRequired)
            {
                return (await this.Connection.SelectAsync<TProcess>()).OrderByDescending(i => i.Id).Where(i => i.Enabled == true);
            }
            else
            {
                return (await this.Connection.SelectAsync<TProcess>()).OrderByDescending(i => i.Id);
            }

        }

        public async Task<IEnumerable<IProcess>> GetListOfProcesses()
        {
            List<IProcessRequestType> ProcessRequestTypes = new List<IProcessRequestType>();
            List<IProcess> processesList = new List<IProcess>();
            var processes = (await this.Connection.SelectAsync<TProcess>()).OrderByDescending(i => i.Id);

            foreach (TProcess obj in processes)
            {
                Process process = new Process();
                process.Id = obj.Id;
                process.Name = obj.Name;
                process.Description = obj.Description;
                process.Enabled = obj.Enabled;
                process.CreatedBy = obj.CreatedBy;
                process.CreatedDate = obj.CreatedDate;
                process.ModifiedBy = obj.ModifiedBy;
                process.ModifiedDate = obj.ModifiedDate;
                process.ProcessRequestTypes = await this._processRequestTypeRepository.FindProcessRequestTypeByProcessId(obj.Id);
                processesList.Add(process);
            }

            return processesList;
        }

        public override async Task<IEnumerable<IValidationResult>> ValidateEntityToCheckExists(TProcess entity)
        {
            ICollection<IValidationResult> errors = (await this.ValidateEntity(entity)).ToList();
            if (await this.Exists(entity.Name))
                errors.Add(new ValidationCodeResult(ErrorCodes[EnumErrorCode.ProcessNameExists, entity.Name]));

            return errors;
        }

        public override async Task<bool> Exists(string name)
        {
            var existedClient = await this.FindFirstByName(name);
            return existedClient != null;
        }

        public async Task<TProcess> FindFirstByName(string name)
        {
            var client = await this.Connection.FirstOrDefaultAsync<TProcess>(i => i.Name == name);
            return client;
        }


        public override async Task<IEnumerable<IValidationResult>> ValidateEntityToCheckExistsAtUpdate(TProcess entity)
        {
            ICollection<IValidationResult> errors = (await this.ValidateEntity(entity)).ToList(); ;
            if (await this.ExistByNameAndId(entity.Name, entity.Id))
                errors.Add(new ValidationCodeResult(ErrorCodes[EnumErrorCode.ProcessNameExists, entity.Name]));

            return errors;
        }

        public async Task<bool> ExistByNameAndId(string name, long Id)
        {
            var existedProcess = await this.FindFirstByProcessNameAndId(name, Id);
            return existedProcess != null;
        }

        public async Task<TProcess> FindFirstByProcessNameAndId(string name, long Id)
        {
            var user = await this.Connection.FirstOrDefaultAsync<TProcess>(i => i.Name == name && i.Id != Id);
            return user;
        }

        public async Task Delete(long id)
        {
            try
            {
                this.StartTransaction();
                var entity = await this.Connection.FirstOrDefaultAsync<TProcess>(i => i.Id == id);
                if (entity != null)
                    await this.Connection.DeleteAsync<TProcess>(entity);
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

