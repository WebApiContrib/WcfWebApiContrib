using System;

namespace JsonWcfRest.RouteConstraints.Converters
{
	public abstract class ServiceContractToRoutePrefixConverter : IServiceContractToRoutePrefixConverter
	{
		public abstract string GetRoutePrefix(Type type);

		public string GetRoutePrefix<T>()
		{
			return GetRoutePrefix(typeof(T));
		}
	}
}