using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

using JsonWcfRest.Authentication;

namespace JsonWcfRest.Channels
{
	public class AuthenticationChannel : DelegatingChannel
	{
		private readonly IAuthenticator authenticator;

		public AuthenticationChannel(HttpMessageChannel innerChannel)
			: this(innerChannel, new BasicAuthenticator())
		{
		}

		public AuthenticationChannel(HttpMessageChannel innerChannel, IAuthenticator authenticator)
			: base(innerChannel)
		{
			this.authenticator = authenticator;
		}

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken)
		{
			IPrincipal principal;
			if (authenticator.Authenticate(httpRequestMessage.Headers.Authorization, out principal))
			{
				Thread.CurrentPrincipal = principal;
				return base.SendAsync(httpRequestMessage, cancellationToken);
			}

			return Task.Factory.StartNew(() => (HttpResponseMessage)new JsonResponse<string>("Can't touch this.", HttpStatusCode.Unauthorized));
		}
	}
}