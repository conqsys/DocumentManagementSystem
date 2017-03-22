
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ticket.Base.Objects;

namespace Ticket.Base.Entities
{
    public interface IUserDetail : IEntity, IStamp
    {
        string FirstName { get; set; }

        string LastName { get; set; }

        string EmailAddress { get; set; }

        string UserName { get; set; }

        string Password { get; set; }

        string TimeZone { get; set; }

        bool ChangePasswordAtLogOn { get; set; }

        bool ChangePasswordAtSixtyDays { get; set; }

        bool EnforcePasswordHistory { get; set; }

        bool ViewDocuments { get; set; }
        bool ImportDocuments { get; set; }

        bool DeleteDocuments { get; set; }
        bool EditIndexes { get; set; }

        bool ScanImages { get; set; }
        bool CreateReports { get; set; }

        bool ChangeOrViewSettings { get; set; }

        bool WorkFlowAdmin { get; set; }

        string ClientCode { get; set; }

        DateTime Date { get; set; }

        string BatchNumber { get; set; }

        string BatchType { get; set; }

        string Status { get; set; }

        string Misc { get; set; }

        long UserId { get; set; }
        bool IsPdfViewer { get; set; }
        bool IsAllowEmail { get; set; }
        bool IsAllowPrint { get; set; }
        bool IsAllowDownload { get; set; }
        bool IsWildCard { get; set; }
        bool IsAllowWorkFlow { get; set; }
        long RouteUser { get; set; }

        IEnumerable<IUserQueue> Queues { get; set; }

    }
}
