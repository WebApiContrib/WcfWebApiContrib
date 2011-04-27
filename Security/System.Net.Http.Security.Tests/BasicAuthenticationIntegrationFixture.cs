using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Microsoft.ApplicationServer.Http.Activation;
using Microsoft.ApplicationServer.Http.Description;
using Microsoft.ApplicationServer.Http.Channels;

namespace System.Net.Http.Security.Tests
{
	[TestFixture]
	public class BasicAuthenticationIntegrationFixture
	{
		HttpConfigurableServiceHost<HelloWorldResource> Host;

		[SetUp]
		public void FixtureSetUp()
		{
			var config = HttpHostConfiguration.Create().
			    SetMessageHandlerFactory(new CustomHttpMessageHandlerFactory(innerChannel => 
					new BasicAuthenticationHandler(innerChannel, new UserValidation(), "test")));
			
			Host = new HttpConfigurableServiceHost<HelloWorldResource>((HttpHostConfiguration)config, 
				new Uri[] { new Uri("http://localhost:8090") });
			
			Host.Open();
		}

		[TearDown]
		public void FixtureTearDown()
		{
			Host.Close();
		}

		[Test]
		[Ignore("It does not work as self host for a bug in WCF")]
		public void ShouldReturnUnauthorizedWhenCredentialsNotProvided()
		{
			var client = new HttpClient("http://localhost:8090");
			
			var response = client.Get("hello");

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		class UserValidation : IUserValidation
		{
			public bool Validate(string username, string password)
			{
				return username == "foo" && password == "bar";
			}
		}
	}
}
