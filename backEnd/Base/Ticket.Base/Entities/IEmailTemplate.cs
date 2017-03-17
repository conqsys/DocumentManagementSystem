using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ticket.Base.Objects;

namespace Ticket.Base.Entities
{
    public interface IEmailTemplate : IEntity
    {
        string DefaultMailFromName { get; set; }

        string DefaultMailFromId { get; set; }

        string DefaultMailToName { get; set; }

        string DefaultMailCC { get; set; }

        string DefaultMailToId { get; set; }

        string DefaultMailSubject { get; set; }

        string DefaultMailBody { get; set; }

        int EventType { get; set; }
    }
}
