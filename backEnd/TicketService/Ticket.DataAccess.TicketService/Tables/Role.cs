using Ticket.Base;
using Ticket.Base.Entities;
using SimpleStack.Orm.Attributes;
using System;

namespace Ticket.DataAccess.TicketService
{
    [TableWithSequence("role", SequenceName = "role_id_seq")]
    public partial class Role : IRole
    {
        public Role()
        {
        }

        [PrimaryKey]
        [AutoIncrement]
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }


        public bool Enabled { get; set; }


        [Alias("created_by")]
        public long? CreatedBy { get; set; }

        [Alias("created_date")]
        public DateTime? CreatedDate { get; set; }


        [Alias("modified_by")]
        public long? ModifiedBy { get; set; }

        [Alias("modified_date")]
        public DateTime? ModifiedDate { get; set; }
    }
}
