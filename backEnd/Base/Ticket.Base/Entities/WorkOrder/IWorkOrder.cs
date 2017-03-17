using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.Base.Entities
{
    public interface IWorkOrder : IEntity, IStamp
    {
        long ClientId { get; set; }
        long? AssignedUserRoleId { get; set; }
        long? AssignedUserId { get; set; }
        string BatchName { get; set; }
        DateTime? ScanDate { get; set; }
        long? PageNo { get; set; }
        string ReferenceNo { get; set; }
        string MRNo { get; set; }
        string PatientName { get; set; }
        DateTime? DOSDate { get; set; }
        long WorkOrderStatusId { get; set; }
        decimal? Amount { get; set; }
        string ClientDoctorName { get; set; }
        string ReferingDoctorName { get; set; }
        long? FacilityId { get; set; }
        long? ProcessId { get; set; }
        long? RequestTypeId { get; set; }
        string Comments { get; set; }
        bool ClarificationVerified { get; set; }
        bool ClarificationRequired { get; set; }
        string AssigneUserName { get; set; }
        string TicketCreatedBy { get; set; }
        string TicketStatusName { get; set; }
        string RequestTypeName { get; set; }
        string ProcessName { get; set; }
        string FacilityName { get; set; }


        IEnumerable<IWorkOrderClarification> WorkOrderClarifications { get; set; }
    }
}
