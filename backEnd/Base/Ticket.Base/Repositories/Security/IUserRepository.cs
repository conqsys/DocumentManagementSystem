using Ticket.Base.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ticket.Base.Objects;

namespace Ticket.Base.Repositories
{
    public interface IUserRepository : IDepRepository
    {
        Task<IUser> CreateInstance();

        Task<IUser> AddNew(IUser entity);
        
        Task<IUser> CheckLogin(string userName, string password);

        Task IncreaseAttempts(string userName);

        Task<bool> CheckAuthentication(string userName);
        
        Task<IUser> UpdatePassword(long userId, string password);

        Task<IUser> Update(IUser entity);
        
        Task<IEnumerable<IUserDTO>> FindAll(bool enabledRequired);
        Task<IEnumerable<IUser>> FindAllUsersList();

        Task<IEnumerable<IUser>> FindAllByRoleId(long roleId);
        
        Task<IUser> FindFirstByName(string userName);

        Task<IUser> ChangeActiveState(long userId, bool isEnabled, long modifiedById);

        Task<bool> Exists(string userName);

        Task<IEnumerable<IValidationResult>> ValidateEntity(IUser entity);

        Task<IUser> LogOut(long userId);
        
        Task<IEnumerable<IValidationResult>> ValidateExists(string userName, ICollection<IValidationResult> errors);

        Task<IEnumerable<IUser>> FindUserByAccountId(long accountId);

        Task<IEnumerable<IUser>> GetAgentsUsersByUserCoordinatorId(long coordinatorId);

        Task<IEnumerable<IUser>> GetClientsUserByClientId(long accountId);

        Task ResetPassowrd(long userId, string oldPassword, string newPassword);

        Task ResetForgetPassword(string emailId);

        Task Delete(long id);


    }
}
