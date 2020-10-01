using Google;
using Polly;
using Polly.Retry;
using System;
using System.Net;

namespace LtiLibrary.NetCore.GoogleStorageRetryPolicy
{
	public static class GoogleStorageRetryPolicy
	{
		static object Just4lock = new object();
		static AsyncRetryPolicy _RetryPolicy;
		public static AsyncRetryPolicy RetryPolicy
		{
			get
			{
				if (_RetryPolicy == null)
					lock (Just4lock)
						if (_RetryPolicy == null)
							_RetryPolicy = Policy
								.Handle<Exception>(ex =>
								{
									if (ex is GoogleApiException && (ex as GoogleApiException).HttpStatusCode == HttpStatusCode.NotFound)
									{
										return false;
									}
									return true;
								})
								.WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

				return _RetryPolicy;
			}
		}
	}
}
