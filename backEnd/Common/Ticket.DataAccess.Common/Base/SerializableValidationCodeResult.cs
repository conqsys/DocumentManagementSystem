
using System.Collections.Generic;

namespace Ticket.DataAccess.Common
{
    public class SerializableValidationCodeResult
    {
        //
        // Summary:
        //     Gets the error message for the validation.
        //
        // Returns:
        //     The error message for the validation.
        public string ErrorMessage { get; set; }
        //
        // Summary:
        //     Gets the collection of member names that indicate which fields have validation
        //     errors.
        //
        // Returns:
        //     The collection of member names that indicate which fields have validation errors.
        public IEnumerable<string> MemberNames { get; set; }

        public int ErrorCode { get; set; }
    }

    public class SerializableEntityValidationCodeResult : IValidationCodeResult
    {
        public IEnumerable<SerializableValidationCodeResult> ValidationErrors { get; set; }
    }
}
