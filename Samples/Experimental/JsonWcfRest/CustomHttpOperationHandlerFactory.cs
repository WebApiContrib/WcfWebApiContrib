using JsonWcfRest.Formatters;

using Microsoft.ApplicationServer.Http.Description;

namespace JsonWcfRest
{
	public class CustomHttpOperationHandlerFactory : HttpOperationHandlerFactory
	{
		public CustomHttpOperationHandlerFactory()
		{
			Formatters.Insert(0, new JsonNetMediaTypeFormatter());
		}
	}
}