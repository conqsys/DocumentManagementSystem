using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ticket.Base;
using Ticket.Base.Repositories;
using Ticket.Base.Entities;

namespace Ticket.API.Common
{
    public interface ISecuredIdentityController
    {
        TicketIdentity Identity { get; }

        TicketPrincipal User { get; }
    }

    [AuthorizeTicket]
    public abstract class SecuredRepositoryController<TRepo> : BaseRepositoryController<TRepo>, ISecuredIdentityController
         where TRepo : class, IDepRepository
    {
        public SecuredRepositoryController(TRepo repository) : base(repository) { }

        public TicketIdentity Identity
        {
            get
            {
                return (TicketIdentity)this.Request.HttpContext.User.Identity;
            }
        }

        public new TicketPrincipal User
        {
            get
            {
                return this.Identity.Principal;
            }
        }
    }


    public abstract class UnSecureRepositoryController<TRepo> : BaseRepositoryController<TRepo>, ISecuredIdentityController
        where TRepo : class, IDepRepository
    {
        public UnSecureRepositoryController(TRepo repository) : base(repository) { }

        public TicketIdentity Identity
        {
            get
            {
                return (TicketIdentity)this.Request.HttpContext.User.Identity;
            }
        }

        public new TicketPrincipal User
        {
            get
            {
                return this.Identity.Principal;
            }
        }
    }


    [AuthorizeTicket]
    public abstract class SecuredServiceController : Controller, ISecuredIdentityController
    {

        public TicketIdentity Identity
        {
            get
            {
                return (TicketIdentity)this.Request.HttpContext.User.Identity;
            }
        }

        public new TicketPrincipal User
        {
            get
            {
                return this.Identity.Principal;
            }
        }
    }
}
