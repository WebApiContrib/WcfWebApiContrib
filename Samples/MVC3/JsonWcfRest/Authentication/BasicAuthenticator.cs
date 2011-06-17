using System;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;

namespace JsonWcfRest.Authentication
{
	public class BasicAuthenticator : IAuthenticator
	{
		public const string Scheme = "Basic";

		public bool Authenticate(AuthenticationHeaderValue authorizationHeader, out IPrincipal principal)
		{
			principal = null;

			if (authorizationHeader == null || authorizationHeader.Scheme != Scheme)
			{
				return false;
			}

			NetworkCredential credential = ExtractCredential(authorizationHeader);

			if (!AuthenticateUser(credential))
			{
				return false;
			}

			principal = new GenericPrincipal(new GenericIdentity(credential.UserName), new string[] { });
			return true;
		}

		private static NetworkCredential ExtractCredential(AuthenticationHeaderValue authorizationHeader)
		{
			string parameter = authorizationHeader.Parameter.Trim();
			string userNameAndPassword = Encoding.ASCII.GetString(Convert.FromBase64String(parameter));
			string[] credentials = userNameAndPassword.Split(':');
			return new NetworkCredential(credentials[0], credentials[1], "DOMAIN");
		}

		private static bool AuthenticateUser(NetworkCredential credential)
		{
			//apply actual authentication logic here
			return credential.UserName == "dpeden" && credential.Password == "test";
		}
	}
}