using System;
using System.Collections.Generic;

namespace Ticket.Base.Entities
{
    public interface IRole : IEntity, IStamp
    {
        string Name { get; set; }

        string Description { get; set; }

        bool Enabled { get; set; }
    }
}
