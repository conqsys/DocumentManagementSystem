using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.Base.Objects
{
    public class UserDTO : IUserDTO
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public bool Enabled { get; set; }
        public string PhoneNumber { get; set; }
        public string RoleName { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public long? AccountId { get; set; }
        public string ClientName { get; set; }
        public long? CoordinatorId { get; set; }
        public string CoordinatorName { get; set; }
    }
}
