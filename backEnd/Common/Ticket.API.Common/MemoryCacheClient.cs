using Ticket.Base.Entities;
using Ticket.Base.Repositories;
using Ticket.DataAccess.Common;
using ServiceStack.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.API.Common
{
    public class MemoryCacheClient : ICacheClient
    {
        private Dictionary<string, object> _cachedObject;
        private UserCacheData _userCache;
        public MemoryCacheClient(UserCacheData userCache)
        {
            this._userCache = userCache;
            this._cachedObject = new Dictionary<string, object>();
        }

        public bool Add<T>(string key, T value)
        {
            throw new NotImplementedException();
        }

        public bool Add<T>(string key, T value, TimeSpan expiresIn)
        {
            throw new NotImplementedException();
        }

        public bool Add<T>(string key, T value, DateTime expiresAt)
        {
            throw new NotImplementedException();
        }

        public long Decrement(string key, uint amount)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }

        public void FlushAll()
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string key)
        {
            if (this._cachedObject.ContainsKey(key))
            {
                return (T)this._cachedObject[key];
            }
            else
            {
                if (this._userCache.Contains(key))
                {
                    return (T)this._userCache[key];
                }
                else
                {
                return default(T);
                 }
                //var user = this._userRepository.FindUserWithRoles(long.Parse(key)).GetAwaiter().GetResult();
                //this.Set<IUser>(key, user);
                //return (T)user;
            }
        }


        public IDictionary<string, T> GetAll<T>(IEnumerable<string> keys)
        {
            throw new NotImplementedException();
        }

        public long Increment(string key, uint amount)
        {
            throw new NotImplementedException();
        }

        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public void RemoveAll(IEnumerable<string> keys)
        {
            throw new NotImplementedException();
        }

        public bool Replace<T>(string key, T value)
        {
            throw new NotImplementedException();
        }

        public bool Replace<T>(string key, T value, TimeSpan expiresIn)
        {
            throw new NotImplementedException();
        }

        public bool Replace<T>(string key, T value, DateTime expiresAt)
        {
            throw new NotImplementedException();
        }


        public bool Set<T>(string key, T value)
        {
            this._cachedObject.Add(key, value);
            return true;
        }

        public bool Set<T>(string key, T value, TimeSpan expiresIn)
        {
            throw new NotImplementedException();
        }

        public bool Set<T>(string key, T value, DateTime expiresAt)
        {
            throw new NotImplementedException();
        }

        public void SetAll<T>(IDictionary<string, T> values)
        {
            throw new NotImplementedException();
        }
    }
}
