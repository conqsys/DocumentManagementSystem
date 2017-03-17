using Ticket.DataAccess.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Ticket.Base.Entities;
using Ticket.Base.Repositories;
using Ticket.DataAccess.TicketService;
using Npgsql;
using SimpleStack.Orm;
using Ticket.BusinessLogic.Common;
using Ticket.Base.Objects;

namespace Ticket.BusinessLogic.TicketService
{
    public class UserRepository<TUser, TRole> : ModuleBaseRepository<TUser>, IUserRepository
        where TUser : class, IUser, new()
        where TRole : class, IRole, new()
    {
        private EncryptionProvider _encryptionProvider;
        private IGroupUserRepository _groupUserRepository;
        private EmailService _emailService;
        public UserRepository(BaseValidationErrorCodes errorCodes,
            DatabaseContext dbContext,
            IUser loggedUser,
            EncryptionProvider encryptionProvider,
            IGroupUserRepository groupUserRepository,
            EmailService emailService
           )
            : base(errorCodes, dbContext, loggedUser)
        {
            this._encryptionProvider = encryptionProvider;
            this._groupUserRepository = groupUserRepository;
            this._emailService = emailService;
        }

        public async Task<IUser> CreateInstance()
        {
            return new User();
        }

        public async Task<IUser> AddNew(IUser entity)
        {
            TUser tEntity = entity as TUser;

            var errors = await this.ValidateEntityToAdd(tEntity);
            if (errors.Count() > 0)
                await this.ThrowEntityException(errors);

            try
            {
                this.StartTransaction();
                tEntity.CreatedDate = DateTime.Now;
                tEntity.CreatedBy = LoggedUser.Id;
                if (tEntity.Password != "" && tEntity.Password != null)
                {
                    tEntity.Password = _encryptionProvider.Encrypt(tEntity.Password);
                }

                if (tEntity.RoleId == 1)
                { //ONshore
                    tEntity.AccountId = null;
                    tEntity.CoordinatorId = null;
                }
                else if (tEntity.RoleId == 2)
                {//OFFshore
                    tEntity.AccountId = null;
                }
                else if (tEntity.RoleId == 3)
                {//Client
                    tEntity.CoordinatorId = null;
                }

                var savedEntity = await base.AddNew(tEntity);
                long userId = savedEntity.Id;
                await this._groupUserRepository.DeleteGroupUserByUserId(userId);
                await this.SaveGroupUser(tEntity, userId);
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

        public async Task<bool> SaveGroupUser(IUser tEntity, long userId)
        {

            if (tEntity.RoleId == 1)
            {
                foreach (var user in tEntity.GroupUsers)
                {
                    IGroupUser groupUser = user;
                    groupUser.UserId = userId;
                    groupUser.GroupId = user.Id;
                    var savedUser = await this._groupUserRepository.AddNew(user);
                }
            }
            return true;
        }


        public async Task<IUser> CheckLogin(string userName, string password)
        {
            return await this.Connection.FirstOrDefaultAsync<TUser>(i => i.UserName == userName && i.Password == _encryptionProvider.Encrypt(password) && i.Enabled == true);
        }

        public async Task IncreaseAttempts(string userName)
        {
            var user = await this.Connection.FirstOrDefaultAsync<TUser>(i => i.Email == userName);
            if (user != null)
            {
                try
                {
                    // user.AuthAttempts = user.AuthAttempts + 1;
                    await this.UpdateAuthAttempts(user);
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

        private async Task UpdateAuthAttempts(TUser user)
        {
            await base.Update(user, x => new
            {
                // x.AuthAttempts,
                x.ModifiedBy,
                x.ModifiedDate
            });
        }

        public async Task<bool> CheckAuthentication(string userName)
        {
            ICollection<ValidationCodeResult> errors = new List<ValidationCodeResult>();

            try
            {
                var currentUser = await this.FindFirstByName(userName);
                if (currentUser == null)
                {
                    /* nothing will be checked if no user exists for the given username*/
                    await this.ThrowEntityException(new ValidationCodeResult(ErrorCodes[EnumErrorCode.IncorrectUserName]));
                }

                if (errors.Count > 0)
                    await this.ThrowEntityException(errors);

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

        public async Task<IUser> LogOut(long userId)
        {
            try
            {
                var user = await base.FindById(userId) as TUser;
                if (user != null)
                {
                    // user.AuthAttempts = 0;
                    await this.UpdateAuthAttempts(user);
                }
                return user;
            }
            catch
            {
                throw;
            }
        }

        public async Task<IUser> UpdatePassword(long userId, string password)
        {
            var user = await base.FindById(userId) as TUser;

            user.Password = _encryptionProvider.Encrypt(password);
            // user.DatePassword = DateTime.Now.AddDays(90);

            await base.Update(user, x => new
            {
                x.Password,
                // x.DatePassword,
                x.ModifiedBy,
                x.ModifiedDate
            });
            return user;
        }

        public async Task<IUser> Update(IUser entity)
        {
            try
            {
                TUser tEntity = entity as TUser;

                TUser user = await base.FindById(entity.Id) as TUser;
                var errors = await this.ValidateEntityToCheckExistsAtUpdate(tEntity);
                if (errors.Count() > 0)
                    await this.ThrowEntityException(errors);

                this.StartTransaction();
                tEntity.ModifiedBy = LoggedUser.Id;
                tEntity.ModifiedDate = DateTime.Now;
                if (tEntity.Password != "XXXXX" && tEntity.IsPasswordReset == true)
                {
                    tEntity.Password = _encryptionProvider.Encrypt(tEntity.Password);
                }
                else
                {
                    tEntity.Password = user.Password;
                }

                if (tEntity.RoleId == 1)
                { //ONshore
                    tEntity.AccountId = null;
                    tEntity.CoordinatorId = null;
                }
                else if (tEntity.RoleId == 2)
                {//OFFshore
                    tEntity.AccountId = null;
                }
                else if (tEntity.RoleId == 3)
                {//Client
                    tEntity.CoordinatorId = null;
                }

                await base.Update(tEntity, x => new
                {
                    x.Email,
                    x.Enabled,
                    x.UserName,
                    x.RoleId,
                    x.PhoneNumber,
                    x.CoordinatorId,
                    x.Password,
                    x.AccountId,
                    x.ModifiedBy,
                    x.ModifiedDate
                });
                await this._groupUserRepository.DeleteGroupUserByUserId(entity.Id);
                await this.SaveGroupUser(tEntity, tEntity.Id);
                this.CommitTransaction();

                return user;
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<IUser>> FindAllUsersList()
        {
            return await this.Connection.SelectAsync<TUser>();
        }

        public async Task<IEnumerable<IUserDTO>> FindAll(bool enabledRequired)
        {
            List<IUserDTO> users = new List<IUserDTO>();

            var sqlQuery = new JoinSqlBuilder<TUser, TRole>(this.Connection.DialectProvider)
                                 .Join<TRole, TUser>(i => i.Id, i => i.RoleId)
                                 .SelectAll<TUser>()
                                 .Select<TRole>(p => new { RoleName = p.Name })
                                 .ToSql();

            var result = (await this.Connection.QueryAsync<TUser>(sqlQuery)).OrderByDescending(i => i.Id);

            foreach (TUser obj in result)
            {
                UserDTO user = new UserDTO();
                user.UserId = obj.Id;
                user.UserEmail = obj.Email;
                user.UserName = obj.UserName;
                user.Enabled = obj.Enabled;
                user.PhoneNumber = obj.PhoneNumber;
                user.RoleId = obj.RoleId;
                user.CoordinatorId = obj.CoordinatorId;
                user.AccountId = obj.AccountId;
                user.RoleName = obj.RoleName;
                user.ClientName = obj.ClientName;
                user.CoordinatorId = obj.CoordinatorId;
                user.CoordinatorName = obj.CoordinatorName;
                users.Add(user);
            }
            return users;

        }

        public async Task<IEnumerable<IUser>> FindAllByRoleId(long roleId)
        {
            var users = await this.Connection.SelectAsync<TUser>(i => i.RoleId == roleId && i.Enabled == true);
            return users;
        }


        public async Task<IUser> FindFirstByName(string userName)
        {
            var user = await this.Connection.FirstOrDefaultAsync<TUser>(i => i.UserName == userName);
            return user;
        }



        public async Task<IUser> FindFirstByEmail(string email)
        {
            var user = await this.Connection.FirstOrDefaultAsync<TUser>(i => i.Email == email);
            return user;
        }

        public async Task<bool> CheckUserExistsByName(string email, string userName)
        {

            var existedUser = await this.FindFirstByName(userName);

            return existedUser != null;
        }

        public async Task<bool> CheckUserExistsByEmail(string email, string userName)
        {
            var existedUser = await this.FindFirstByEmail(email);

            return existedUser != null;
        }


        public async Task<IEnumerable<IValidationResult>> ValidateExists(string userName, ICollection<IValidationResult> errors)
        {
            var existedUser = await this.FindFirstByName(userName);
            if (existedUser != null)
            {
                errors.Add(new ValidationCodeResult(ErrorCodes[EnumErrorCode.UserNameExists, userName]));
            }
            return errors;
        }

        public override async Task<IEnumerable<IValidationResult>> ValidateEntityToAdd(TUser entity)
        {
            ICollection<IValidationResult> errors = (await this.ValidateEntity(entity)).ToList();

            /* check if user already exists then same email, theow exception if true*/
            if (await this.CheckUserExistsByName(entity.Email, entity.UserName))
                errors.Add(new ValidationCodeResult(ErrorCodes[EnumErrorCode.UserNameExists, entity.UserName]));

            if (await this.CheckUserExistsByEmail(entity.Email, entity.UserName))
                errors.Add(new ValidationCodeResult(ErrorCodes[EnumErrorCode.UserEmailExists, entity.Email]));


            return errors;
        }

        public override async Task<IEnumerable<IValidationResult>> ValidateEntityToUpdate(TUser entity)
        {
            ICollection<IValidationResult> errors = (await this.ValidateEntity(entity)).ToList();

            var user = await base.FindById(entity.Id);
            if (user == null)
            {
                errors.Add(new ValidationCodeResult(ErrorCodes[EnumErrorCode.UserDoesNotExist]));
            }

            return errors;
        }

        public async Task<IEnumerable<IValidationResult>> ValidateEntity(IUser entity)
        {
            ICollection<IValidationResult> errors = (await base.ValidateEntity(entity as TUser)).ToList();
            return errors;
        }

        public async Task<IUser> ChangeActiveState(long userId, bool isEnabled, long modifiedById)
        {
            try
            {
                var user = await base.FindById(userId) as TUser;
                if (user != null)
                {
                    user.Enabled = isEnabled;
                    await base.Update(user, x => new
                    {
                        x.Enabled,
                        x.ModifiedBy,
                        x.ModifiedDate
                    });
                }
                return user;
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

        public override async Task<IEnumerable<IValidationResult>> ValidateEntityToUpdate(TUser entityNew, TUser entityDb)
        {
            var baseResults = await base.ValidateEntityToUpdate(entityNew, entityDb);
            var commonResults = await this.ValidateEntityToUpdate(entityNew);
            baseResults = commonResults.Union(baseResults);
            return baseResults;
        }

        public async Task<IEnumerable<IUser>> FindUserByAccountId(long accountId)
        {
            var users = (await this.Connection.SelectAsync<TUser>(i => i.AccountId == accountId)).OrderBy(i => i.UserName);
            return users;
        }

        public override async Task<object> FindById(long id)
        {
            var user = await base.FindById(id) as TUser;
            if (user != null)
            {
                IEnumerable<IGroupUser> groupUsers = await this._groupUserRepository.FindGroupUsersByUserId(id);
                if (user != null)
                {
                    user.GroupUsers = groupUsers;
                }
            }
            return user;
        }

        public async Task<IEnumerable<IUser>> GetAgentsUsersByUserCoordinatorId(long coordinatorId)
        {
            var users = (await this.Connection.SelectAsync<TUser>(i => i.CoordinatorId == coordinatorId && i.Enabled == true)).OrderByDescending(i => i.Id);
            return users;
        }


        public async Task<IEnumerable<IUser>> GetClientsUserByClientId(long accountId)
        {
            var users = (await this.Connection.SelectAsync<TUser>(i => i.AccountId == accountId && i.Enabled == true)).OrderByDescending(i => i.Id);
            return users;
        }

        public async Task ResetPassowrd(long userId, string oldPassword, string newPassword)
        {
            var user = await base.FindById(userId) as TUser;

            var errors = await this.ValidatePasswrodExists(user, oldPassword);
            if (errors.Count() > 0)
                await this.ThrowEntityException(errors);

            user.Password = _encryptionProvider.Encrypt(newPassword);

            await base.Update(user, x => new
            {
                x.Password,
                x.ModifiedBy,
                x.ModifiedDate
            });
        }


        public async Task<bool> CheckUserPassowrdExists(long Id, string oldPassword)
        {
            TUser user = await base.FindById(Id) as TUser;
            var result = false;
            if (user != null)
            {
                string userOldPassword = user.Password;
                if (_encryptionProvider.Decrypt(userOldPassword) == oldPassword)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }

        public async Task<IEnumerable<IValidationResult>> ValidatePasswrodExists(IUser entity, string oldPassword)
        {
            ICollection<IValidationResult> errors = (await this.ValidateEntity(entity)).ToList();
            bool UserPasswordMatched = await this.CheckUserPassowrdExists(entity.Id, oldPassword);
            if (UserPasswordMatched == false)
            {
                errors.Add(new ValidationCodeResult(ErrorCodes[EnumErrorCode.UserExists]));
            }
            return errors;
        }


        public async Task ResetForgetPassword(string emailId)
        {
            var user = await this.FindFirstByEmail(emailId);
            if (user != null)
            {
                string newPassword = _encryptionProvider.GetRandomString();
                await this.UpdatePassword(user.Id, newPassword);

                // Common.DataTable dt = this.ExecuteToDataTable("select u.*, p.\"FirstName\",p.\"LastName\",p.\"Phone1\",p.\"Phone2\",p.\"Address1\",p.\"Address2\",p.\"City\",p.\"State\",p.\"Zip\",p.\"Country\" FROM public.portal_users u inner join portal_profiles p on  p.id=u.profile_id  where u.id=" + savedEntity.Id, null);
                // bool isSend = this.GetDepRepository<EmailService>().SendEmail(EmailEventType.RegistrationEmailToUser, dt, savedEntity.Email);
                this._emailService.SendMail(emailId, "Ticket System Admin ", "sp-mailer-daemon@ionemicro.com",
                    "Ticket System Admin ", "", "", "Ticket Service Password is reset", "Hi " + user.UserName + " Your Password is Reset and now its " + newPassword);

            }
            else
            {
                await this.ThrowEntityException(new ValidationCodeResult(ErrorCodes[EnumErrorCode.EmailDoesNotExists]));
            }
        }

        public async Task<IEnumerable<IValidationResult>> ValidateEntityToCheckExistsAtUpdate(IUser entity)
        {
            ICollection<IValidationResult> errors = (await this.ValidateEntity(entity)).ToList();

            if (await this.CheckExistByNameAndId(entity.UserName, entity.Id))
                errors.Add(new ValidationCodeResult(ErrorCodes[EnumErrorCode.UserNameExists, entity.UserName]));

            if (await this.CheckExistByEmailAndId(entity.Email, entity.Id))
                errors.Add(new ValidationCodeResult(ErrorCodes[EnumErrorCode.UserEmailExists, entity.Email]));

            return errors;
        }

        public async Task<bool> CheckExistByNameAndId(string name, long Id)
        {
            var existedProcess = await this.FindFirstByNameAndId(name, Id);
            return existedProcess != null;
        }

        public async Task<bool> CheckExistByEmailAndId(string Email, long Id)
        {
            var existedProcess = await this.FindFirstByEmailAndId(Email, Id);
            return existedProcess != null;
        }


        public async Task<IUser> FindFirstByNameAndId(string name, long Id)
        {
            var user = await this.Connection.FirstOrDefaultAsync<TUser>(i => i.UserName == name && i.Id != Id);
            return user;
        }

        public async Task<IUser> FindFirstByEmailAndId(string email, long Id)
        {
            var user = await this.Connection.FirstOrDefaultAsync<TUser>(i => i.Email == email && i.Id != Id);
            return user;
        }

        public async Task Delete(long id)
        {
            try
            {
                this.StartTransaction();
                var entity = await this.Connection.FirstOrDefaultAsync<TUser>(i => i.Id == id);
                if (entity != null)
                    await this.Connection.DeleteAsync<TUser>(entity);
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