
using Ticket.Base.Entities;
using Ticket.DataAccess.Common;

namespace Ticket.BusinessLogic.TicketService
{
    public class EmailTemplateRepository : ModuleBaseRepository<IEmailTemplate>

    {
        public EmailTemplateRepository(BaseValidationErrorCodes errorCodes,
                                                    DatabaseContext dbContext,
                                                    IUser loggedUser) : base(errorCodes, dbContext, loggedUser)
        {

        }

        //public override IEmailTemplate FindById(long id)
        //{
        //    return base.FindById(id);
        //}

        public IEmailTemplate GetTemplate(int eventType)
        {
            return this.Connection.FirstOrDefault<IEmailTemplate>(i => i.EventType == eventType);
        }

    }
}
