using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ticket.Base.Objects;

namespace Ticket.Base.Entities
{
    public interface IClient : IEntity, IStamp
    {
        string ClientName { get; set; }

        string Description { get; set; }

        bool Enabled { get; set; }

        bool? LoginEnabled { get; set; }

        string ClientAcronym { get; set; }

        long? RequestTypeId { get; set; }

        IEnumerable<IUser> ClientUsers { get; set; }

        IEnumerable<IClientFacility> ClientFacilities { get; set; }

        string AccountNumber { get; set; }
    }
}
