namespace QueryableSilverlight.Web
{
	using System;
	using System.Web.Routing;
	using Microsoft.ApplicationServer.Http;
	using Microsoft.ApplicationServer.Http.Activation;

	public class Global : System.Web.HttpApplication
	{
		protected void Application_Start(object sender, EventArgs e)
		{
			RouteTable.Routes.MapServiceRoute<PeopleResource>( "people" );
		}
	}
}