namespace HttpContrib.Http
{
	using System;
	using System.IO;

	public class HttpRequestMessage
	{
		public HttpRequestMessage()
			: this(HttpMethod.Get)
		{
		}

		public HttpRequestMessage(string method)
		{
			this.Method = method;
			this.Accept = MediaType.Json;
			this.ContentType = MediaType.Json;
		}

		public string ContentType { get; set; }
		public string Method { get; set; }
		public Uri RequestUri { get; set; }
		public string Accept { get; set; }

		public Stream Content { get; set; }
	}
}