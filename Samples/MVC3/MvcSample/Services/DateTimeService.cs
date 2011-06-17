using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;

using JsonWcfRest;

namespace MvcSample.Services
{
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
	[ServiceContract]
	public class DateTimeService
	{
		[WebGet(UriTemplate = "ServiceName")]
		public JsonResponse<string> GetServiceName()
		{
			return new JsonResponse<string>(GetType().Name);
		}

		[WebGet(UriTemplate = "Now")]
		public JsonResponse<DateTime> GetNow()
		{
			return new JsonResponse<DateTime>(DateTime.UtcNow);
		}
	}
}