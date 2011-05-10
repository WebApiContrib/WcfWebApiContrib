using System;

namespace MvcSample.Models.Home
{
	public class IndexModel
	{
		public string ServiceName
		{
			get;
			set;
		}

		public DateTime Now
		{
			get;
			set;
		}

		public string ErrorMessage
		{
			get;
			set;
		}
	}
}