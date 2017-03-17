using Ticket.DataAccess.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.DataAccess.TicketService
{
    public enum EnumErrorCode : Int32
    {
        RoleExists = 1000,
        LabExists = 1001,
        RoleDoestNotExist = 1002,
        UserRoleExists = 1003,
        RoleIdIsNull = 1004,
        LabNotActive = 1005,
        NewIpAddress = 1006,
        UserIdDoesNotExists = 1007,
        OldAndNewPwdNotSame = 1008,
        IncorrectUserName = 1009,
        PasswordUsedBefore = 1010,
        UserDoesNotExist = 1011,
        UserExists = 1012,
        NoRoleForAssigned = 1013,
        RecordDoesNotExist = 1014,
        VerificationCodeIsWrong = 1015,
        VerificationTimeExpired = 1016,
        AlreadyAuthenticated = 1017,
        PermissionDoestNotExist = 1018,
        PermissionIdIsNull = 1019,
        PermissionAlreadyExists = 1020,
        RequiredIntialLabUser = 1021,
        RoleExistInUser = 1022,
        UsePasswordExists = 1023,
        EmailDoesNotExists = 1024,
        ProcessTypeExists = 1025,
        TicketStatusExists = 1026,
        ClientNameExists = 1027,
        FacilityNameExists = 1028,
        ProcessNameExists = 1029,
        UserNameExists = 1030,
        UserEmailExists = 1031,
        RequesTypeExists = 1032,
        ClientAcroymnExists = 1033,
        WorkOrderNotExists = 1034,
        ClientExistsInWorkOrder = 1035,
        EntityUsedInProcess = 1036,
        EntityUsedInWorkOrder = 1037,
        EntityUsedInUser = 1038,
        EntityUsedInGroupUser = 1039,
        EntityUsedInClient = 1040

    }

    public enum TransactionEntityState
    {
        Added = 1,
        Modified = 2,
        Deleted = 3
    }

    public class ValidationErrorCodes : BaseValidationErrorCodes
    {
        public ValidationErrorCodes()
        {
            this.InitializeErrorCodes();
        }

        protected override void InitializeErrorCodes()
        {
            base.InitializeErrorCodes();
            this.AddErrorCode(EnumErrorCode.RoleExists, "Role already exists - {0}");//2000
            this.AddErrorCode(EnumErrorCode.LabExists, "Lab already exists - {0}");//2001
            this.AddErrorCode(EnumErrorCode.RoleDoestNotExist, "Role does not exist");//2002
            this.AddErrorCode(EnumErrorCode.UserRoleExists, "User Role already exists - {0}");//2003
            this.AddErrorCode(EnumErrorCode.RoleIdIsNull, "RoleId can not be set null");//2004
            this.AddErrorCode(EnumErrorCode.LabNotActive, "Lab is not Active");//2005
            this.AddErrorCode(EnumErrorCode.NewIpAddress, "New Ip Address");//2006
            this.AddErrorCode(EnumErrorCode.UserIdDoesNotExists, "User does not exist for the given userid - {0}");//2007
            this.AddErrorCode(EnumErrorCode.OldAndNewPwdNotSame, "Old password and new password are not same.");//2008
            this.AddErrorCode(EnumErrorCode.IncorrectUserName, "Incorrect Username.");//2009
            this.AddErrorCode(EnumErrorCode.PasswordUsedBefore, "Password has been used before."); //2010
            this.AddErrorCode(EnumErrorCode.UserDoesNotExist, "User does not exist."); //2011
            this.AddErrorCode(EnumErrorCode.UserExists, "User already exists - {0}");//2012
            this.AddErrorCode(EnumErrorCode.NoRoleForAssigned, "Please assign role to - {0}");//2013
            this.AddErrorCode(EnumErrorCode.RecordDoesNotExist, "Record does not exist"); //2014
            this.AddErrorCode(EnumErrorCode.VerificationCodeIsWrong, "Verification code is wrong."); //2015
            this.AddErrorCode(EnumErrorCode.VerificationTimeExpired, "Verification Time has been expired."); //2016
            this.AddErrorCode(EnumErrorCode.AlreadyAuthenticated, "Authentication already exists for - {0} and IpAdress - {1}");//2017
            this.AddErrorCode(EnumErrorCode.PermissionDoestNotExist, "Permission does not exist");//2018
            this.AddErrorCode(EnumErrorCode.PermissionIdIsNull, "Permission can not be set null");//2019
            this.AddErrorCode(EnumErrorCode.PermissionAlreadyExists, "Permission already exists - {0}");//2020
            this.AddErrorCode(EnumErrorCode.RequiredIntialLabUser, "Required initial lab admin.");//2021         
            this.AddErrorCode(EnumErrorCode.RoleExistInUser, "Role exist in User, can not be deleted.");//2022
            this.AddErrorCode(EnumErrorCode.UsePasswordExists, "User's Old Password does not match");//1023
            this.AddErrorCode(EnumErrorCode.EmailDoesNotExists, "User does not exists or this email Id");//1024
            this.AddErrorCode(EnumErrorCode.ProcessTypeExists, "Process type is already exists");//1025
            this.AddErrorCode(EnumErrorCode.TicketStatusExists, "Ticket status is already exists");//1026
            this.AddErrorCode(EnumErrorCode.ClientNameExists, "Client name is already exists");//1027
            this.AddErrorCode(EnumErrorCode.FacilityNameExists, "Facility name is already exists");//1028
            this.AddErrorCode(EnumErrorCode.ProcessNameExists, "Process name is already exists");//1029
            this.AddErrorCode(EnumErrorCode.UserNameExists, "User name is already exists");//1030
            this.AddErrorCode(EnumErrorCode.UserEmailExists, "User email is already exists");//1031
            this.AddErrorCode(EnumErrorCode.RequesTypeExists, "Request type is already exists");//1032
            this.AddErrorCode(EnumErrorCode.ClientAcroymnExists, "Client acroynm is already exists");//1033
            this.AddErrorCode(EnumErrorCode.WorkOrderNotExists, "Work order does not exists");//1034
            this.AddErrorCode(EnumErrorCode.ClientExistsInWorkOrder, "Can't delete, used in work order.");//1035
            this.AddErrorCode(EnumErrorCode.EntityUsedInProcess, "Can't delete, used in process.");//1036
            this.AddErrorCode(EnumErrorCode.EntityUsedInWorkOrder, "Can't delete, used in work order.");//1037
            this.AddErrorCode(EnumErrorCode.EntityUsedInUser, "Can't delete, used in user.");//1038
            this.AddErrorCode(EnumErrorCode.EntityUsedInGroupUser, "Can't delete, used in group user.");//1039
            this.AddErrorCode(EnumErrorCode.EntityUsedInClient, "Can't delete, used in client.");//1040

        }

        public void AddErrorCode(EnumErrorCode errorCode, string message)
        {
            base.AddErrorCode((int)errorCode, message);
        }

        public KeyValuePair<int, string> this[EnumErrorCode errorCode, params object[] formatter]
        {
            get
            {
                return base[(int)errorCode, formatter];
            }
        }


    }
}
