using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.Base.Objects
{
    public class IUserDTO
    {
        long UserId { get; set; }
        long RoleId { get; set; }
        string RoleName { get; set; }
        string UserName { get; set; } 
        string UserEmail { get; set; } 
        long? AccountId { get; set; }
        string ClientName { get; set; }
        long CoordinatorId { get; set; }
        string CoordinatorName { get; set; }
    }
}
