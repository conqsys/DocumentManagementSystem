using Microsoft.AspNetCore.Mvc.Filters;
using Ticket.Base.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.API.Common
{
    public class UserStampFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var identity = context.HttpContext.User.Identity as TicketIdentity;

            var httpMethod = context.HttpContext.Request.Method;
            List<ICreatedStamp> createdStampEntities = new List<ICreatedStamp>();
            List<IStamp> stampEntities = new List<IStamp>();
            if (httpMethod == "POST" || httpMethod == "PUT" && identity != null)
            {
                foreach (var arg in context.ActionArguments)
                {
                    if (arg.Value is IStamp)
                    {
                        stampEntities.Add((IStamp)arg.Value);
                    }
                    else if (arg.Value is ICreatedStamp)
                    {
                        createdStampEntities = new List<ICreatedStamp>();
                    }
                }
            }

            if (createdStampEntities.Count > 0 || stampEntities.Count > 0)
            {
                foreach (var item in createdStampEntities)
                {
                    item.CreatedBy = identity.User.Id;
                    item.CreatedDate = DateTime.Now;

                }

                foreach (var item in stampEntities)
                {
                    if (httpMethod == "POST")
                    {
                        item.CreatedBy = identity.User.Id;
                        item.CreatedDate = DateTime.Now;
                    }

                    item.ModifiedBy = identity.User.Id;
                    item.ModifiedDate = DateTime.Now;
                }
            }

            base.OnActionExecuting(context);
        }


        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            base.OnResultExecuting(context);
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            base.OnResultExecuted(context);
        }
    }
}
