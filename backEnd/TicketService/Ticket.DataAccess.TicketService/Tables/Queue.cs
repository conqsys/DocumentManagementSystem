
using Ticket.Base;
using Ticket.Base.Entities;
using Newtonsoft.Json;
using SimpleStack.Orm.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ticket.Base.Objects;
using Ticket.DataAccess.TicketService;

namespace Ticket.DataAccess.TicketService
{
    [TableWithSequence("queue", SequenceName = "queue_id_seq")]
    [Alias("queue")]
    public partial class Queue : IQueue
    {
        public Queue()
        {

        }
        [PrimaryKey]
        [AutoIncrement]
        [Alias("id")]
        public long Id { get; set; }
        

        [Alias("name")]
        public string Name { get; set; }

        [Alias("description")]
        public string Description { get; set; }

        

    }
}
