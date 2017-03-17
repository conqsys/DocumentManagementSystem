
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
    [TableWithSequence("file_indexes", SequenceName = "file_indexes_id_seq")]
    [Alias("file_indexes")]
    public partial class FileIndexes : IFileIndexes
    {
        public FileIndexes()
        {

        }
        [PrimaryKey]
        [AutoIncrement]
        [Alias("id")]
        public long Id { get; set; }

        [Alias("index_name")]
        public string IndexName { get; set; }

        [Alias("index_type")]
        public int IndexType { get; set; }

        [Alias("default_value")]
        public string DefaultValue { get; set; }

        [Alias("list_value")]
        public string ListValue { get; set; }

      

    }
}
