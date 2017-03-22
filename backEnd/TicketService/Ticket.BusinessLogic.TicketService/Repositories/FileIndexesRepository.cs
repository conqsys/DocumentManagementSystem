
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
    public class FileIndexesRepository<TFileIndexes> : ModuleBaseRepository<TFileIndexes>, IFileIndexesRepository
        where TFileIndexes : class, IFileIndexes, new()
        


    {
        
        public FileIndexesRepository(BaseValidationErrorCodes errorCodes, DatabaseContext dbContext, IUser loggedUser,
            IUserQueueRepository _userQueueRepository)
            : base(errorCodes, dbContext, loggedUser)
        {
        }


        public async Task<IFileIndexes> AddNew(IFileIndexes entity)
        {
            TFileIndexes tEntity = entity as TFileIndexes;

            var errors = await this.ValidateEntityToCheckExists(tEntity);
            if (errors.Count() > 0)
                await this.ThrowEntityException(errors);

            try
            {
                this.StartTransaction();
                var savedEntity = await base.AddNew(entity as TFileIndexes);
              
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

        public async Task<IFileIndexes> Update(IFileIndexes entity)
        {
            TFileIndexes tEntity = entity as TFileIndexes;

            var errors = await this.ValidateEntityToCheckExistsAtUpdate(tEntity);
            if (errors.Count() > 0)
                await this.ThrowEntityException(errors);

            try
            {
                this.StartTransaction();
                await base.Update(tEntity, x => new
                {
                    x.IndexName,
                    x.IndexType,
                    x.ListValue,
                    x.DefaultValue
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

        public async Task<IEnumerable<IFileIndexes>> GetListOfFileIndexes()
        {
            
            var fileIndexesList = (await this.Connection.SelectAsync<TFileIndexes>()).OrderBy(i => i.Id);


            return fileIndexesList;
        }
        public async Task Delete(long id)
        {
            try
            {
                ICollection<IValidationResult> errors = new List<IValidationResult>();



                if (errors.Count() > 0)
                    await this.ThrowEntityException(errors);

                this.StartTransaction();
                var entity = await this.Connection.FirstOrDefaultAsync<TFileIndexes>(i => i.Id == id);
                await this.Connection.DeleteAsync<TFileIndexes>(entity);
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
