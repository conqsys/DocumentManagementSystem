using Ticket.DataAccess.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ticket.Base.Entities;
using Ticket.DataAccess.TicketService;
using Ticket.Base.Repositories;
using Npgsql;

namespace Ticket.BusinessLogic.TicketService
{
    public class RoleRepository<TRole> : ModuleBaseRepository<TRole>, IRoleRepository
        where TRole : class, IRole, new()
    {

        public RoleRepository(BaseValidationErrorCodes errorCodes,
            DatabaseContext dbContext,
            IUser loggedUser
           )
            : base(errorCodes, dbContext, loggedUser)
        {
        }

        public async Task<IRole> AddNew(IRole entity)
        {
            TRole tEntity = entity as TRole;

            var errors = await this.ValidateEntityToAdd(tEntity);
            if (errors.Count() > 0)
                await this.ThrowEntityException(errors);

            try
            {
                this.StartTransaction();
                /* save the role first*/
                var savedEntity = await base.AddNew(tEntity);
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

        public async Task<bool> Update(IRole entity)
        {
            try
            {
                TRole tEntity = entity as TRole;
                TRole role = await base.FindById(entity.Id) as TRole;
                var errors = await this.ValidateEntityToUpdate(tEntity);
                if (errors.Count() > 0)
                    await this.ThrowEntityException(errors);

                this.StartTransaction();

                await base.Update(tEntity, x => new
                {
                    x.Name,
                    x.Description,
                    x.ModifiedBy,
                    x.ModifiedDate,
                });

                this.CommitTransaction();
                return true;
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
            var role = await base.FindById(id) as TRole;
            return role;
        }

        public async Task<IEnumerable<IRole>> FindAllRoles()
        {
            return (await this.Connection.SelectAsync<TRole>()).OrderByDescending(i => i.Id);
        }

        public override async Task<bool> Exists(string description)
        {
            var role = await this.Connection.FirstOrDefaultAsync<TRole>(i => i.Description == description);
            return role != null;
        }

        public override async Task<IEnumerable<IValidationResult>> ValidateEntityToAdd(TRole entity)
        {
            ICollection<IValidationResult> errors = (await this.ValidateEntity(entity)).ToList();

            /* check if role already exists then same role, theow exception if true*/
            if (await this.Exists(entity.Description))
                errors.Add(new ValidationCodeResult(ErrorCodes[EnumErrorCode.RoleExists, entity.Description]));

            return errors;
        }

        public override async Task<IEnumerable<IValidationResult>> ValidateEntityToUpdate(TRole entityNew, TRole entityDb)
        {
            var baseResults = await base.ValidateEntityToUpdate(entityNew, entityDb);
            var commonResults = await this.ValidateEntity(entityNew);
            baseResults = commonResults.Union(baseResults);
            return baseResults;
        }

        public override async Task<IEnumerable<IValidationResult>> ValidateEntity(TRole entity)
        {
            return await base.ValidateEntity(entity);
        }

        public async Task Delete(long id)
        {
            await this.Connection.DeleteAllAsync<TRole>(i => i.Id == id);
        }
    }
}
