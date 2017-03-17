using System;
using System.Collections.Generic;

namespace Ticket.Base.Entities
{
    public interface IUserRoles : IEntity, IStamp
    {
        long? UserId { get; set; }

        long? RoleId { get; set; }
    }
}
