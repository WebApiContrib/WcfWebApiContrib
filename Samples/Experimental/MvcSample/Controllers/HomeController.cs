using System.Web.Mvc;

using Microsoft.ApplicationServer.Http.Dispatcher;

using MvcSample.Models.Home;

namespace MvcSample.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			IndexModel indexModel;
			try
			{
				indexModel = new IndexModel { ServiceName = DateTimeServiceClient.GetServiceName(), Now = DateTimeServiceClient.GetNow() };
			}
			catch (HttpResponseException httpResponseException)
			{
				indexModel = new IndexModel { ErrorMessage = httpResponseException.Response.ReasonPhrase };
			}
			return View(indexModel);
		}
	}
}