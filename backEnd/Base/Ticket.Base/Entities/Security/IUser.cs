using System;
using System.Collections.Generic;

namespace Ticket.Base.Entities
{

    public interface IUser : IEntity, IStamp
    {
        bool Enabled { get; set; }

        string UserName { get; set; }

        string Password { get; set; }

        string PhoneNumber { get; set; }

        string Email { get; set; }

        long RoleId { get; set; }

        long? AccountId { get; set; }

        long? CoordinatorId { get; set; }

        IEnumerable<IGroupUser> GroupUsers { get; set; }

        string RoleName { get; set; }

        string CoordinatorName { get; set; }

        string ClientName { get; set; }

        bool IsPasswordReset { get; set; }
    }
}
