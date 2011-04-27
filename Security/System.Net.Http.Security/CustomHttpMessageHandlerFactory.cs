using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Microsoft.ApplicationServer.Http.Channels;

namespace System.Net.Http.Security
{
	/// <summary>
	/// The HttpMessageHandlerFactory shipped with the framework does not support creating handlers with custom
	/// arguments in the constructor.
	/// </summary>
	public class CustomHttpMessageHandlerFactory : HttpMessageHandlerFactory
	{
		Func<HttpMessageChannel, DelegatingChannel>[] factories;

		public CustomHttpMessageHandlerFactory()
			: base()
		{
		}

		public CustomHttpMessageHandlerFactory(params Func<HttpMessageChannel, DelegatingChannel>[] factories)
		{
			this.factories = factories;
		}

		protected override HttpMessageChannel OnCreate(HttpMessageChannel innerChannel)
		{
			if (innerChannel == null)
			{
				throw new ArgumentNullException("innerChannel");
			}

			HttpMessageChannel pipeline = innerChannel;
			
			foreach (var factory in this.factories)
			{
				pipeline = factory(innerChannel);
			}

			return pipeline;
		}
	}
}
