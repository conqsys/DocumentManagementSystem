
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ticket.Base.Objects;

namespace Ticket.Base.Entities
{
    public interface IImportFile : IEntity, IStamp
    {
        long ClientId { get; set; }
        string BatchNumber { get; set; }
        string BatchType { get; set; }

        string Status { get; set; }

        string Misc { get; set; }

        long RouteTo { get; set; }

        int Type { get; set; }

        DateTime UploadDate { get; set; }

        string UploadImage { get; set; }
        

    }
}
