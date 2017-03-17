using Ticket.Base;
using Ticket.Base.Entities;
using SimpleStack.Orm.Attributes;
using System;

namespace Ticket.DataAccess.TicketService
{
    [TableWithSequence("work_order_clarification", SequenceName = "work_order_clearification_id_seq")]
    [Alias("work_order_clarification")]
    public partial class WorkOrderClarification : IWorkOrderClarification
    {
        public WorkOrderClarification()
        {
        }

        [PrimaryKey]
        [AutoIncrement]
        public long Id { get; set; }

        [System.ComponentModel.DataAnnotations.Range(1, long.MaxValue)]
        [Alias("work_order_id")]
        public long WorkOrderId { get; set; }


        [Alias("clarification_question_comment")]
        public string ClarificationQuestionComment { get; set; }

        [Alias("clarification_question_by")]
        public long? ClarificationQuestionBy { get; set; }


        [Alias("clarification_answer_comment")]
        public string clarificationAnswerComment { get; set; }

        [Alias("clarification_answer_by")]
        public long? ClarificationAnswerBy { get; set; }


        [Alias("clarification_answer_date")]
        public DateTime? ClarificationAnswerDate { get; set; }


        [Alias("clarification_question_date")]
        public DateTime? ClarificationQuestionDate { get; set; }

        [Alias("clarification_verified_comment")]
        public string ClarificationVerifiedComment { get; set; }

        [Alias("clarification_verified_date")]
        public DateTime? ClarificationVerifiedDate { get; set; }

        [Alias("clarification_required")]
        [System.ComponentModel.DataAnnotations.Required]
        public bool ClarificationRequired { get; set; }

        [Alias("clarification_verified")]
        [System.ComponentModel.DataAnnotations.Required]
        public bool ClarificationVerified { get; set; }

        [Alias("clarification_verified_by")]
        public long? ClarificationVerifiedBy { get; set; }

        [Alias("question_assign_to_user_id")]
        public long? QuestionAssignToUserId { get; set; }

        [Ignore]
        public string ClarificationQuestionUserName { get; set; }

        [Ignore]
        public string ClarificationAnswerUserName { get; set; }

        [Ignore]
        public string ClarificationVerifyUserName { get; set; }

        [Alias("question_assign_to_user_role_id")]
        public long? QuestionAssignToUserRoleId { get; set; }

    }
}
