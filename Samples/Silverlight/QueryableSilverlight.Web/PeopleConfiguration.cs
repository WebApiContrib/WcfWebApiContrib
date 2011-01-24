namespace QueryableSilverlight.Web
{
	using System.Collections.Generic;
	using System.ServiceModel.Description;
	using System.ServiceModel.Dispatcher;
	using Microsoft.ServiceModel.Description;
	using Microsoft.ServiceModel.Http;

	public class PeopleConfiguration : HttpHostConfiguration, IProcessorProvider
	{
		public PeopleConfiguration()
		{
			this.SetProcessorProvider(this);
		}

		public void RegisterRequestProcessorsForOperation(HttpOperationDescription operation, IList<Processor> processors, MediaTypeProcessorMode mode)
		{
		}

		public void RegisterResponseProcessorsForOperation(HttpOperationDescription operation, IList<Processor> processors, MediaTypeProcessorMode mode)
		{
			processors.Add(new JsonProcessor(operation, mode));
		}
	}
}