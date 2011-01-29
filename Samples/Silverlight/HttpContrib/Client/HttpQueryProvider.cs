namespace HttpContrib.Client
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using HttpContrib.Http;

	public interface IHttpQueryProvider
	{
		Task<T> ExecuteSingleAsync<T>(HttpQuery<T> query);
		Task<IEnumerable<T>> ExecuteAsync<T>(HttpQuery<T> query);
	}

	public class HttpQueryProvider : IHttpQueryProvider
	{
		private SimpleHttpClient _client;

		public HttpQueryProvider(SimpleHttpClient client)
		{
			_client = client;
		}

		public Task<IEnumerable<T>> ExecuteAsync<T>(HttpQuery<T> query)
		{
			return ExecuteAsyncInternal<T, IEnumerable<T>>(query);
		}

		public Task<T> ExecuteSingleAsync<T>(HttpQuery<T> query)
		{
			return ExecuteAsyncInternal<T, T>(query);
		}

		private Task<TResult> ExecuteAsyncInternal<T,TResult>(HttpQuery<T> query)
		{
			var tcs = new TaskCompletionSource<TResult>();

			HttpRequestMessage request = new HttpRequestMessage();
			request.Method = query.Method;

			request.RequestUri = query.GetFullyQualifiedQuery(this._client);

			if ((query.Method == HttpMethod.Post || query.Method == HttpMethod.Put)
				&& query.Content != null)
			{
				request.Accept = MediaType.Xml;
				request.ContentType = MediaType.Xml;
				request.Content = query.Content.WriteObjectAsXml();
			}

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
						TResult result;

						if ((query.Method == HttpMethod.Post || query.Method == HttpMethod.Put))
							result = response.Result.ReadXmlAsObject<TResult>();
						else
							result = response.Result.ReadAsObject<TResult>();

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