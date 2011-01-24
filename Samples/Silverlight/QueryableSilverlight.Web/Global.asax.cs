namespace QueryableSilverlight.Web
{
	using System;
	using System.Web.Routing;
	using Microsoft.ServiceModel.Http;

	public class Global : System.Web.HttpApplication
	{
		protected void Application_Start(object sender, EventArgs e)
		{
			RouteTable.Routes.AddServiceRoute<PeopleResource>("people", new PeopleConfiguration());
		}
	}
}