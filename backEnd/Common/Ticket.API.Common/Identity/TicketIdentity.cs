using Microsoft.AspNetCore.Http;
using Ticket.Base.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Ticket.API.Common
{
    public enum TicketRole
    {
        Client,
        Coordinator,
        Agent
    }

    public class TicketPrincipal : ClaimsPrincipal
    {
        public TicketPrincipal(TicketIdentity identity) : base()
        {
            this._identity = identity;
            identity.Principal = this;
            this.AddIdentity(identity);

        }
        public bool IsInRole(TicketRole role)
        {
            bool result = base.IsInRole(role.ToString());
            return result;
        }
        private IIdentity _identity;
        public override IIdentity Identity
        {
            get
            {
                return _identity;
            }
        }

    }

    public class TicketIdentity : ClaimsIdentity
    {
        //
        public TicketIdentity(IIdentity identity,
            IEnumerable<Claim> claims,
            string authenticationType,
            string nameType,
            string roleType,
            IUser user = null) : base(identity, claims, authenticationType, nameType, roleType)
        {
            if (user != null)
            {
                this.User = user;
                this.AddClaim(new Claim("Id", user.Id.ToString()));
            }

        }

        public TicketPrincipal Principal { get; set; }

        public void AddUserClaims(IUser user)
        {
            this.AddClaim(new Claim("UserName", user.UserName));
            this.AddClaim(new Claim("Email", user.Email));
        }
        private IUser _user;

        public IUser User
        {
            get { return _user; }
            set { _user = value; }
        }

        public static TicketIdentity ToIdentity(ClaimsIdentity identity)
        {
            TicketIdentity myIdentity = new TicketIdentity(identity, identity.Claims, identity.AuthenticationType, identity.NameClaimType, identity.RoleClaimType);
            return myIdentity;
        }

    }
}


