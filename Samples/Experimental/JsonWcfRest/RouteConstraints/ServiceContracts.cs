using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Web.Routing;

using JsonWcfRest.RouteConstraints.Converters;

namespace JsonWcfRest.RouteConstraints
{
	public class ServiceContracts
	{
		private readonly List<Type> types;

		private ServiceContracts(IEnumerable<Type> types)
		{
			this.types = new List<Type>(types);
		}

		public static ServiceContracts In(Assembly assembly)
		{
			IEnumerable<Type> types = assembly.GetTypes()
				.Where(type => type.GetCustomAttributes(typeof(ServiceContractAttribute), false).Length > 0);
			return new ServiceContracts(types);
		}

		public IRouteConstraint WithDefaultConverter()
		{
			return WithConverter(DefaultServiceContractToRoutePrefixConverter.Instance);
		}

		public IRouteConstraint WithConverter(IServiceContractToRoutePrefixConverter converter)
		{
			return new ServiceRouteConstraint(types.Select(converter.GetRoutePrefix));
		}
	}
}