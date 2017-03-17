using Ticket.Base;
using Ticket.Base.Entities;
using SimpleStack.Orm.Attributes;
using System;
using System.Collections.Generic;

namespace Ticket.DataAccess.TicketService
{
    [TableWithSequence("user", SequenceName = "user_id_seq")]
    public partial class User : IUser
    {
        public User()
        {
            GroupUsers = new List<Ticket.DataAccess.TicketService.GroupUser>();
        }

        [AutoIncrement]
        public long Id { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        [Alias("email")]
        public string Email { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        [Alias("user_name")]
        public string UserName { get; set; }

        [Alias("password")]
        public string Password { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        [Alias("enabled")]
        public bool Enabled { get; set; }

        [Alias("phone_number")]
        public string PhoneNumber { get; set; }

        [System.ComponentModel.DataAnnotations.Range(1, long.MaxValue)]
        [Alias("role_id")]
        public long RoleId { get; set; }

        [Alias("account_id")]
        public long? AccountId { get; set; }

        [Alias("coordinator_id")]
        public long? CoordinatorId { get; set; }

        [Alias("created_date")]
        public DateTime? CreatedDate { get; set; }

        [Alias("created_by")]
        public long? CreatedBy { get; set; }

        [Alias("modified_by")]
        public long? ModifiedBy { get; set; }

        [Alias("modified_date")]
        public DateTime? ModifiedDate { get; set; }

        [Ignore]
        public virtual IEnumerable<IGroupUser> GroupUsers { get; set; }

        [Ignore]
        [Alias("name")]
        public string RoleName { get; set; }

        [Ignore]
        public string CoordinatorName { get; set; }


        [Ignore]
        public bool IsPasswordReset { get; set; }

        [Ignore]
        public string ClientName { get; set; }

    }
}
