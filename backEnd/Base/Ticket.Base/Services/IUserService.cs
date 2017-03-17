using Ticket.Base.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ticket.Base.Objects;

namespace Ticket.Base.Services
{
    public interface IUserService
    {
        Task<IUser> AddNewUser(IUser entity);

        Task ChangePassword(long userId, string password);

        Task<bool> ChangeActiveState(long userId, bool isEnabled, long modifiredById);

        Task<IUser> CheckUserAuthentication(string userName);

        Task<bool> UpdateUser(IUser entity);

        Task<bool> LogOutUser(long userId);

        Task DeleteRole(long roleId);

        Task<IEnumerable<IUserDTO>> GetAllUsers(bool enabledRequired);

        Task<IEnumerable<IUser>> GetAllUsersList();

    }
}
