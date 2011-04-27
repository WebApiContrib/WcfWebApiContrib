using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Web;
using System.ServiceModel;

namespace System.Net.Http.Security.Tests
{
	[ServiceContract]
	public class HelloWorldResource
	{
		[WebGet(UriTemplate = "hello")]
		public string Get()
		{
			return "Hell World";
		}
	}
}
