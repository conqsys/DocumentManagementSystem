
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
    public class ClientRepository<TClient, TWorkOrder> : ModuleBaseRepository<TClient>, IClientRepository
        where TClient : class, IClient, new()
        where TWorkOrder : class, IWorkOrder, new()
    {
        private IUserRepository _userRepository;
        private IRoleRepository _roleRepository;
        private IClientFacilityRepository _clientFacilityRepository;
        private EncryptionProvider _encryptionProvider;
        public ClientRepository(BaseValidationErrorCodes errorCodes,
                                                        DatabaseContext dbContext,
                                                        IUser loggedUser,
                                                        IUserRepository userRepository,
                                                        IRoleRepository roleRepository,
                                                         EncryptionProvider encryptionProvider,
                                                         IClientFacilityRepository clientFacilityRepository
                                                         )
            : base(errorCodes, dbContext, loggedUser)
        {
            this._userRepository = userRepository;
            this._roleRepository = roleRepository;
            this._encryptionProvider = encryptionProvider;
            this._clientFacilityRepository = clientFacilityRepository;
        }


        public async Task<IClient> AddNew(IClient entity)
        {
            TClient tEntity = entity as TClient;

            var errors = await this.ValidateEntityToCheckExists(tEntity);
            if (errors.Count() > 0)
                await this.ThrowEntityException(errors);

            try
            {
                this.StartTransaction();
                var savedEntity = await base.AddNew(entity as TClient);
                long clientId = savedEntity.Id;
                await this._clientFacilityRepository.DeleteClientFacilityByClientId(clientId);
                await this.SaveClientFacilities(tEntity, clientId);
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

        public async Task<bool> SaveClientFacilities(IClient tEntity, long clientId)
        {
            foreach (var facility in tEntity.ClientFacilities)
            {
                IClientFacility clientFacility = facility;
                clientFacility.ClientId = clientId;
                clientFacility.FacilityId = facility.Id;
                var savedUser = await this._clientFacilityRepository.AddNew(facility);
            }
            return true;
        }

        public async Task<IClient> Update(IClient entity)
        {
            TClient tEntity = entity as TClient;
            var errors = await this.ValidateEntityToCheckExistsAtUpdate(tEntity);
            if (errors.Count() > 0)
                await this.ThrowEntityException(errors);
            try
            {
                this.StartTransaction();
                await base.Update(tEntity, x => new
                {
                    x.ClientName,
                    x.Description,
                    x.Enabled,
                    x.LoginEnabled,
                    x.ClientAcronym,
                    x.RequestTypeId,
                    x.ModifiedBy,
                    x.ModifiedDate,
                    x.AccountNumber
                });
                await this._clientFacilityRepository.DeleteClientFacilityByClientId(tEntity.Id);
                await this.SaveClientFacilities(tEntity, tEntity.Id);
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

        public async Task<IClient> ChangeActiveState(long clientId, bool isEnabled, long modifiedById)
        {
            try
            {
                var client = await base.FindById(clientId) as TClient;
                if (client != null)
                {
                    client.Enabled = isEnabled;
                    await base.Update(client, x => new
                    {
                        x.Enabled,
                        x.ModifiedBy,
                        x.ModifiedDate
                    });
                }
                return client;
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
            var client = await base.FindById(id) as TClient;
            IEnumerable<IUser> users = await this._userRepository.FindUserByAccountId(id);
            IEnumerable<IClientFacility> clientFacilities = await this._clientFacilityRepository.FindClientFacilitiesByClientId(id);

            if (client != null)
            {
                client.ClientUsers = users;
                client.ClientFacilities = clientFacilities;
            }
            return client;
        }


        public async Task<IEnumerable<IClient>> FindAllClients(bool enabledRequired)
        {
            if (enabledRequired)
            {
                return (await this.Connection.SelectAsync<TClient>()).OrderByDescending(i => i.Id).Where(i => i.Enabled == true);
            }
            else
            {
                return (await this.Connection.SelectAsync<TClient>()).OrderByDescending(i => i.Id);
            }

        }

        public override async Task<IEnumerable<IValidationResult>> ValidateEntityToCheckExists(TClient entity)
        {
            ICollection<IValidationResult> errors = (await this.ValidateEntity(entity)).ToList();
            if (await this.CheckClientNameExists(entity.ClientName))
                errors.Add(new ValidationCodeResult(ErrorCodes[EnumErrorCode.ClientNameExists, entity.ClientName]));

            if (await this.CheckClientAcronymExists(entity.ClientAcronym))
                errors.Add(new ValidationCodeResult(ErrorCodes[EnumErrorCode.ClientAcroymnExists, entity.ClientName]));

            return errors;
        }

        public async Task<bool> CheckClientNameExists(string name)
        {
            var existedClient = await this.FindFirstByClientName(name);
            return existedClient != null;
        }

        public async Task<bool> CheckClientAcronymExists(string acronym)
        {
            var existedClient = await this.FindFirstByClientAcronym(acronym);
            return existedClient != null;
        }

        public async Task<TClient> FindFirstByClientName(string name)
        {
            var client = await this.Connection.FirstOrDefaultAsync<TClient>(i => i.ClientName == name);
            return client;
        }

        public async Task<TClient> FindFirstByClientAcronym(string acronym)
        {
            var client = await this.Connection.FirstOrDefaultAsync<TClient>(i => i.ClientAcronym == acronym);
            return client;
        }


        public override async Task<IEnumerable<IValidationResult>> ValidateEntityToCheckExistsAtUpdate(TClient entity)
        {
            ICollection<IValidationResult> errors = (await this.ValidateEntity(entity)).ToList();

            if (await this.CheckExistByNameAndId(entity.ClientName, entity.Id))
                errors.Add(new ValidationCodeResult(ErrorCodes[EnumErrorCode.ClientNameExists, entity.ClientName]));

            if (await this.CheckExistByAcroynmAndId(entity.ClientAcronym, entity.Id))
                errors.Add(new ValidationCodeResult(ErrorCodes[EnumErrorCode.ClientAcroymnExists, entity.ClientName]));


            return errors;
        }

        public async Task<IEnumerable<IValidationResult>> ValidateEntityToDelete(TClient entity)
        {
            ICollection<IValidationResult> errors = (await this.ValidateEntity(entity)).ToList();

            if (await this.CheckExistInWorkOrder(entity.Id))
                errors.Add(new ValidationCodeResult(ErrorCodes[EnumErrorCode.ClientExistsInWorkOrder]));
            return errors;
        }

        public async Task<bool> CheckExistByNameAndId(string name, long Id)
        {
            var existedProcess = await this.FindFirstByCientNameAndId(name, Id);
            return existedProcess != null;
        }

        public async Task<bool> CheckExistInWorkOrder(long id)
        {
            var existedWorkOrder = await this.Connection.FirstOrDefaultAsync<TWorkOrder>(i => i.ClientId == id);
            return existedWorkOrder != null;
        }

        public async Task<TClient> FindFirstByCientNameAndId(string name, long Id)
        {
            var user = await this.Connection.FirstOrDefaultAsync<TClient>((i => i.ClientName == name && i.Id != Id));
            return user;
        }


        public async Task<bool> CheckExistByAcroynmAndId(string acroynm, long Id)
        {
            var existedProcess = await this.FindFirstByCientAcroynmAndId(acroynm, Id);
            return existedProcess != null;
        }

        public async Task<TClient> FindFirstByCientAcroynmAndId(string acroynm, long Id)
        {
            var user = await this.Connection.FirstOrDefaultAsync<TClient>((i => i.ClientAcronym == acroynm && i.Id != Id));
            return user;
        }

        public async Task Delete(long id)
        {
            try
            {
                this.StartTransaction();
                var entity = await this.Connection.FirstOrDefaultAsync<TClient>(i => i.Id == id);
                await this.Connection.DeleteAsync<TClient>(entity);
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
