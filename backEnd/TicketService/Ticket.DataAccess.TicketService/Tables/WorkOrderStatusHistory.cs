using Ticket.Base;
using Ticket.Base.Entities;
using SimpleStack.Orm.Attributes;
using System;

namespace Ticket.DataAccess.TicketService
{
    [TableWithSequence("work_order_status_history", SequenceName = "work_order_status_history_id_seq")]
    [Alias("work_order_status_history")]
    public partial class WorkOrderStatusHistory : IWorkOrderStatusHistory
    {
        public WorkOrderStatusHistory()
        {
        }

        [PrimaryKey]
        [AutoIncrement]
        public long Id { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        [Alias("work_order_id")]
        public long WorkOrderId { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        [Alias("work_order_status_id")]
        public long WorkOrderStatusId { get; set; }

        [Alias("created_by")]
        public long? CreatedBy { get; set; }

        [Alias("created_date")]
        public DateTime? CreatedDate { get; set; }

    }
}
