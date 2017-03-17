
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
    [TableWithSequence("import_file", SequenceName = "import_file_id_seq")]
    [Alias("import_file")]
    public partial class ImportFile : IImportFile
    {
        public ImportFile()
        {

        }
        [PrimaryKey]
        [AutoIncrement]
        [Alias("id")]
        public long Id { get; set; }

        [Alias("client_id")]
        public long ClientId { get; set; } 
      
        [Alias("batch_number")]
        public string BatchNumber { get; set; }

        [Alias("batch_type")]
        public string BatchType { get; set; }

        [Alias("status")]
        public string Status { get; set; }

        [Alias("misc")]
        public string Misc { get; set; }

        [Alias("route_to")]
        public long RouteTo { get; set; }

        [Alias("type")]
        public int Type { get; set; }

        [Alias("upload_date")]
        public DateTime UploadDate { get; set; }

        [Alias("upload_image")]
        public string UploadImage { get; set; }

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
