
using Dapper;
using Ticket.Base.Entities;
using Ticket.Base.Repositories;
using Ticket.DataAccess.Common;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ticket.DataAccess.TicketService;

namespace Ticket.BusinessLogic.TicketService
{
    public class WorkOrderClarificationRepository<TWorkOrderClarification, TWorkOrderClarificationAnswer, TWorkOrderClarificationVerify, TUser> : ModuleBaseRepository<TWorkOrderClarification>, IWorkOrderClarificationRepository
        where TWorkOrderClarification : class, IWorkOrderClarification, new()
         where TWorkOrderClarificationAnswer : class, IWorkOrderClarification, new()
         where TWorkOrderClarificationVerify : class, IWorkOrderClarification, new()

         where TUser : class, IUser, new()
    {
        public WorkOrderClarificationRepository(BaseValidationErrorCodes errorCodes,
                                                        DatabaseContext dbContext,
                                                        IUser loggedUser
                                                       )
            : base(errorCodes, dbContext, loggedUser)
        {
        }


        public async Task<IWorkOrderClarification> AddNew(IWorkOrderClarification entity, long loggedUserId)
        {
            TWorkOrderClarification tEntity = entity as TWorkOrderClarification;
            var errors = await this.ValidateEntityToAdd(tEntity);
            if (errors.Count() > 0)
                await this.ThrowEntityException(errors);

            try
            {
                this.StartTransaction();
                entity.ClarificationRequired = false;
                entity.ClarificationVerified = false;
                entity.ClarificationQuestionBy = loggedUserId;
                entity.ClarificationQuestionDate = DateTime.Now;
                var savedEntity = await base.AddNew(entity as TWorkOrderClarification);
                this.CommitTransaction();
                return savedEntity;
            }
            catch (PostgresException ex)
            {
                throw new EntityUpdateException(ex);
            }
            catch
            {
                throw;
            }
        }


        public async Task<IWorkOrderClarification> Update(IWorkOrderClarification entity, long loggedUserId)
        {
            try
            {
                TWorkOrderClarification tEntity = entity as TWorkOrderClarification;
                var errors = await this.ValidateEntityToUpdate(tEntity);
                if (errors.Count() > 0)
                    await this.ThrowEntityException(errors);

                if (tEntity.clarificationAnswerComment != "" && tEntity.clarificationAnswerComment != null)
                {
                    tEntity.ClarificationRequired = true;
                    tEntity.ClarificationAnswerDate = DateTime.Now;
                    tEntity.ClarificationAnswerBy = loggedUserId;
                }


                this.StartTransaction();
                await base.Update(tEntity, x => new
                {
                    x.QuestionAssignToUserId,
                    x.clarificationAnswerComment,
                    x.ClarificationAnswerDate,
                    x.ClarificationAnswerBy,
                    x.ClarificationRequired
                });

                this.CommitTransaction();
                return tEntity;
            }
            catch (PostgresException ex)
            {
                throw new EntityUpdateException(ex);
            }
            catch
            {
                throw;
            }
        }


        public async Task<IWorkOrderClarification> VerifyClarification(IWorkOrderClarification entity, long loggedUserId)
        {
            try
            {
                TWorkOrderClarification tEntity = entity as TWorkOrderClarification;
                var errors = await this.ValidateEntityToUpdate(tEntity);
                if (errors.Count() > 0)
                    await this.ThrowEntityException(errors);

                tEntity.ClarificationVerified = true;
                tEntity.ClarificationVerifiedComment = "OK , Verified";
                tEntity.ClarificationVerifiedDate = DateTime.Now;
                tEntity.ClarificationVerifiedBy = loggedUserId;


                this.StartTransaction();
                await base.Update(tEntity, x => new
                {
                    x.ClarificationVerified,
                    x.ClarificationVerifiedComment,
                    x.ClarificationVerifiedDate,
                    x.ClarificationVerifiedBy
                });

                this.CommitTransaction();
                return tEntity;
            }
            catch (PostgresException ex)
            {
                throw new EntityUpdateException(ex);
            }
            catch
            {
                throw;
            }
        }

        public async Task<IWorkOrderClarification> ReAssignClarificationAnwserer(IWorkOrderClarification entity, long loggedUserId)
        {
            try
            {
                TWorkOrderClarification tEntity = entity as TWorkOrderClarification;
                var errors = await this.ValidateEntityToUpdate(tEntity);
                if (errors.Count() > 0)
                    await this.ThrowEntityException(errors);

                this.StartTransaction();
                await base.Update(tEntity, x => new
                {
                    x.QuestionAssignToUserId,
                    x.QuestionAssignToUserRoleId,
                });

                this.CommitTransaction();
                return tEntity;
            }
            catch (PostgresException ex)
            {
                throw new EntityUpdateException(ex);
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<IWorkOrderClarification>> FindClarificationsByWorkOrderId(long workOrderId)
        {
            var sqlQuery = @" SELECT clr.id,
 		                        clr.work_order_id AS workorderid,
                                clr.clarification_question_comment AS clarificationquestioncomment,
                                clr_ques.id AS clarificationquestionby,
                                clr.clarification_answer_comment AS clarificationanswercomment,
                                clr_ans.id AS clarificationanswerby,
                                clr.clarification_answer_date AS clarificationanswerdate,
                                clr.clarification_question_date AS clarificationquestiondate,
                                clr.clarification_verified_comment AS clarificationverifiedcomment,
                                clr.clarification_verified_date AS clarificationverifieddate,
                                clr.clarification_required AS clarificationrequired,
                                clr.clarification_verified AS clarificationverified,
                                clr.question_assign_to_user_id AS questionassigntouserid,
                                clr.question_assign_to_user_role_id AS questionassigntouserroleid,   
                                clr_ver.id AS clarificationverifiedby,
                                clr_ques.user_name AS clarificationquestionusername,
                                clr_ans.user_name AS clarificationanswerusername,
                                clr_ver.user_name AS clarificationverifyusername
		                        FROM work_order_clarification clr
                                LEFT OUTER JOIN  public.user clr_ques ON clr_ques.id = clr.clarification_question_by
                                LEFT OUTER JOIN  public.user clr_ans ON clr_ans.id = clr.clarification_answer_by  
                                LEFT OUTER JOIN  public.user clr_ver ON clr_ver.id = clr.clarification_verified_by
                                WHERE clr.work_order_id =" + workOrderId + "Order By id Asc";

            var result = await this.Connection.QueryAsync<TWorkOrderClarification>(sqlQuery);
            return result;
        }

        public override async Task<IEnumerable<IValidationResult>> ValidateEntityToAdd(TWorkOrderClarification entity)
        {
            ICollection<IValidationResult> errors = (await this.ValidateEntity(entity)).ToList();
            return errors;
        }

        public override async Task<IEnumerable<IValidationResult>> ValidateEntityToUpdate(TWorkOrderClarification entity)
        {
            ICollection<IValidationResult> errors = (await this.ValidateEntity(entity)).ToList();
            var workOrder = await this.FindById(entity.Id) as TWorkOrderClarification;
            if (workOrder == null)
            {
                errors.Add(new ValidationCodeResult(ErrorCodes[EnumErrorCode.WorkOrderNotExists, entity.Id]));
            }
            return errors;

        }

    }
}
