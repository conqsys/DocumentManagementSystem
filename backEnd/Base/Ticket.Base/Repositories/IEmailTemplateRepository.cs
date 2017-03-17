using Ticket.Base.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.Base.Repositories
{
    public interface IEmailTemplateRepository : IDepRepository
    {
        Task<IEmailTemplate> GetTemplate(IClient entity);
    }
}
