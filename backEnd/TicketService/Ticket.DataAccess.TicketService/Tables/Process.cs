using Ticket.Base;
using Ticket.Base.Entities;
using SimpleStack.Orm.Attributes;
using System;
using System.Collections.Generic;

namespace Ticket.DataAccess.TicketService
{
    [TableWithSequence("process", SequenceName = "process_id_seq")]
    [Alias("process")]
    public partial class Process : IProcess
    {
        public Process()
        {
            ProcessRequestTypes = new List<ProcessRequestType>();
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

        [Ignore]
        public virtual IEnumerable<IProcessRequestType> ProcessRequestTypes { get; set; }
    }
}
