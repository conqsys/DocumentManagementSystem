using Ticket.API.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using Ticket.Base.Entities;

namespace Microsoft.AspNetCore.Builder
{
    public static class JWTService
    {
        private readonly static string secretKey = "JWT!Secret#As#perMYChoiCE!123";
        public readonly static SymmetricSecurityKey SigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

        public static IApplicationBuilder UseJwtAuthentication(this IApplicationBuilder app)
        {
            var Configuration = (IConfiguration)app.ApplicationServices.GetService(typeof(IConfiguration));
            TicketJwtTokenHandler jwtHandler = new TicketJwtTokenHandler();



            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = SigningKey,

                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = Configuration["ValidIssuer"],

                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = Configuration["ValidAudience"],

                // Validate the token expiry
                ValidateLifetime = true,

                // If you want to allow a certain amount of clock drift, set that here:
                ClockSkew = TimeSpan.Zero,

            };



            JwtBearerOptions bearerOptions = new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = tokenValidationParameters,
                Authority = Configuration["domain"],
                RequireHttpsMetadata = false,
                Audience = Configuration["ValidAudience"],
                Configuration = new OpenIdConnectConfiguration
                {
                    Issuer = Configuration["ValidIssuer"],
                },

                Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {

                        return Task.FromResult(0);
                    },
                    OnMessageReceived = context =>
                    {

                        return Task.FromResult(0);
                    },
                    OnTokenValidated = context =>
                    {
                        var cacheService = app.ApplicationServices.GetService<UserCacheService<IdentityUser>>();
                        var identity = context.Ticket.Principal.Identity as TicketIdentity;


                        var idClaim = identity.Claims.FirstOrDefault(i => i.Type == "Id");
                        if (idClaim != null)
                        {
                            var cachedUser = cacheService.Get(idClaim.Value);
                            identity.User = cachedUser;
                        }

                        Console.Write(context.Ticket.Principal.Identity.Name);
                        return Task.FromResult(0);
                    },
                    OnChallenge = context =>
                    {
                        return Task.FromResult(0);
                    }

                }
            };
            bearerOptions.SecurityTokenValidators.RemoveAt(0);
            bearerOptions.SecurityTokenValidators.Add(jwtHandler);

            app.UseJwtBearerAuthentication(bearerOptions);

            return app;
        }


    }
}
