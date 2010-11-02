using System.Collections.Generic;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Microsoft.ServiceModel.Http;
using SelfhostedServer.Processors;

namespace SelfhostedServer.ProcessorFactories {
    public class RawProcessorFactory : HostConfiguration {
        public override void RegisterRequestProcessorsForOperation(HttpOperationDescription operation, IList<Processor> processors, MediaTypeProcessorMode mode) {
        }

        public override void RegisterResponseProcessorsForOperation(HttpOperationDescription operation, IList<Processor> processors, MediaTypeProcessorMode mode) {
            processors.Clear();
            processors.Add(new RawHttpProcessor(operation));
        }

    }
}