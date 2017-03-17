using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.Base.Entities
{
    public interface IWorkOrderClarification : IEntity
    {
        long WorkOrderId { get; set; }

        string ClarificationQuestionComment { get; set; }

        long? ClarificationQuestionBy { get; set; }

        string clarificationAnswerComment { get; set; }

        long? ClarificationAnswerBy { get; set; }

        DateTime? ClarificationAnswerDate { get; set; }

        DateTime? ClarificationQuestionDate { get; set; }

        string ClarificationVerifiedComment { get; set; }

        DateTime? ClarificationVerifiedDate { get; set; }

        bool ClarificationRequired { get; set; }

        bool ClarificationVerified { get; set; }

        long? ClarificationVerifiedBy { get; set; }

        long? QuestionAssignToUserId { get; set; }

        string ClarificationQuestionUserName { get; set; }

        string ClarificationAnswerUserName { get; set; }

        string ClarificationVerifyUserName { get; set; }

        long? QuestionAssignToUserRoleId { get; set; }
    }
}
