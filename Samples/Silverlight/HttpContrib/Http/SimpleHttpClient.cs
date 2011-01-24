namespace HttpContrib.Http
{
	using System;
	using System.IO;
	using System.Net;
	using System.Text;
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
				if (request.Method == HttpMethod.Post)
				{
					PostContent(request, tcs);
				}
				else
				{
					HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(request.RequestUri);
					httpRequest.Method = request.Method;
					httpRequest.Accept = request.Accept;

					HandleResponse(httpRequest, tcs);
				}
			});

			return tcs.Task;
		}

		private void PostContent(HttpRequestMessage request, TaskCompletionSource<HttpResponseMessage> tcs)
		{
			using (StreamReader reader = new StreamReader(request.Content))
			{
				string content = reader.ReadToEnd();

				WebClient client = new WebClient();
				client.Headers["Accept"] = request.Accept;
				client.Headers["Content-Type"] = request.ContentType;
				client.Encoding = Encoding.UTF8;
				client.UploadStringCompleted += (s, e) =>
				{
					if (e.Error != null)
					{
						tcs.TrySetException(e.Error);
					}
					else
					{
						var buffer = Encoding.UTF8.GetBytes(e.Result);
						MemoryStream responseStream = new MemoryStream(buffer);
						HttpResponseMessage response = new HttpResponseMessage(request.ContentType, responseStream);

						tcs.TrySetResult(response);
					}
				};
				client.UploadStringAsync(request.RequestUri, content);
			}
		}

		private void HandleResponse(HttpWebRequest request, TaskCompletionSource<HttpResponseMessage> tcs)
		{
			var responseTask = request.GetResponseAsync();

			responseTask.ContinueWith(responseResult =>
			{
				HttpWebResponse webResponse = (HttpWebResponse)responseResult.Result;
				HttpResponseMessage response = new HttpWebResponseMessage(webResponse);

				tcs.TrySetResult(response);
			});
		}
	}
}