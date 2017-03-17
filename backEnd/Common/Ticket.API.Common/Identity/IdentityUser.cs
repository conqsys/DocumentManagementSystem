using Microsoft.AspNetCore.Http;
using Ticket.Base.Entities;
using Ticket.DataAccess.Common;
using SimpleStack.Orm.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace Ticket.API.Common
{

    public partial class IdentityUser : IEntity, IUser
    {

        public IdentityUser(UserCacheService<IdentityUser> userCacheService, IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor != null
                && httpContextAccessor.HttpContext != null
                && httpContextAccessor.HttpContext.User != null
                && httpContextAccessor.HttpContext.User.Identity != null
                )
            {
                var identity = (httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity);
                var idClaim = identity.Claims.FirstOrDefault(i => i.Type == "Id");
                if (identity != null && idClaim != null)
                {
                    var cacheUser = userCacheService.Get(idClaim.Value);
                    this.CacheToMe(cacheUser);
                }
            }
        }

        public void CacheToMe(IUser user)
        {
            if (user != null)
            {
                this.RoleId = user.RoleId;
                this.PhoneNumber = user.PhoneNumber;
                this.AccountId = user.AccountId;
                this.CoordinatorId = user.CoordinatorId;
                this.Email = user.Email;
                this.Enabled = user.Enabled;
                this.UserName = user.UserName;
                this.Id = user.Id;
            }
        }

        public void SetIdentiyFromClaims(ClaimsIdentity identity)
        {
            if (identity.Claims.Count() > 0)
            {
                this.UserName = identity.Claims.FirstOrDefault(i => i.Type == "UserName").Value;
                this.Email = identity.Claims.FirstOrDefault(i => i.Type == "Email").Value;
                this.Id = long.Parse(identity.Claims.FirstOrDefault(i => i.Type == "UserId").Value);
            }
        }

        public long Id { get; set; }


        public string Email { get; set; }


        public string UserName { get; set; }

        public string Password { get; set; }


        public bool Enabled { get; set; }


        public string PhoneNumber { get; set; }

        public long? CreatedBy { get; set; }

        public long RoleId { get; set; }

        public long? CoordinatorId { get; set; }

        public long? AccountId { get; set; }

        public DateTime? CreatedDate { get; set; }


        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public virtual IEnumerable<IGroupUser> GroupUsers { get; set; }

        public string RoleName { get; set; }

        public string CoordinatorName { get; set; }

        public string ClientName { get; set; }

        public bool IsPasswordReset { get; set; }

    }
}
