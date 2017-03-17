using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.DataAccess.Common
{

    public enum TransactionEntityState
    {
        Added = 1,
        Modified = 2,
        Deleted = 3
    }

    public enum EnumErrorBaseCode : Int32
    {
        NewVersionExists = 500,
        VersionMismatched = 501
    }

    public abstract class BaseValidationErrorCodes
    {
        public BaseValidationErrorCodes()
        {
            this.ErrorCodes = new Dictionary<int, string>();

        }

        protected virtual void InitializeErrorCodes()
        {
            this.AddErrorCode((int)EnumErrorBaseCode.NewVersionExists, "Record updated to a newer version before this update");//4028
            this.AddErrorCode((int)EnumErrorBaseCode.VersionMismatched, "Record Version mismatched");//4028
        }

        public virtual void AddErrorCode(Int32 errorCode, string message)
        {
            this.ErrorCodes.Add((int)errorCode, message);
        }

        public virtual KeyValuePair<int, string> this[int errorCode, params object[] formatter]
        {
            get
            {
                string errorMessage = this.ErrorCodes[(int)errorCode];
                var returnValue = new KeyValuePair<int, string>((int)errorCode, formatter.Length > 0 ? string.Format(errorMessage, formatter) : errorMessage);
                return returnValue;
            }
        }



        public Dictionary<int, string> ErrorCodes { get; set; }
    }
}
