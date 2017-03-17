using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ticket.DataAccess;
using System.Security.Principal;
using Ticket.Base;
using System.ComponentModel.DataAnnotations;
using Ticket.API.Common;
using Ticket.DataAccess.Common;
using Dapper;
using Ticket.Base.Entities;
using Ticket.Base.Repositories;
using Ticket.DataAccess.TicketService;

namespace Ticket.API.TicketService
{
    [Route("api/[controller]")]
    public class QueueController : SecuredRepositoryController<IQueueRepository>
    {
        public QueueController(IQueueRepository repository) : base(repository)
        {

        }

        [HttpGet("getListOfUsers")]
        public async Task<IEnumerable<IQueue>> GetListOfUsers()
        {
            return await this.Repository.GetUsers();
        }








    }

}

