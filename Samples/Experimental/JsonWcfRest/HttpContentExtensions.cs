using System.Net.Http;

using JsonWcfRest.Formatters;

using Microsoft.ApplicationServer.Http;

namespace JsonWcfRest
{
	public static class HttpContentExtensions
	{
		private static readonly JsonNetMediaTypeFormatter jsonMediaTypeFormatter = new JsonNetMediaTypeFormatter();

		public static T ReadAsJson<T>(this HttpContent content)
		{
			return content.ReadAs<T>(new[] { jsonMediaTypeFormatter });
		}
	}
}