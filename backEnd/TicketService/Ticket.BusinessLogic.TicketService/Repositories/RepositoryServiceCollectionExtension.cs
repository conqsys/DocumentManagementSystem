using Ticket.Base.Repositories;
using Ticket.BusinessLogic.Common;
using Ticket.BusinessLogic.TicketService;
using Ticket.DataAccess.TicketService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class TicketServiceRepositoryCollectionExtension
    {
        public static IServiceCollection AddTicketServiceRepositories(this IServiceCollection services)
        {

            services.AddScoped<IUserRepository, UserRepository<User, Role>>();
            services.AddScoped<IRoleRepository, RoleRepository<Role>>();
            services.AddScoped<IGroupUserRepository, GroupUserRepository<GroupUser>>();
            services.AddScoped<IRequestTypeRepository, RequestTypeRepository<RequestType, ProcessRequestType, WorkOrder, Process>>();
            services.AddScoped<ITicketStatusRepository, TicketStatusRepository<TicketStatus, WorkOrder>>();
            services.AddScoped<IFacilityRepository, FacilityRepository<Facility, ClientFacility, Client, WorkOrder>>();
            services.AddScoped<IProcessRepository, ProcessRepository<Process>>();
            services.AddScoped<IClientRepository, ClientRepository<Client, WorkOrder>>();
            services.AddScoped<IWorkOrderRepository, WorkOrderRepository<WorkOrder, WorkOrderStatusHistory, WorkOrderClarification, WorkOrderAttachment, User>>();
            services.AddScoped<IClientFacilityRepository, ClientFacilityRepository<ClientFacility>>();
            services.AddScoped<IProcessRequestTypeRepository, ProcessRequestTypeRepository<ProcessRequestType>>();
            services.AddScoped<IWorkOrderClarificationRepository, WorkOrderClarificationRepository<WorkOrderClarification, WorkOrderClarification, WorkOrderClarification, User>>();
            services.AddScoped<IWorkOrderStatusHistoryRepository, WorkOrderStatusHistoryRepository<WorkOrderStatusHistory>>();
            services.AddScoped<IFileIndexesRepository, FileIndexesRepository<FileIndexes>>();
            services.AddScoped<IImportFileRepository, ImportFileRepository<ImportFile>>();
            services.AddScoped<IUserDetailRepository, UserDetailRepository<UserDetail>>();
            services.AddScoped<IQueueRepository, QueueRepository<Queue>>();
            
            services.AddScoped<EmailTemplateRepository>();
            return services;
        }
    }
}
