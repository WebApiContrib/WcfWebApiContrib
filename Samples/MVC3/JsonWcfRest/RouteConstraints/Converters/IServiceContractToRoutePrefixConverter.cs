using System;

namespace JsonWcfRest.RouteConstraints.Converters
{
	public interface IServiceContractToRoutePrefixConverter
	{
		string GetRoutePrefix(Type type);
		string GetRoutePrefix<T>();
	}
}