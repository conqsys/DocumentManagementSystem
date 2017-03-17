using Microsoft.AspNetCore.Mvc;
using Ticket.API.Common;
using Ticket.BusinessLogic.Common;
using Ticket.DataAccess.TicketService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.API.TicketService
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {


        [HttpPost("/login/{username}/{password}")]
        public async Task<ActionResult> Login(string username, string password)
        {
            System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
            client.BaseAddress = new Uri("http://" + this.Request.Host.Value);
            List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>();
            values.Add(new KeyValuePair<string, string>("username", username));
            values.Add(new KeyValuePair<string, string>("password", password));

            System.Net.Http.FormUrlEncodedContent content = new System.Net.Http.FormUrlEncodedContent(values);
            var response = await client.PostAsync("/token", content);

            string result = response.Content.ReadAsStringAsync().Result;

            object dataObject = result;
            try
            {
                dataObject = Newtonsoft.Json.Linq.JObject.Parse(result);
            }
            catch
            {

            }

            return StatusCode((int)response.StatusCode, dataObject);
        }
    }
}
