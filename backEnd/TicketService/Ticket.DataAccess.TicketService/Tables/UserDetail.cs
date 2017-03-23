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
    [TableWithSequence("user_detail", SequenceName = "user_detail_id_seq")]
    [Alias("user_detail")]
    public partial class UserDetail : IUserDetail
    {
        public UserDetail()
        {
            SelectedQueues = new List<UserQueue>();
        }
        [PrimaryKey]
        [AutoIncrement]
        [Alias("user_detail_id")]
        public long Id { get; set; }

        [Alias("first_name")]
        public string FirstName { get; set; }

        [Alias("last_name")]
        public string LastName { get; set; }

        [Alias("email_address")]
        public string EmailAddress { get; set; }

        [Alias("is_wild_card")]
        public bool IsWildCard { get; set; }

        [Alias("is_allow_workflow")]
        public bool IsAllowWorkFlow { get; set; }        

        [Alias("user_name")]
        public string UserName { get; set; }

        [Alias("password")]
        public string Password { get; set; }

        [Alias("time_zone")]
        public string TimeZone { get; set; }

        [Alias("change_pwd_at_log_on")]
        public bool ChangePasswordAtLogOn { get; set; }

        [Alias("change_pwd_sixty_days")]
        public bool ChangePasswordAtSixtyDays { get; set; }

        [Alias("enforce_pwd_history")]
        public bool EnforcePasswordHistory { get; set; }

        [Alias("view_documents")]
        public bool ViewDocuments { get; set; }

        [Alias("import_documents")]
        public bool ImportDocuments { get; set; }

        [Alias("delete_documents")]
        public bool DeleteDocuments { get; set; }

        [Alias("edit_indexes")]
        public bool EditIndexes { get; set; }

        [Alias("scan_images")]
        public bool ScanImages { get; set; }

        [Alias("create_reports")]
        public bool CreateReports { get; set; }

        [Alias("change_or_view_settings")]
        public bool ChangeOrViewSettings { get; set; }

        [Alias("created_by")]
        public long? CreatedBy { get; set; }

        [Alias("created_date")]
        public DateTime? CreatedDate { get; set; }

        [Alias("modified_by")]
        public long? ModifiedBy { get; set; }

        [Alias("modified_date")]
        public DateTime? ModifiedDate { get; set; }

        [Alias("route_user")]
        public long RouteUser { get; set; }

        [Alias("work_flow_admin")]
        public bool WorkFlowAdmin { get; set; }
         
        [Alias("is_pdf_viewer")]
        public bool IsPdfViewer { get; set; }

        [Alias("is_allow_email")]
        public bool IsAllowEmail { get; set; }

        [Alias("is_allow_print")]
        public bool IsAllowPrint { get; set; }

        [Alias("is_allow_download")]
        public bool IsAllowDownload { get; set; }        

        [Alias("client_code")]
        public string ClientCode { get; set; }

        [Alias("date")]
        public DateTime Date { get; set; }

        [Alias("batch_number")]
        public String BatchNumber { get; set; }

        [Alias("batch_type")]
        public String BatchType { get; set; }

        [Alias("status")]
        public String Status { get; set; }

        [Alias("misc")]
        public String Misc { get; set; }

        [Alias("user_id")]
        public long UserId { get; set; }

        [Ignore]
        public IEnumerable<IUserQueue> SelectedQueues { get; set; }
    }
}
