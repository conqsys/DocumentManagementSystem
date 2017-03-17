using Ticket.DataAccess.Common;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.API.Common
{
    public class AsyncRestClient : RestClient
    {
        public AsyncRestClient(string baseUrl) : base(baseUrl)
        {
            this.ReadWriteTimeout = 10000;
            this.Timeout = 10000;

        }
        public async Task<string> ExecuteAsync(IRestRequest request)
        {
            var result = await Task.Run<string>(() =>
            {

                try
                {
                    var t = new TaskCompletionSource<string>();

                    base.ExecuteAsync<string>(request, (response, handle) =>
                    {
                        t.TrySetResult(response.Content);

                    });

                    return t.Task;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });
            return result;
        }
        public async Task<HttpResult<T>> ExecuteAsync<T>(IRestRequest request)
        {
            var result = await Task.Run<HttpResult<T>>(() =>
            {

                try
                {
                    var t = new TaskCompletionSource<HttpResult<T>>();

                    base.ExecuteAsync(request, (response, handle) =>
                    {

                        if (response.ErrorException != null)
                        {
                            t.TrySetException(response.ErrorException);
                            return;
                        }


                        HttpResult<T> httpResult = new HttpResult<T>();
                        httpResult.ActualContent = response.Content;

                        T data = default(T);

                        try
                        {
                            data = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(response.Content);
                        }
                        catch(Exception ex)
                        {
                            httpResult.HasParseError = true;
                        }


                        if (typeof(T) == typeof(JObject))
                        {
                            httpResult.Data = data as dynamic;
                        }
                        else
                        {
                            httpResult.Data = data;
                        }
                        httpResult.Response = response;
                        t.TrySetResult(httpResult);


                    });

                    return t.Task;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });
            return result;
        }

    }
}
