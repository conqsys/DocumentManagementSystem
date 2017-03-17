using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Ticket.API.Common;

namespace Ticket.API.TicketService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            UrlConfiguration urlConfiguration = new UrlConfiguration();

            var host = new WebHostBuilder()
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseStartup<Startup>()
            .UseUrls(urlConfiguration.GetAppUrl(ServiceType.Security))
            .Build();

            host.Run();


        }
    }
}
