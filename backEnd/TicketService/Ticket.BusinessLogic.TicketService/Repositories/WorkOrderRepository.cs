
using Dapper;
using Ticket.Base.Entities;
using Ticket.Base.Repositories;
using Ticket.DataAccess.Common;
using Npgsql;
using SimpleStack.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ticket.BusinessLogic.Common;
using Ticket.DataAccess.TicketService;

namespace Ticket.BusinessLogic.TicketService
{
    public class WorkOrderRepository<TWorkOrder, TWorkOrderStatusHistory, TWorkOrderClarification, TWorkOrderAttachment, TUser> : ModuleBaseRepository<TWorkOrder>, IWorkOrderRepository
        where TWorkOrder : class, IWorkOrder, new()
        where TWorkOrderStatusHistory : class, IWorkOrderStatusHistory, new()
        where TWorkOrderClarification : class, IWorkOrderClarification, new()
        where TWorkOrderAttachment : class, IWorkOrderAttachment, new()
          where TUser : class, IUser, new()

    {
        IWorkOrderStatusHistoryRepository _workOrderStatusHistoryRepository;
        IWorkOrderClarificationRepository _workOrderClarificationRepository;
        public WorkOrderRepository(BaseValidationErrorCodes errorCodes,
                                                        DatabaseContext dbContext,
                                                        IUser loggedUser,
                                                        IUserRepository userRepository,
                                                        IRoleRepository roleRepository,
                                                        EncryptionProvider encryptionProvider,
                                                        IWorkOrderStatusHistoryRepository workOrderStatusHistoryRepository,
                                                        IWorkOrderClarificationRepository workOrderClarificationRepository)
            : base(errorCodes, dbContext, loggedUser)
        {
            this._workOrderStatusHistoryRepository = workOrderStatusHistoryRepository;
            this._workOrderClarificationRepository = workOrderClarificationRepository;
        }


        public async Task<IWorkOrder> AddNew(IWorkOrder entity, long loggedUserId)
        {
            TWorkOrder tEntity = entity as TWorkOrder;
            var errors = await this.ValidateEntityToAdd(tEntity);
            if (errors.Count() > 0)
                await this.ThrowEntityException(errors);
            try
            {
                this.StartTransaction();
                if (tEntity.AssignedUserId > 0)
                {
                    tEntity.WorkOrderStatusId = 2;
                }

                tEntity.CreatedBy = loggedUserId;
                tEntity.CreatedDate = DateTime.Now;
                var savedEntity = await base.AddNew(tEntity);
                long workOrderId = savedEntity.Id;
                this.CommitTransaction();
                await this.SaveTicketStatusHistory(workOrderId, true, tEntity.WorkOrderStatusId, loggedUserId);
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

        public async Task<bool> SaveTicketStatusHistory(long workOrderId, bool isNew, long workOrderStatus, long loggedUserId)
        {
            IWorkOrderStatusHistory workOrderStatusHistory = null;
            workOrderStatusHistory = new WorkOrderStatusHistory();
            workOrderStatusHistory.WorkOrderId = workOrderId;
            workOrderStatusHistory.CreatedBy = loggedUserId;
            workOrderStatusHistory.CreatedDate = DateTime.UtcNow;

            if (isNew)
            {
                workOrderStatusHistory.WorkOrderStatusId = 1;
                await this._workOrderStatusHistoryRepository.AddNew(workOrderStatusHistory);

                if (workOrderStatus == 2)
                {
                    workOrderStatusHistory.WorkOrderStatusId = 2;
                    await this._workOrderStatusHistoryRepository.AddNew(workOrderStatusHistory);
                }
            }
            else
            {
                workOrderStatusHistory.WorkOrderStatusId = workOrderStatus;
                await this._workOrderStatusHistoryRepository.AddNew(workOrderStatusHistory);
            }
            return true;

        }

        public async Task<IWorkOrder> Update(IWorkOrder entity, long loggedUserId)
        {
            try
            {
                TWorkOrder tEntity = entity as TWorkOrder;

                var errors = await this.ValidateEntityToUpdate(tEntity);
                if (errors.Count() > 0)
                    await this.ThrowEntityException(errors);

                var isSaveStatusHistoryRequired = false;
                if (tEntity.WorkOrderStatusId == 1 && tEntity.AssignedUserId > 0)
                {
                    tEntity.WorkOrderStatusId = 2;
                    isSaveStatusHistoryRequired = true;
                }

                tEntity.ModifiedBy = loggedUserId;
                tEntity.ModifiedDate = DateTime.Now;

                this.StartTransaction();
                await base.Update(tEntity, x => new
                {
                    x.AssignedUserRoleId,
                    x.AssignedUserId,
                    x.BatchName,
                    x.ScanDate,
                    x.PageNo,
                    x.ReferenceNo,
                    x.MRNo,
                    x.PatientName,
                    x.DOSDate,
                    x.Amount,
                    x.ClientDoctorName,
                    x.ReferingDoctorName,
                    x.Comments,
                    x.WorkOrderStatusId,
                    x.RequestTypeId,
                    x.ProcessId,
                    x.FacilityId,
                    x.ModifiedBy,
                    x.ModifiedDate
                });

                if (isSaveStatusHistoryRequired)
                {
                    await this.SaveTicketStatusHistory(tEntity.Id, false, tEntity.WorkOrderStatusId, loggedUserId);
                }

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

        public async Task<IEnumerable<IWorkOrder>> FindAllWorkOrders(long loggedUserId, long loggedUserRoleId, long? loggedUserCoordinatorId, long? loddedUserAccountId)
        {
            if (loggedUserCoordinatorId == null)
            {
                loggedUserCoordinatorId = 0;
            }


            if (loddedUserAccountId == null)
            {
                loddedUserAccountId = 0;
            }

            var sqlQuery = @"WITH wo_verification AS (
                            SELECT   work_order.id woId,
			                         work_order_clarification.clarification_verified
			                        from work_order_clarification 
			                        INNER JOIN work_order ON work_order_clarification.work_order_id = work_order.id 
			                        WHERE work_order.Assigned_user_id = " + loggedUserId + " AND work_order_clarification.clarification_verified = false " +
                                     "AND work_order_clarification.clarification_required = true " +
                                    "ORDER BY work_order_clarification.id  Desc LIMIT 1 )," +
                                    "wo_clarification AS ( " +
                                    " SELECT work_order.id woId,  work_order_clarification.clarification_required " +
                                    " from work_order_clarification " +
                                    "INNER JOIN work_order ON work_order_clarification.work_order_id = work_order.id WHERE " +
                                    "((work_order_clarification.question_assign_to_user_id IN (select  id from public.user where  client_id = " + loddedUserAccountId + " ) OR " +
                                    "work_order_clarification.question_assign_to_user_id =" + loggedUserId + 
                                    ") AND work_order_clarification.clarification_required = false)" +
                                    "ORDER BY work_order_clarification.id  Desc LIMIT 1 )" +

                                    "SELECT '#' || wok.id AS TicketNumber, wok.id as Id," +
                                    "wok.client_id as ClientId," +
                                    "cli.client_name as ClientName," +
                                    "cli.account_number as ClientAccountNumber," +
                                    "wok.assigned_user_role_id as AssignedUserRoleId," +
                                    "wok.assigned_user_id as AssignedUserId," +
                                    "wok.batch_name as BatchName," +
                                    "wok.scan_date as ScanDate," +
                                    "wok.page_no as PageNo," +
                                    "wok.reference_no as ReferenceNo," +
                                    "wok.mr_no as MRNo," +
                                    "wok.patient_name as PatientName," +
                                    "wok.dos_date as DOSDate," +
                                    "wok.amount as Amount ," +
                                    "wok.client_doctor_name as ClientDoctorName," +
                                    "wok.refering_doctor_name as ReferingDoctorName," +
                                    "wok.comments," +
                                    "wok.work_order_status_id as WorkOrderStatusId," +
                                    "wok.request_type_id as RequestTypeId," +
                                    "wok.process_id as ProcessId," +
                                    "wok.facility_id as FacilityId," +
                                    "wok.created_by as CreatedBy," +
                                    "coalesce(sa.clarification_verified, false) AS ClarificationVerified , " +
                                    "coalesce(sad.clarification_required, false) AS ClarificationRequired , " +
                                    "assigne.user_name AS AssigneUserName ," +
                                    "creater.user_name AS TicketCreatedBy," +
                                    "wos.name AS TicketStatusName," +
                                    "req.name AS RequestTypeName," +
                                    "proc.name AS ProcessName," +
                                    "fac.name AS FacilityName " +
                                    "FROM work_order wok " +
                                    "LEFT JOIN public.user  creater ON  wok.created_by =  creater.id " +
                                    "LEFT JOIN public.client  cli ON  cli.id =  wok.client_id " +
                                    "LEFT JOIN public.user  assigne ON wok.assigned_user_id = assigne.id " +
                                    "LEFT JOIN role rol ON rol.id = wok.assigned_user_role_id " +
                                    "LEFT JOIN wo_verification sa ON sa.woId = wok.id " +
                                    "LEFT JOIN wo_clarification sad ON sad.WoId = wok.id " +
                                    "LEFT JOIN ticket_status wos ON wos.id = wok.work_order_status_id " +
                                    "LEFT JOIN request_type req ON req.id = wok.request_type_id " +
                                    "LEFT JOIN process proc ON proc.id = wok.process_id " +
                                    "LEFT JOIN facility fac ON fac.id = wok.facility_id " +
                                    " WHERE (wok.created_by = " + loggedUserId + " OR (" + loggedUserRoleId + " = 1 AND assigne.coordinator_id =" + loggedUserId + " OR   creater.coordinator_id =" + loggedUserId + " ) " +
                                                " OR (" + loggedUserRoleId + " = 2  AND (wok.assigned_user_id = " + loggedUserCoordinatorId + " OR assigne.coordinator_id = " + loggedUserCoordinatorId +
                                                " OR wok.created_by = " + loggedUserCoordinatorId + "OR  creater.coordinator_id =" + loggedUserCoordinatorId + "  )) OR (" + loggedUserRoleId + " = 3  AND wok.client_id = " + loddedUserAccountId + "))";


            var result = await this.Connection.QueryAsync<TWorkOrder>(sqlQuery);
            return result;

        }

        public async Task<int> Delete(long workOrderId)
        {
            try
            {
                ICollection<ValidationCodeResult> errors = new List<ValidationCodeResult>();
                this.StartTransaction();
                await this.Connection.DeleteAllAsync<TWorkOrderAttachment>(i => i.Id == workOrderId);
                await this.Connection.DeleteAllAsync<TWorkOrderStatusHistory>(i => i.Id == workOrderId);
                await this.Connection.DeleteAllAsync<TWorkOrderClarification>(i => i.Id == workOrderId);
                int deletedRecords = await this.Connection.DeleteAllAsync<TWorkOrder>(i => i.Id == workOrderId);
                this.CommitTransaction();
                return deletedRecords;
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

        public async Task<IWorkOrder> FindWorkOrderById(long id)
        {
            // var workOrder = entity as TWorkOrder;
            var sqlQuery = new JoinSqlBuilder<TUser, TWorkOrder>(this.Connection.DialectProvider)
                             .LeftJoin<TUser, TWorkOrder>(i => i.Id, i => i.AssignedUserId)
                             .SelectAll<TWorkOrder>()
                             .Select<TUser>(p => new { CoordinatorId = p.CoordinatorId })
                              .Where<TWorkOrder>(ri => ri.Id == id)
                             .ToSql();

            var result = await this.Connection.QueryFirstAsync<TWorkOrder>(sqlQuery, new { p_0 = id });

            IEnumerable<IWorkOrderClarification> workOrderClarifications = await this._workOrderClarificationRepository.FindClarificationsByWorkOrderId(id);

            if (result != null)
            {
                result.WorkOrderClarifications = workOrderClarifications;
            }
            return result;
        }

        public override async Task<IEnumerable<IValidationResult>> ValidateEntityToAdd(TWorkOrder entity)
        {
            ICollection<IValidationResult> errors = (await this.ValidateEntity(entity)).ToList();
            return errors;
        }

        public override async Task<IEnumerable<IValidationResult>> ValidateEntityToUpdate(TWorkOrder entity)
        {
            ICollection<IValidationResult> errors = (await this.ValidateEntity(entity)).ToList();
            var workOrder = await this.FindById(entity.Id) as TWorkOrder;
            if (workOrder == null)
            {
                errors.Add(new ValidationCodeResult(ErrorCodes[EnumErrorCode.WorkOrderNotExists, entity.Id + "#"]));
            }
            return errors;

        }



    }
}
