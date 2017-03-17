using Ticket.Base;
using Ticket.Base.Entities;
using SimpleStack.Orm.Attributes;
using System;

namespace Ticket.DataAccess.TicketService
{
    [TableWithSequence("ticket_status", SequenceName = "ticket_status_id_seq")]
    [Alias("ticket_status")]
    public partial class TicketStatus : ITicketStatus
    {
        public TicketStatus()
        {

        }

        [PrimaryKey]
        [AutoIncrement]
        public long Id { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
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
