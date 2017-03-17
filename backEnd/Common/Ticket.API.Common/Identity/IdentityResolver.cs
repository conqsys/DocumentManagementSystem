using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

using Ticket.Base.Repositories;
using Ticket.Base.Entities;

namespace Ticket.API.Common
{
    public class IdentityResolver
    {
        private IUserRepository _userRepository;
        private UserCacheService<IUser> _userCacheService;
        public IdentityResolver(IUserRepository userRepository, UserCacheService<IUser> userCacheService)
        {
            this._userRepository = userRepository;
            this._userCacheService = userCacheService;
        }
        public async Task<ClaimsIdentity> CheckUserLogin(string userName, string password)
        {
            var user = await this._userRepository.CheckLogin(userName, password);
            if (user != null)
            {
                if (user != null)
                {
                    return this.GetIdentity(user);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        private ClaimsIdentity GetIdentity(IUser user)
        {
            if (user != null)
            {
                TicketIdentity myIdentity = new TicketIdentity(new GenericIdentity(user.Email), new List<Claim>(), "Standard", "name", "role", user);
                return myIdentity;
            }
            else
            {
                return null;
            }
        }
    }

}
