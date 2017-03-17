using Ticket.Base.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.Base.Repositories
{
    public interface IUserRoleRepository : IDepRepository
    {
        Task<IUserRoles> AddNew(IUserRoles entity);
        
        Task<IEnumerable<IUserRoles>> FindAllRolesByUserId(long userId);

        Task DeleteUserRole(long userId, long roleid);

        Task DeleteUserRoles(IEnumerable<IUserRoles> userRoles);

        Task DeleteUserRoles(long userId);

        Task AddNew(long userId, long?[] roleIds);

        Task ValidateRoleExistInUser(long roleId);
    }
}
