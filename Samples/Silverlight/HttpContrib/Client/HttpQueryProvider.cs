namespace HttpContrib.Client
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using HttpContrib.Http;

	public interface IHttpQueryProvider
	{
		Task<IEnumerable<T>> ExecuteAsync<T>(string query);
	}

	public class HttpQueryProvider : IHttpQueryProvider
	{
		private SimpleHttpClient _client;

		public HttpQueryProvider(SimpleHttpClient client)
		{
			_client = client;
		}

		public Task<IEnumerable<T>> ExecuteAsync<T>(string requestUri)
		{
			var tcs = new TaskCompletionSource<IEnumerable<T>>();

			HttpRequestMessage request = new HttpRequestMessage();

			Uri fullUri = new Uri(_client.BaseAddress, requestUri);

			request.RequestUri = fullUri;
			request.Accept = _client.Accept;

			var requestTask = _client.SendAsync(request);

			requestTask.ContinueWith(response =>
			{
				try
				{
					if (response.IsFaulted)
					{
						tcs.TrySetException(response.Exception);
					}
					else
					{
						var result = response.Result.ReadAsObjectList<T>();

						tcs.TrySetResult(result);
					}
				}
				catch (Exception exc)
				{
					tcs.TrySetException(exc);
				}
			});

			return tcs.Task;
		}
	}
}