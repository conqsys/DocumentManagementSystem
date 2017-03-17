using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.Base.Entities
{
    public interface IValidationResult
    {
        string ErrorMessage { get; set; }

        IEnumerable<string> MemberNames { get; }

        int ErrorCode { get; set; }

    }
}
