
using Ticket.DataAccess.Common;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

using System.Data;

using SimpleStack.Orm;
using Ticket.Base.Repositories;
using Ticket.Base.Entities;
using System.Linq.Expressions;

namespace Ticket.BusinessLogic.Common
{
    public abstract class BaseRepository<T> : IRepository<T>, IDepRepository
        where T : class, IEntity
    {
        private string _connectionString;

        private DatabaseContext _dbContext;
        public DatabaseContext DbContext
        {
            get { return _dbContext; }
            private set { _dbContext = value; }
        }

        protected OrmConnection Connection
        {
            get
            {
                return this._dbContext.Connection;
            }
        }
        
        public IUser LoggedUser { get; private set; }

        public BaseRepository(BaseValidationErrorCodes errorCodes, DatabaseContext dbContext, IUser loggedUser)
        {
            this.ErrorCodes = errorCodes; // (BaseValidationErrorCodes)ServiceProvider.GetService(typeof(BaseValidationErrorCodes));
            this.DbContext = dbContext; // (DatabaseContext)ServiceProvider.GetService(typeof(DatabaseContext));
            this.LoggedUser = loggedUser; // (IUser)ServiceProvider.GetService(typeof(IUser));
        }

        public virtual BaseValidationErrorCodes ErrorCodes
        {
            get; protected set;
        }

        public bool TransInitialized { get; private set; }

        public IDbTransaction CurrentTransaction { get; private set; }

        public IDbTransaction StartTransaction()
        {
            this.CurrentTransaction = null;
            this.TransInitialized = false;
            /* if there is no existing transaction going on*/
            this.CurrentTransaction = this._dbContext.CurrentTransaction;

            if (this.CurrentTransaction == null)
            {
                /* so this transaction is initialized by this function 
                 * commit it after all CUD is done
                 */
                this.CurrentTransaction = this._dbContext.BeginTransaction();
                this.TransInitialized = true;
            }

            return this.CurrentTransaction;
        }

        public void CommitTransaction()
        {
            /* commit the above database changes if transaction initialized by this function*/
            if (this.TransInitialized && this.CurrentTransaction != null)
            {
                this.CurrentTransaction.Commit();
            }
        }

        public virtual async Task<object> FindById(long id)
        {
            var t = await this.Connection.FirstOrDefaultAsync<T>(i => i.Id == id);
            return t;
        }

        public virtual async Task<T> AddNew(T entity)
        {
            this.SetUserStamp(entity, false);
            //this.SetLabChild(entity);

            long newId = await this.DbContext.InsertAsync<long, T>(entity);
            entity = await this.FindById(newId) as T;
            return entity;
        }

        public abstract Task<bool> Exists(string name);

        private void SetUpdateFields(T entity)
        {
            this.SetVersion(entity);
            this.SetUserStamp(entity, true);
        }

        public virtual async Task<bool> Update(T entity)
        {
            this.SetUpdateFields(entity);
            await this.Connection.UpdateAsync<T>(entity);
            return true;
        }

        public virtual async Task<bool> Update<TKey>(T entity, Expression<Func<T, TKey>> onlyFields)
        {
            this.SetUpdateFields(entity);
            await this.Connection.UpdateAsync(entity, onlyFields);
            return true;
        }

        public abstract Task<IEnumerable<IValidationResult>> ValidateEntityToAdd(T entity);

        public virtual Task<IEnumerable<IValidationResult>> ValidateEntityToUpdate(T entity)
        {
            return Task.FromResult<IEnumerable<IValidationResult>>(new List<IValidationResult>());
        }

        public virtual Task<IEnumerable<IValidationResult>> ValidateEntityToCheckExists(T entity)
        {
            return Task.FromResult<IEnumerable<IValidationResult>>(new List<IValidationResult>());
        }

        public virtual Task<IEnumerable<IValidationResult>> ValidateEntityToCheckExistsAtUpdate(T entity)
        {
            return Task.FromResult<IEnumerable<IValidationResult>>(new List<IValidationResult>());
        }

        public virtual Task<IEnumerable<IValidationResult>> ValidateEntityToUpdate(T entityNew, T entityDb)
        {
            IList<IValidationResult> validations = new List<IValidationResult>();
            var compareResult = this.CompareVersions(entityNew, entityDb);
            if (compareResult.ErrorCode > 0)
                validations.Add(compareResult);

            return Task.FromResult(validations.AsEnumerable<IValidationResult>());
        }

        public virtual async Task<IEnumerable<IValidationResult>> ValidateEntity(T entity)
        {
            IList<ValidationResult> validations = new List<ValidationResult>();
            Validator.TryValidateObject(entity, new ValidationContext(entity), validations, true);

            var errors = (from x in validations
                          select new ValidationCodeResult(x.ErrorMessage, x.MemberNames)).ToList<IValidationResult>();

            return errors;
        }

        protected Task ThrowEntityException(IEnumerable<IValidationResult> validationErrors, string message = "Record rejected due to following errors -")
        {
            throw new EntityValidationException(message, validationErrors);
        }

        protected Task ThrowEntityException(ValidationCodeResult validationError, string message = "Record rejected due to following errors -")
        {
            throw new EntityValidationException(message, new List<IValidationResult>() { validationError });
        }

        private void SetUserStamp(T entity, bool isUpdate)
        {
            if (entity is IStamp)
            {
                IStamp stamp = entity as IStamp;
                if (!isUpdate)
                {
                    stamp.CreatedBy = LoggedUser.Id;
                    stamp.CreatedDate = DateTime.Now;
                }
                stamp.ModifiedBy = LoggedUser.Id;
                stamp.ModifiedDate = DateTime.Now;
            }
            else if (entity is ICreatedStamp && !isUpdate)
            {
                ICreatedStamp stamp = entity as ICreatedStamp;

                stamp.CreatedBy = LoggedUser.Id;
                stamp.CreatedDate = DateTime.Now;
            }
        }

        private void SetVersion(T entity)
        {
            if (entity is IVersioned)
            {
                IVersioned versioned = entity as IVersioned;
                versioned.Version = versioned.Version + 1;
            }
        }

        public IValidationResult CompareVersions(T entityNew, T entityDb)
        {
            if (!(entityDb is IVersioned))
            {
                throw new Exception("Entity is not versioned entity");
            }

            ValidationCodeResult validationCode = new ValidationCodeResult("No Error Found", 0);

            var dbVersion = entityDb as IVersioned;
            var newVersion = entityNew as IVersioned;

            if (dbVersion.Version > newVersion.Version)
            {
                validationCode = new ValidationCodeResult(ErrorCodes[(int)EnumErrorBaseCode.NewVersionExists]);
            }
            else if (dbVersion.Version < newVersion.Version)
            {
                validationCode = new ValidationCodeResult(ErrorCodes[(int)EnumErrorBaseCode.VersionMismatched]);
            }

            return validationCode;
        }
    }
}

