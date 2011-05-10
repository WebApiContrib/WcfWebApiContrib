using System.Net.Http.Headers;
using System.Security.Principal;

namespace JsonWcfRest.Authentication
{
	public interface IAuthenticator
	{
		bool Authenticate(AuthenticationHeaderValue authorizationHeader, out IPrincipal principal);
	}
}