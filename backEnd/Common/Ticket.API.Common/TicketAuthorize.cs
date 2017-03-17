using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.API.Common
{
   
    public class AuthorizeTicket : AuthorizeAttribute
    {
       // public string Roles { get; set; }

            
        public AuthorizeTicket(): base()
        {
            //this.Roles =  "LabUser,LabAdmin,SysAdmin";
        }        
    }

    public class AuthorizeLabAdmin : AuthorizeTicket
    {
        public AuthorizeLabAdmin(): base()
        {
            this.Roles = "LabAdmin,SysAdmin";
        }
    }

    public class AuthorizeSystemAdmin : AuthorizeTicket
    {
        public AuthorizeSystemAdmin() : base()
        {
            this.Roles = "SysAdmin";
        }
    }

}
