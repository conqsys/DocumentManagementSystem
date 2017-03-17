using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.API.Common
{
    public class HttpResult<T>
    {
        public T Data { get; set; }
        public IRestResponse Response { get; set; }
        public bool HasParseError { get; set; }
        public string ActualContent { get; set; }
    }
}
