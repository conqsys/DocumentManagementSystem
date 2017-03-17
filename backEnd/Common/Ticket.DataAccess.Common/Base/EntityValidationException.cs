using Ticket.Base.Entities;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Ticket.DataAccess.Common
{
    public class EntityValidationException : TicketException
    {
        public override IValidationCodeResult ValidationCodeResult { get; set; }
        public EntityValidationException(string message, IEnumerable<IValidationResult> validationErrors) : base(message)
        {
            this.ValidationCodeResult = new EntityValidationCodeResult(validationErrors);
        }

    }

    public class EntityValidationCodeResult : IValidationCodeResult
    {
        public EntityValidationCodeResult(IEnumerable<IValidationResult> validationErrors)
        {
            this.ValidationErrors = validationErrors;
        }
        public IEnumerable<IValidationResult> ValidationErrors { get; private set; }
    }


    public class ErrorMessageResult : IValidationCodeResult
    {
        public string ErrorMessage { get; set; }
    }
    public interface IValidationCodeResult
    {

    }
}
