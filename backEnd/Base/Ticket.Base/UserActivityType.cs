using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.Base
{
    public enum UserActivityType
    {
        None,
        NewUser,
        UpdateUser,
        ChangePassword,
        Authentication,
        LogOut,
        PasswordChange,
        UserEnable,
        UserDisable
    }
}
