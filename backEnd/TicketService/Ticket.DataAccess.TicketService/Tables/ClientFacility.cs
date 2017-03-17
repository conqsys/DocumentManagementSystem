using Ticket.Base;
using Ticket.Base.Entities;
using SimpleStack.Orm.Attributes;


namespace Ticket.DataAccess.TicketService
{
    [TableWithSequence("client_facility", SequenceName = "client_facility_id_seq")]
    [Alias("client_facility")]
    public partial class ClientFacility : IClientFacility
    {
        public ClientFacility()
        {

        }

        [PrimaryKey]
        [AutoIncrement]
        public long Id { get; set; }

        [Alias("facility_id")]
        [System.ComponentModel.DataAnnotations.Range(1, long.MaxValue)]
        public long FacilityId { get; set; }

        [Alias("client_id")]
        [System.ComponentModel.DataAnnotations.Range(1, long.MaxValue)]
        public long ClientId { get; set; }


    }
}
