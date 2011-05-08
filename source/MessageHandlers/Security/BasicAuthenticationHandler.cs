using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Security.Principal;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Threading;

namespace MessageHandlers.Security
{
	public class BasicAuthenticationHandler : DelegatingChannel, IAuthenticationHandler
	{
		IUserValidation userValidation;
		string realm;

		public BasicAuthenticationHandler(HttpMessageChannel innerChannel)
			: base(innerChannel)
		{
		}

		public BasicAuthenticationHandler(HttpMessageChannel innerChannel, IUserValidation userValidation, string realm)
			: base(innerChannel)
		{
			if (userValidation == null)
				throw new ArgumentNullException("userValidation");

			if (string.IsNullOrEmpty(realm))
				throw new ArgumentNullException("realm");

			this.userValidation = userValidation;
			this.realm = realm;
		}

		public string Scheme
		{
			get { return "Basic"; }
		}

		public bool Authenticate(HttpRequestMessage request, HttpResponseMessage response, out IPrincipal principal)
		{
			string[] credentials = ExtractCredentials(request);
			if (credentials.Length == 0 || !AuthenticateUser(credentials[0], credentials[1]))
			{
				Challenge(response);

				principal = null;

				return false;
			}
			else
			{
				//var nameClaim = new Claim(ClaimTypes.Name, credentials[0]);
				
				//principal = new ClaimsPrincipal(new ClaimsIdentity[] { new ClaimsIdentity(new Claim[] { nameClaim } )});

				principal = new GenericPrincipal(new GenericIdentity(credentials[0]), new string[] { });
				
				return true;
			}
		}

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			return Task<HttpResponseMessage>.Factory.StartNew(
				() =>
				{
					var response = new HttpResponseMessage(HttpStatusCode.Unauthorized, "unauthorized");
					response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue("Basic", "realm=" + "foo"));

					return response;
				}
			);
		}

		private void Challenge(HttpResponseMessage response)
		{
			response.StatusCode = HttpStatusCode.Unauthorized;
			response.Content = new StringContent("Access denied");
			response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue(Scheme, "realm=" + this.realm));
		}

		private string[] ExtractCredentials(HttpRequestMessage request)
		{
			if (request.Headers.Authorization != null && request.Headers.Authorization.Scheme.StartsWith(Scheme))
			{
				string encodedUserPass = request.Headers.Authorization.Parameter.Trim();

				try
				{
					Encoding encoding = Encoding.GetEncoding("iso-8859-1");
					string userPass = encoding.GetString(Convert.FromBase64String(encodedUserPass));
					int separator = userPass.IndexOf(':');

					string[] credentials = new string[2];
					credentials[0] = userPass.Substring(0, separator);
					credentials[1] = userPass.Substring(separator + 1);

					return credentials;
				}
				catch (FormatException)
				{
					return new string[] { };
				}
			}

			return new string[] { };
		}

		private bool AuthenticateUser(string username, string password)
		{
			if (this.userValidation.Validate(username, password))
			{
				return true;
			}

			return false;
		}
	}
}
