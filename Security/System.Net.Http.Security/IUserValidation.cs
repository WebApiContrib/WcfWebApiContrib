using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Net.Http.Security
{
	public interface IUserValidation
	{
		bool Validate(string username, string password);
	}
}
