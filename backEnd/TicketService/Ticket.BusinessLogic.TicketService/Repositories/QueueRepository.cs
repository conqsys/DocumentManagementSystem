using Ticket.DataAccess.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Extensions.Configuration;
using Dapper;
using Ticket.Base.Entities;
using Ticket.Base.Repositories;
using Ticket.DataAccess.TicketService;
using Npgsql;
using SimpleStack.Orm;
using SimpleStack.Orm.Expressions;
using Ticket.BusinessLogic.Common;
using Ticket.Base;
using Ticket.Base.Objects;

namespace Ticket.BusinessLogic.TicketService
{
    public class QueueRepository<TQueue> : ModuleBaseRepository<TQueue>, IQueueRepository
        where TQueue : class, IQueue, new()


    {
        public QueueRepository(BaseValidationErrorCodes errorCodes, DatabaseContext dbContext, IUser loggedUser)
            : base(errorCodes, dbContext, loggedUser)
        {
        }
        
        public async Task<IEnumerable<IQueue>> GetUsers()
        {
            var getUsersList = (await this.Connection.SelectAsync<TQueue>()).OrderByDescending(i => i.Id);
                       
            return getUsersList;
        }
        
    }
}
