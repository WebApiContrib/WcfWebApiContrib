using System.Net.Http;

using JsonWcfRest.Channels;

using Microsoft.ApplicationServer.Http.Channels;

namespace JsonWcfRest
{
	public class CustomHttpMessageChannelFactory : HttpMessageHandlerFactory // shouldn't HttpMessageHandlerFactory actually be named HttpMessageChannelFactory?
	{
		protected override HttpMessageChannel OnCreate(HttpMessageChannel innerChannel)
		{
			return base.OnCreate(new AuthenticationChannel(innerChannel));
		}
	}
}