using Ticket.Base;
using Ticket.Base.Entities;
using SimpleStack.Orm.Attributes;
using System;

namespace Ticket.DataAccess.TicketService
{
    [TableWithSequence("facility", SequenceName = "facility_id_seq")]
    [Alias("facility")]
    public partial class Facility : IFacility
    {
        public Facility()
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
