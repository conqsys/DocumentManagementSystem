using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.API.Common
{
    public class BaseClient
    {
        public string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJtYW5vaiIsImp0aSI6Ijk1M2IwOGM3LTVkYzgtNDUyNC05MDAyLWE3Y2JkOThlOGVmMyIsImlhdCI6MTQ4OTEzMDg2MSwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6Im1hbm9qQGNvbnFzeXMuY29tIiwiSWQiOiIxIiwibmJmIjoxNDg5MTMwODYxLCJleHAiOjE0ODkxNzc0ODYsImlzcyI6Imh0dHA6Ly9hYmMuY29tIiwiYXVkIjoiVGlja2V0aW5nQXBwIn0.DGSn1aZQq6BGkQ9rn3-SzrnN-u_jIm_OA7WK1gMzsaA";

        public AsyncRestClient GetClient(string defaultServiceTypeUrl)
        {
            UrlConfiguration configuration = new UrlConfiguration();
            string restUrl = configuration.GetAppUrl(defaultServiceTypeUrl);

            var client = new AsyncRestClient(restUrl);
            return client;
        }

        public RestRequest GetRequest(string resource, Method method)
        {
            var request = new RestRequest(resource, method);
            request.AddHeader("Authorization", "Bearer " + token);
            return request;
        }
    }
}
