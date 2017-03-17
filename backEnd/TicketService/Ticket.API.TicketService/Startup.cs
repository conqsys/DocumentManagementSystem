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
using Ticket.API.Common;
using Ticket.DataAccess.Common;
using Ticket.BusinessLogic.TicketService;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Ticket.API.TicketService;
using Ticket.Base.Entities;
using Ticket.DataAccess.TicketService;
using ServiceStack.Redis;
using Ticket.Base.Repositories;
using Ticket.BusinessLogic.Common;

namespace Ticket.API.TicketService
{
    public class Startup
    {
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
            services.AddSingleton<EncryptionProvider>();
            services.AddSingleton<CodeGenerator>();
            services.AddCommonService(Configuration, ServiceType.Security);
            services.AddSingleton<BaseValidationErrorCodes, ValidationErrorCodes>();
            services.AddSingleton<IdentityResolver>();
            services.AddSingleton<UserCacheScheduler>();

            services.AddSwaggerService();
            services.AddTicketServiceRepositories();

            services.AddScoped<Base.Services.IUserService, Ticket.BusinessLogic.TicketService.UserService>();
            //services.AddScoped<IGroupUserRepository, GroupUserRepository<Ticket.DataAccess.TicketService.GroupUser>>();

            services.AddSingleton<EmailService>();
            services.AddCustomMvc();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            Console.WriteLine("Starting...");

            var cacheScheduler = app.ApplicationServices.GetService<UserCacheScheduler>();
            cacheScheduler.ScheduleCache(message =>
            {
                Console.WriteLine(message);
            });

            app.UseCors(builder =>
               builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod()
               );



            app.UseSwagger();
            app.UseSwaggerUi();

            app.UseJwtAuthentication();

            app.UseSimpleTokenProvider(new TokenProviderOptions
            {
                Path = "/token",
                Audience = Configuration["ValidAudience"],
                Issuer = Configuration["ValidIssuer"],
                SigningCredentials = new SigningCredentials(JWTService.SigningKey, SecurityAlgorithms.HmacSha256),
                IdentityResolver = app.ApplicationServices.GetService<IdentityResolver>().CheckUserLogin,
                Expiration = DateTime.Now.AddDays(7).TimeOfDay
            });

           // app.UseMvc();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "index");
                //routes.MapRoute(
                //    name: "resetPassword",
                //    template: "resetpassword/token",
                //    defaults: new { controller = "ResetPassword", action = "GetUserDetail" });
            });

            DefaultFilesOptions options = new DefaultFilesOptions();
            options.DefaultFileNames.Add("index.html");
            app.UseDefaultFiles(options);

            app.UseStaticFiles();

        }
    }

}
