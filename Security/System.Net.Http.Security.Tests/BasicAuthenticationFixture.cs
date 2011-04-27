using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Security.Principal;

namespace System.Net.Http.Security.Tests
{
	[TestFixture]
	public class BasicAuthenticationFixture
	{
		const string BasicAuthenticationScheme = "Basic";

		[Test]
		[ExpectedException(ExpectedException = typeof(ArgumentNullException))]
		public void ShouldThrowWithNullValidationFunction()
		{
			var handler = new BasicAuthenticationHandler(new FakeChannel(), null, "realm");
		}

		[Test]
		[ExpectedException(ExpectedException = typeof(ArgumentNullException))]
		public void ShouldThrowWithNullRealm()
		{
			var handler = new BasicAuthenticationHandler(new FakeChannel(), new UserValidation((u, p) => true), null);
		}

		[Test]
		public void ShouldReturnScheme()
		{
			var handler = new BasicAuthenticationHandler(new FakeChannel(), new UserValidation((u, p) => true), "realm");
			Assert.AreEqual(BasicAuthenticationScheme, handler.Scheme);
		}

		[Test]
		public void ShouldNotAuthenticateWhenMissingHeaders()
		{
			IPrincipal principal = null;

			var handler = new BasicAuthenticationHandler(new FakeChannel(), new UserValidation((u, p) => true), "realm");
			var authenticated = handler.Authenticate(new HttpRequestMessage(), new HttpResponseMessage(), out principal);

			Assert.IsFalse(authenticated, "The user must not be authenticated");
		}

		[Test]
		public void ShouldNotSetPrincipalWhenMissingHeaders()
		{
			IPrincipal principal = null;

			var handler = new BasicAuthenticationHandler(new FakeChannel(), new UserValidation((u, p) => true), "realm");
			var authenticated = handler.Authenticate(new HttpRequestMessage(), new HttpResponseMessage(), out principal);

			Assert.IsNull(principal, "The principal should not be set");
		}

		[Test]
		public void ShouldChallengeWhenMissingHeaders()
		{
			IPrincipal principal = null;

			var response = new HttpResponseMessage();

			var handler = new BasicAuthenticationHandler(new FakeChannel(), new UserValidation((u, p) => true), "realm");
			var authenticated = handler.Authenticate(new HttpRequestMessage(), response, out principal);

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
			Assert.IsTrue(response.Headers.WwwAuthenticate.Any(h => h.Scheme == BasicAuthenticationScheme && h.Parameter.Contains("realm")),
				"A WWW must be present in the response");
		}

		[Test]
		public void ShouldNotAuthenticateWithInvalidScheme()
		{
			IPrincipal principal = null;

			var request = new HttpRequestMessage();
			var response = new HttpResponseMessage();

			var credentials = EncodeCredentials("foo", "pass");

			request.Headers.Authorization = new Headers.AuthenticationHeaderValue("foo", credentials);

			var handler = new BasicAuthenticationHandler(new FakeChannel(), new UserValidation((u, p) => true), "realm");
			var authenticated = handler.Authenticate(request, response, out principal);

			Assert.IsFalse(authenticated, "The user should have not been authenticated");
		}

		[Test]
		public void ShouldNotAuthenticateWithInvalidEncodedUsernameAndPassword()
		{
			IPrincipal principal = null;

			var request = new HttpRequestMessage();
			var response = new HttpResponseMessage();

			var credentials = "some credentials";

			request.Headers.Authorization = new Headers.AuthenticationHeaderValue(BasicAuthenticationScheme, credentials);

			var handler = new BasicAuthenticationHandler(new FakeChannel(), new UserValidation((u, p) => true), "realm");
			var authenticated = handler.Authenticate(request, response, out principal);

			Assert.IsFalse(authenticated, "The user should have not been authenticated");
		}

		[Test]
		public void ShouldCallAuthenticateWithProvidedUserAndPassword()
		{
			IPrincipal principal = null;

			var request = new HttpRequestMessage();
			var response = new HttpResponseMessage();

			var credentials = EncodeCredentials("foo", "pass");

			request.Headers.Authorization = new Headers.AuthenticationHeaderValue(BasicAuthenticationScheme, credentials);

			var handler = new BasicAuthenticationHandler(new FakeChannel(),
				new UserValidation((u, p) => u == "foo" && p == "pass"),
				"realm");
			var authenticated = handler.Authenticate(request, response, out principal);

			Assert.IsTrue(authenticated, "The user should have been authenticated");
		}

		[Test]
		public void ShouldSetPrincipalForAuthenticatedUsers()
		{
			IPrincipal principal = null;

			var request = new HttpRequestMessage();
			var response = new HttpResponseMessage();

			var credentials = EncodeCredentials("foo", "pass");

			request.Headers.Authorization = new Headers.AuthenticationHeaderValue(BasicAuthenticationScheme, credentials);

			var handler = new BasicAuthenticationHandler(new FakeChannel(), new UserValidation((u, p) => u == "foo" && p == "pass"), "realm");
			var authenticated = handler.Authenticate(request, response, out principal);

			Assert.AreEqual("foo", principal.Identity.Name, "The principal should have been set with the user name");
		}

		private string EncodeCredentials(string username, string password)
		{
			var encoding = Encoding.GetEncoding("iso-8859-1");

			var credentials = string.Format("{0}:{1}", username, password);

			var encodedCredentials = Convert.ToBase64String(encoding.GetBytes(credentials));

			return encodedCredentials;
		}

		class UserValidation : IUserValidation
		{
			Func<string, string, bool> validation;

			public UserValidation(Func<string, string, bool> validation)
			{
				this.validation = validation;
			}

			public bool Validate(string username, string password)
			{
				return this.validation(username, password);
			}
		}

		class FakeChannel : HttpMessageChannel
		{
			protected override HttpResponseMessage Send(HttpRequestMessage request, Threading.CancellationToken cancellationToken)
			{
				throw new NotImplementedException();
			}

			protected override Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, Threading.CancellationToken cancellationToken)
			{
				throw new NotImplementedException();
			}
		}
	}
}
