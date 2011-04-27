using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;

namespace System.Net.Http.Security
{
	public interface IAuthenticationHandler
	{
		string Scheme { get; }
		bool Authenticate(HttpRequestMessage request, HttpResponseMessage response, out IPrincipal principal);
	}
}
