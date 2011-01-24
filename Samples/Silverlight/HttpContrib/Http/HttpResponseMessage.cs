namespace HttpContrib.Http
{
	using System.IO;
	using System.Net;

	public class HttpResponseMessage
	{
		private readonly string _contentType;
		private readonly Stream _responseStream;

		public HttpResponseMessage()
		{
		}

		public HttpResponseMessage(string contentType, Stream responseStream)
		{
			_contentType = contentType;
			_responseStream = responseStream;
		}

		public virtual string ContentType
		{
			get
			{
				return _contentType;
			}
		}

		public virtual Stream GetResponseStream()
		{
			return _responseStream;
		}
	}

	public class HttpWebResponseMessage : HttpResponseMessage
	{
		private HttpWebResponse _webResponse;

		public HttpWebResponseMessage(HttpWebResponse webResponse)
		{
			_webResponse = webResponse;
		}

		public override string ContentType
		{
			get
			{
				return _webResponse.ContentType;
			}
		}

		public override Stream GetResponseStream()
		{
			return _webResponse.GetResponseStream();
		}
	}
}