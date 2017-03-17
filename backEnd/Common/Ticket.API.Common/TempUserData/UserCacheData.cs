using Ticket.Base.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.API.Common
{
    public class UserCacheData
    {
        public UserCacheData()
        {
            InitialIze();
        }

        private Dictionary<string, IUser> _cachedObject;

        public bool Contains(string key)
        {
            return this._cachedObject.ContainsKey(key);
        }

        public IUser this[string key]
        {
            get
            {
                return this._cachedObject[key];
            }
        }
        private void InitialIze()
        {
            IdentityUser user = new IdentityUser(null, null);
            user.Id = 2;
            user.Email = "test@test.com";
            this._cachedObject = new Dictionary<string, IUser>();
            this._cachedObject.Add("2", user);

        }

    }
}
