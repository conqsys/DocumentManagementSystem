using Ticket.Base;
using Ticket.Base.Entities;
using SimpleStack.Orm.Attributes;
using System;

namespace Ticket.DataAccess.TicketService
{
    [TableWithSequence("work_order_attachment", SequenceName = "work_order_attachment_id_seq")]
    [Alias("work_order_attachment")]
    public partial class WorkOrderAttachment : IWorkOrderAttachment
    {
        public WorkOrderAttachment()
        {
        }

        [PrimaryKey]
        [AutoIncrement]
        public long Id { get; set; }

        [Alias("attachment_name")]
        public string AttachmentName { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        [Alias("work_order_id")]
        public long WorkOrderId { get; set; }

        [Alias("created_by")]
        public long? CreatedBy { get; set; }

        [Alias("created_date")]
        public DateTime? CreatedDate { get; set; }

    }
}
