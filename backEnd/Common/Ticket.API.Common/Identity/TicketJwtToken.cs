using Microsoft.IdentityModel.Tokens;
using Ticket.Base.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Ticket.API.Common
{
    public class TicketJwtToken : JwtSecurityToken
    {
        public TicketJwtToken(TicketJwtPayLoad payload) : base(new JwtHeader(), payload)
        {

        }

        public TicketJwtToken(IUser user, string issuer, string audience, IEnumerable<Claim> claims, DateTime? notBefore, DateTime? expires, SigningCredentials signingCredentials) : base(new JwtHeader(signingCredentials), new TicketJwtPayLoad(user, issuer, audience, claims, notBefore, expires))
        {

        }
    }

    public class TicketJwtPayLoad : JwtPayload
    {
        public TicketJwtPayLoad(IUser user, string issuer, string audience, IEnumerable<Claim> claims, DateTime? notBefore, DateTime? expires) : base(issuer, audience, claims, notBefore, expires)
        {
            this._user = user;
        }
        private IUser _user;

        public IUser User
        {
            get { return _user; }
            private set { _user = value; }
        }


    }
}
