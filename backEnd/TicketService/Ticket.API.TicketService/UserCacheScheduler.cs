
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using Ticket.Base.Services;
using Ticket.API.Common;
using Ticket.Base.Entities;

namespace Ticket.API.TicketService
{
    public class UserCacheScheduler
    {
        private Timer _timer;

        private IUserService _userService;
        private UserCacheService<IUser> _userCacheService;
        private Action<string> _scheduleCallBack;

        public UserCacheScheduler(IServiceProvider serviceProvider)
        {
            this._userService = serviceProvider.GetService<IUserService>();
            this._userCacheService = serviceProvider.GetService<UserCacheService<IUser>>();
        }

        private bool _working;
        public async void OnTimeReached(object state)
        {
            if (this._working) return;

            this._working = true;


            this._scheduleCallBack?.Invoke("Starting cache..");
            try
            {
                var allUsers = (await this._userService.GetAllUsersList());
                foreach (var user in allUsers)
                {
                    this._userCacheService.Set(user);
                }


                this._scheduleCallBack?.Invoke("Cache completed....");
            }
            catch (AggregateException ex)
            {
                this._scheduleCallBack?.Invoke("Cache completed with error\r\n" + ex.Message);
            }
            finally
            {
                this._working = false;
            }

        }

        public void ScheduleCache(Action<string> callBack)
        {

            this._scheduleCallBack = callBack ?? this._scheduleCallBack;

            if (this._timer != null)
            {
                this._timer.Dispose();
            }
            ///x x Mill Second x Second x Minute
            this._timer = new Timer(this.OnTimeReached, callBack, 0, 1000 * 60 * 5);

        }
    }
}
