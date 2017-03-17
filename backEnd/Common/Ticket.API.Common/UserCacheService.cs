using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ticket.Base.Entities;
using System.Text;
using ServiceStack.Redis;
using ServiceStack.Caching;
using System.Threading;

namespace Ticket.API.Common
{
    public class UserCacheService<T> where T : IUser
    {
        private ICacheClient _cacheClient;



        public UserCacheService(ICacheClient cacheClient)
        {
            this._cacheClient = cacheClient;
        }

        public void Set(T user)
        {
            this._cacheClient.Set<T>(user.Id.ToString(), user);

        }

        public T Get(string id)
        {
            while (true)
            {
                try
                {
                    return this._cacheClient.Get<T>(id.ToString());                    
                }
                catch
                {
                    Thread.Sleep(10);
                }
            }

        }

    }
}
