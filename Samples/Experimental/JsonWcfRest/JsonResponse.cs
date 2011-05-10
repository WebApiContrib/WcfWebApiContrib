using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;

using Microsoft.ApplicationServer.Http;

namespace JsonWcfRest
{
	public class JsonResponse<T> : HttpResponseMessage<T>
	{
		private void AddJsonHeader()
		{
			Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
		}

		public JsonResponse(HttpStatusCode statusCode)
			: base(statusCode)
		{
			AddJsonHeader();
		}

		public JsonResponse(T value)
			: base(value)
		{
			AddJsonHeader();
		}

		public JsonResponse(T value, HttpStatusCode statusCode)
			: base(value, statusCode)
		{
			AddJsonHeader();
		}

		public JsonResponse(T value, IEnumerable<MediaTypeFormatter> formatters)
			: base(value, formatters)
		{
			AddJsonHeader();
		}

		public JsonResponse(T value, HttpStatusCode statusCode, IEnumerable<MediaTypeFormatter> formatters)
			: base(value, statusCode, formatters)
		{
			AddJsonHeader();
		}
	}
}