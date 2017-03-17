
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
    public class ImportFileRepository<TImportFile> : ModuleBaseRepository<TImportFile>, IImportFileRepository
        where TImportFile : class, IImportFile, new()


    {
        public ImportFileRepository(BaseValidationErrorCodes errorCodes, DatabaseContext dbContext, IUser loggedUser)
            : base(errorCodes, dbContext, loggedUser)
        {
        }


        public async Task<IImportFile> AddNew(IImportFile entity)
        {
            TImportFile tEntity = entity as TImportFile;

            var errors = await this.ValidateEntityToCheckExists(tEntity);
            if (errors.Count() > 0)
                await this.ThrowEntityException(errors);

            try
            {
                this.StartTransaction();
                var savedEntity = await base.AddNew(entity as TImportFile);
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

        public async Task<IImportFile> Update(IImportFile entity)
        {
            TImportFile tEntity = entity as TImportFile;

            var errors = await this.ValidateEntityToCheckExistsAtUpdate(tEntity);
            if (errors.Count() > 0)
                await this.ThrowEntityException(errors);

            try
            {
                this.StartTransaction();
                await base.Update(tEntity, x => new
                {
                    x.BatchNumber,
                    x.BatchType,
                    x.ClientId,
                    x.Misc,
                    x.Status,
                    x.UploadDate,
                    x.Type,
                    x.UploadImage
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
                ICollection<IValidationResult> errors = new List<IValidationResult>();



                if (errors.Count() > 0)
                    await this.ThrowEntityException(errors);

                this.StartTransaction();
                var entity = await this.Connection.FirstOrDefaultAsync<TImportFile>(i => i.Id == id);
                await this.Connection.DeleteAsync<TImportFile>(entity);
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
