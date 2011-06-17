using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using JsonWcfRest;
using JsonWcfRest.RouteConstraints;

using Microsoft.ApplicationServer.Http.Description;

using MvcSample.Services;

namespace MvcSample
{
	public class MvcApplication : HttpApplication
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				"Default", // Route name
				"{controller}/{action}/{id}", // URL with parameters
				new { controller = "Home", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
				new { controller = ServiceContracts.In(Assembly.GetExecutingAssembly()).WithDefaultConverter() }
				);

			var configurationBuilder = HttpHostConfiguration.Create()
				.SetMessageHandlerFactory(new CustomHttpMessageChannelFactory())
				.SetOperationHandlerFactory(new CustomHttpOperationHandlerFactory());

			routes.MapServiceRoute<DateTimeService>(configurationBuilder);
		}

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			RegisterGlobalFilters(GlobalFilters.Filters);
			RegisterRoutes(RouteTable.Routes);
		}
	}
}