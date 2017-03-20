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
    [TableWithSequence("user_queue", SequenceName = "user_queue_id_seq")]
    [Alias("user_queue")]
    public partial class UserQueue : IUserQueue
    {
        public UserQueue()
        {
        }
        [PrimaryKey]
        [AutoIncrement]
        [Alias("id")]
        public long Id { get; set; }

        [Alias("user_id")]
        public long UserId { get; set; }

        [Alias("queue_id")]
        public long QueueId { get; set; }
    }
}
