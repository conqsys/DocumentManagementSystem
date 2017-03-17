using Ticket.Base;
using Ticket.Base.Entities;
using SimpleStack.Orm.Attributes;

namespace Ticket.DataAccess.TicketService
{
    [TableWithSequence("process_request_type", SequenceName = "process_request_type_id_seq")]
    [Alias("process_request_type")]
    public partial class ProcessRequestType : IProcessRequestType
    {
        public ProcessRequestType()
        {

        }

        [PrimaryKey]
        [AutoIncrement]
        public long Id { get; set; }

        [Alias("process_id")]
        [System.ComponentModel.DataAnnotations.Range(1, long.MaxValue)]
        public long ProcessId { get; set; }

        [Alias("request_type_id")]
        [System.ComponentModel.DataAnnotations.Range(1, long.MaxValue)]
        public long RequestTypeId { get; set; }


    }
}
