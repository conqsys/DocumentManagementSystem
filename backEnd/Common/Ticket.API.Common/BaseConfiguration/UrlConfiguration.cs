using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.API.Common
{
    public class ServiceType
    {
        public const string BaseHost = "http://0.0.0.0";
        public const string Security = BaseHost + ":6001";
        public const string Ticket = BaseHost + ":7001";
    }

    public class UrlConfiguration
    {
        public string GetAppUrl(string defaultServiceTypeUrl)
        {
            var builder = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddEnvironmentVariables();

            Console.WriteLine(Directory.GetCurrentDirectory());

            var configuration = builder.Build();
            string url = defaultServiceTypeUrl;
            if (configuration["appUrl"] != null)
            {
                url = configuration["appUrl"];
            }

            return url;
        }
    }
}
