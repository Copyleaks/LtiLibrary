using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Copyleaks.Utils.Http
{
    public static class HttpClientRetryPolicy
    {
        static object Just4lock = new object();
        static AsyncRetryPolicy<HttpResponseMessage> _RetryPolicy;
        public static AsyncRetryPolicy<HttpResponseMessage> RetryPolicy
        {
            get
            {
                if (_RetryPolicy == null)
                    lock (Just4lock)
                        if (_RetryPolicy == null)
                            _RetryPolicy = Policy
                                .Handle<HttpRequestException>()
                                .Or<TaskCanceledException>()
                                .OrResult<HttpResponseMessage>(response => (int)response.StatusCode >= 500)
                                  .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(4));

                return _RetryPolicy;
            }
        }
    }
}
