namespace HttpContrib.Http
{
	using System;
	using System.IO;
	using System.Net;
	using System.Threading;
	using System.Threading.Tasks;
	using HttpContrib.Client;

	public class SimpleHttpClient
	{
		private Uri _baseAddress;
		private Func<SimpleHttpClient, IHttpQueryProvider> _queryProviderFactory;

		public SimpleHttpClient(string baseAddress)
			: this(baseAddress, null)
		{ }

		public SimpleHttpClient(string baseAddress, Func<SimpleHttpClient, IHttpQueryProvider> queryProviderFactory)
		{
			_baseAddress = new Uri(baseAddress);

			if(queryProviderFactory == null)
				_queryProviderFactory = client => new HttpQueryProvider(client);

			this.Accept = MediaType.Json;
		}

		public string Accept
		{
			get;
			set;
		}

		public Uri BaseAddress
		{
			get
			{
				return _baseAddress;
			}
		}

		public HttpQuery<T> CreateQuery<T>()
		{
			return new HttpQuery<T>(_queryProviderFactory(this));
		}

		public HttpQuery<T> CreateQuery<T>(string resourceName)
		{
			return new HttpQuery<T>(_queryProviderFactory(this), resourceName);
		}

		public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
		{
			var tcs = new TaskCompletionSource<HttpResponseMessage>();

			ThreadPool.QueueUserWorkItem(w =>
			{
				if (request.Method == HttpMethod.Post || request.Method == HttpMethod.Put)
				{
					PostContent(request, tcs);
				}
				else
				{
					GetContent(request, tcs);
				}
			});

			return tcs.Task;
		}

		private void GetContent(HttpRequestMessage request, TaskCompletionSource<HttpResponseMessage> tcs)
		{
			HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(request.RequestUri);
			httpRequest.Method = request.Method;
			httpRequest.Accept = request.Accept;

			if(!String.IsNullOrEmpty(request.IfMatch))
				httpRequest.Headers[HttpRequestHeader.IfMatch] = request.IfMatch;
			if (!String.IsNullOrEmpty(request.IfNoneMatch))
				httpRequest.Headers[HttpRequestHeader.IfNoneMatch] = request.IfNoneMatch;

			HandleResponse(httpRequest, tcs);
		}

		private void PostContent(HttpRequestMessage request, TaskCompletionSource<HttpResponseMessage> tcs)
		{
			HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(request.RequestUri);
			httpRequest.Method = request.Method;
			httpRequest.Accept = request.Accept;
			httpRequest.ContentType = request.ContentType;

			var getStreamTask = httpRequest.GetRequestStreamAsync();
			getStreamTask.ContinueWith(t =>
			{
				CopyStream(request.Content, t.Result);

				t.Result.Flush();
				t.Result.Close();

				HandleResponse(httpRequest, tcs);
			});
		}

		private void HandleResponse(HttpWebRequest request, TaskCompletionSource<HttpResponseMessage> tcs)
		{
			var responseTask = request.GetResponseAsync();

			responseTask.ContinueWith(responseResult =>
			{
				if (responseResult.IsFaulted)
				{
					tcs.TrySetException(responseResult.Exception);
				}
				else
				{
					HttpWebResponse webResponse = (HttpWebResponse)responseResult.Result;
					HttpResponseMessage response = new HttpWebResponseMessage(webResponse);

					tcs.TrySetResult(response);
				}
			});
		}

		private static void CopyStream(Stream input, Stream output)
		{
			byte[] buffer = new byte[32768];
			while (true)
			{
				int read = input.Read(buffer, 0, buffer.Length);
				if (read <= 0)
					return;
				output.Write(buffer, 0, read);
			}
		}
	}
}