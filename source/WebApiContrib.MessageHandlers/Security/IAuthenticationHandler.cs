using System.Net.Http;
using System.Security.Principal;

namespace WebApiContrib.MessageHandlers.Security
{
	public interface IAuthenticationHandler
	{
		string Scheme { get; }
		bool Authenticate(HttpRequestMessage request, HttpResponseMessage response, out IPrincipal principal);
	}
}
