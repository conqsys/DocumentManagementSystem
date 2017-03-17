using Ticket.Base;
using Ticket.Base.Entities;
using SimpleStack.Orm.Attributes;
using System;
using System.Collections.Generic;

namespace Ticket.DataAccess.TicketService
{
    [TableWithSequence("client", SequenceName = "client_id_seq")]
    [Alias("client")]
    public partial class Client : IClient
    {
        public Client()
        {
            ClientUsers = new List<Ticket.DataAccess.TicketService.User>();
            ClientFacilities = new List<ClientFacility>();
        }

        [PrimaryKey]
        [AutoIncrement]
        public long Id { get; set; }


        [System.ComponentModel.DataAnnotations.Required]
        public bool Enabled { get; set; }

        [Alias("login_enabled")]
        public bool? LoginEnabled { get; set; }

        [Alias("client_name")]
        [System.ComponentModel.DataAnnotations.Required]
        public string ClientName { get; set; }

        public string Description { get; set; }

        [Alias("client_acronym")]
        [System.ComponentModel.DataAnnotations.Required]
        public string ClientAcronym { get; set; }

        [Alias("request_type_id")]
        public long? RequestTypeId { get; set; }


        [Alias("created_by")]
        public long? CreatedBy { get; set; }

        [Alias("created_date")]
        public DateTime? CreatedDate { get; set; }


        [Alias("modified_by")]
        public long? ModifiedBy { get; set; }

        [Alias("modified_date")]
        public DateTime? ModifiedDate { get; set; }

        [Ignore]
        public IEnumerable<IUser> ClientUsers { get; set; }

        [Ignore]
        public virtual IEnumerable<IClientFacility> ClientFacilities { get; set; }

        [Alias("account_number")]
        [System.ComponentModel.DataAnnotations.Required]
        public string AccountNumber { get; set; }

    }
}
