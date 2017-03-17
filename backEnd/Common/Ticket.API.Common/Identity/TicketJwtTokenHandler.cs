using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ticket.API.Common
{
    public class TicketJwtTokenHandler : JwtSecurityTokenHandler
    {
        public override ClaimsPrincipal ValidateToken(string token, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            ClaimsPrincipal principal = base.ValidateToken(token, validationParameters, out validatedToken);
            var parsedToken = base.ReadJwtToken(token);
            TicketIdentity identity = TicketIdentity.ToIdentity(principal.Identity as ClaimsIdentity);
            TicketPrincipal TicketPrincipal = new TicketPrincipal(identity);
            return TicketPrincipal;

        }
    }
}
