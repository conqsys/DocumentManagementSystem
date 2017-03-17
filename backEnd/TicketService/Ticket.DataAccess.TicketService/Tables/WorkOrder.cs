using Ticket.Base;
using Ticket.Base.Entities;
using SimpleStack.Orm.Attributes;
using System;
using System.Collections.Generic;

namespace Ticket.DataAccess.TicketService
{
    [TableWithSequence("work_order", SequenceName = "work_order_id_seq")]
    [Alias("work_order")]
    public partial class WorkOrder : IWorkOrder
    {
        public WorkOrder()
        {
            WorkOrderClarifications = new List<WorkOrderClarification>();
        }

        [Ignore]
        public string TicketNumber { get; set; }

        [PrimaryKey]
        [AutoIncrement]
        public long Id { get; set; }

        [System.ComponentModel.DataAnnotations.Range(1, long.MaxValue)]
        [Alias("client_id")]
        public long ClientId { get; set; }

        [Alias("assigned_user_role_id")]
        public long? AssignedUserRoleId { get; set; }

        [Alias("assigned_user_id")]
        public long? AssignedUserId { get; set; }

        [Alias("batch_name")]
        public string BatchName { get; set; }

        [Alias("scan_date")]
        public DateTime? ScanDate { get; set; }

        [Alias("page_no")]
        public long? PageNo { get; set; }

        [Alias("reference_no")]
        public string ReferenceNo { get; set; }

        [Alias("mr_no")]
        public string MRNo { get; set; }

        [Alias("patient_name")]
        public string PatientName { get; set; }

        [Alias("dos_date")]
        public DateTime? DOSDate { get; set; }

        [Alias("work_order_status_id")]
        [System.ComponentModel.DataAnnotations.Range(1, long.MaxValue)]
        public long WorkOrderStatusId { get; set; }

        public decimal? Amount { get; set; }

        [Alias("client_doctor_name")]
        public string ClientDoctorName { get; set; }

        [Alias("refering_doctor_name")]
        public string ReferingDoctorName { get; set; }

        [Alias("facility_id")]
        public long? FacilityId { get; set; }

        [Alias("process_id")]
        public long? ProcessId { get; set; }

        [Alias("request_type_id")]
        public long? RequestTypeId { get; set; }

        public string Comments { get; set; }

        [Alias("created_by")]
        public long? CreatedBy { get; set; }

        [Alias("created_date")]
        public DateTime? CreatedDate { get; set; }


        [Alias("modified_by")]
        public long? ModifiedBy { get; set; }

        [Alias("modified_date")]
        public DateTime? ModifiedDate { get; set; }

        [Ignore]
        [Alias("clarification_verified")]
        public bool ClarificationVerified { get; set; }


        [Ignore]
        [Alias("clarification_required")]
        public bool ClarificationRequired { get; set; }

        [Ignore]
        public string AssigneUserName { get; set; }

        [Ignore]
        public string TicketCreatedBy { get; set; }

        [Ignore]
        public string TicketStatusName { get; set; }

        [Ignore]
        public string RequestTypeName { get; set; }

        [Ignore]
        public string ProcessName { get; set; }

        [Ignore]
        public string FacilityName { get; set; }

        [Ignore]
        [Alias("coordinator_id")]
        public long? CoordinatorId { get; set; }

        [Ignore]
        public IEnumerable<IWorkOrderClarification> WorkOrderClarifications { get; set; }

        [Ignore]
        public string ClientName { get; set; }

        [Ignore]
        public string ClientAccountNumber { get; set; }

    }
}
