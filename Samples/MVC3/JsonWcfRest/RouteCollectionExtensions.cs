using System.Web.Routing;

using JsonWcfRest.RouteConstraints.Converters;

using Microsoft.ApplicationServer.Http.Activation;
using Microsoft.ApplicationServer.Http.Description;

namespace JsonWcfRest
{
	public static class RouteCollectionExtensions
	{
		public static void MapServiceRoute<T>(this RouteCollection routeCollection, IServiceContractToRoutePrefixConverter converter)
		{
			routeCollection.MapServiceRoute<T>(converter.GetRoutePrefix(typeof(T)));
		}

		public static void MapServiceRoute<T>(this RouteCollection routeCollection, IServiceContractToRoutePrefixConverter converter, IHttpHostConfigurationBuilder configuration)
		{
			routeCollection.MapServiceRoute<T>(converter.GetRoutePrefix(typeof(T)), configuration);
		}

		public static void MapServiceRoute<T>(this RouteCollection routeCollection)
		{
			routeCollection.MapServiceRoute<T>(DefaultServiceContractToRoutePrefixConverter.Instance.GetRoutePrefix(typeof(T)));
		}

		public static void MapServiceRoute<T>(this RouteCollection routeCollection, IHttpHostConfigurationBuilder configuration)
		{
			routeCollection.MapServiceRoute<T>(DefaultServiceContractToRoutePrefixConverter.Instance.GetRoutePrefix(typeof(T)), configuration);
		}
	}
}