using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace JsonWcfRest.RouteConstraints
{
	internal class ServiceRouteConstraint : IRouteConstraint
	{
		private readonly List<string> routePrefixes = new List<string>();

		public ServiceRouteConstraint(IEnumerable<string> routePrefixes)
		{
			this.routePrefixes = new List<string>(routePrefixes);
		}

		public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
		{
			return !routePrefixes.Contains(values["controller"]);
		}
	}
}