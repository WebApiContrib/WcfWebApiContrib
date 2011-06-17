using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

using JsonWcfRest;

using Microsoft.ApplicationServer.Http.Dispatcher;

namespace MvcSample
{
	public class DateTimeServiceClient
	{
		public static string GetServiceName()
		{
			return CallService<string>("ServiceName");
		}

		public static DateTime GetNow()
		{
			return CallService<DateTime>("Now");
		}

		private static T CallService<T>(string serviceName)
		{
			using (var httpClient = new HttpClient("http://localhost:9001"))
			{
				httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes("dpeden:test")));
				using (HttpResponseMessage httpResponseMessage = httpClient.Get("DateTime/" + serviceName))
				{
					if (httpResponseMessage.IsSuccessStatusCode)
					{
						return httpResponseMessage.Content.ReadAsJson<T>();
					}

					throw new HttpResponseException(httpResponseMessage);
				}
			}
		}
	}
}