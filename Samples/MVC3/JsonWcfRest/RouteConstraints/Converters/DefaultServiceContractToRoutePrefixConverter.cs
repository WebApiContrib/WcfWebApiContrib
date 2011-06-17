using System;

namespace JsonWcfRest.RouteConstraints.Converters
{
	internal class DefaultServiceContractToRoutePrefixConverter : ServiceContractToRoutePrefixConverter
	{
		private static readonly DefaultServiceContractToRoutePrefixConverter instance = new DefaultServiceContractToRoutePrefixConverter();

		private DefaultServiceContractToRoutePrefixConverter()
		{
		}

		public static DefaultServiceContractToRoutePrefixConverter Instance
		{
			get
			{
				return instance;
			}
		}

		public override string GetRoutePrefix(Type type)
		{
			return type.Name.Replace("Service", "");
		}
	}
}