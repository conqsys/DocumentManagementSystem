using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.SwaggerGen.Generator;
using Microsoft.AspNetCore.Mvc.Authorization;
using Swashbuckle.Swagger.Model;
using Molecular.API.Common;
using Molecular.DataAccess.Common;
using Molecular.BusinessLogic.Security;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Molecular.API.SecurityService;
using Molecular.DataAccess.PatientService;

namespace Molecular.API.Host
{
    public class Startup
    {
        private readonly string secretKey = "JWT!Secret#As#perMYChoiCE!123";

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            var pathToDoc = Configuration["Swagger:Path"];
            //connection = Configuration["connectionString"];
            //Console.WriteLine(connection);
            services.AddSingleton<IConfiguration>(Configuration);
            
            services.AddSingleton<Microsoft.AspNetCore.Http.HttpContextAccessor>();
            services.AddSwaggerGen();
            services.AddScoped<DatabaseContext>();
            services.AddSingleton<IdentityResolver>();
            services.AddSingleton<ValidationErrorCodes>();

            services.ConfigureSwaggerGen(options =>
            {

                options.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
                options.SingleApiVersion(new Swashbuckle.Swagger.Model.Info
                {
                    Version = "v1",
                    Title = "Molecular Sample API",
                    Description = "REST API Access to Molecular System",
                    TermsOfService = "None"
                });
                options.DescribeAllEnumsAsStrings();
            });
            services.AddSecurityServiceRepositories();
            services.AddPatientServiceRepositories();

            services.AddCors();
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
            app.UseCors(builder =>
               builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod()
               );

            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

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

            MolecularJwtTokenHandler jwtHandler = new MolecularJwtTokenHandler();
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

                        //app.ApplicationServices.GetService<HttpContext>().User = new MolecularPrincipal(app.ApplicationServices.GetService<IdentityResolver>().GetIdentity(context.Ticket.Principal.Identity.Name));
                        //context.HttpContext.User = new MolecularPrincipal(app.ApplicationServices.GetService<IdentityResolver>().GetIdentity(context.Ticket.Principal.Identity.Name));
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

            app.UseSwagger();
            app.UseSwaggerUi();

            
            app.UseSimpleTokenProvider(new TokenProviderOptions
            {
                Path = "/token",
                Audience = Configuration["ValidAudience"],
                Issuer = Configuration["ValidIssuer"],
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
                IdentityResolver = app.ApplicationServices.GetService<IdentityResolver>().CheckUserLogin,
                Expiration = DateTime.Now.AddDays(7).TimeOfDay
            });

            app.UseMvc();
        }
    }
    public class AuthorizationHeaderParameterOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var filterPipeline = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            var isAuthorized = filterPipeline.Select(filterInfo => filterInfo.Filter).Any(filter => filter is AuthorizeFilter);
            var allowAnonymous = filterPipeline.Select(filterInfo => filterInfo.Filter).Any(filter => filter is IAllowAnonymousFilter);

            if (isAuthorized && !allowAnonymous)
            {
                if (operation.Parameters == null)
                    operation.Parameters = new List<IParameter>();

                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "Authorization",
                    In = "header",
                    Description = "access token",
                    Required = true,
                    Type = "string"
                });
            }
        }
    }
}
