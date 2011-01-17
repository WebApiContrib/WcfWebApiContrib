using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using Microsoft.ServiceModel.Http;
using Microsoft.ServiceModel.Description;
using SelfhostedServer.Processors;
using SelfhostedServer.Services;

namespace SelfhostedServer.ProcessorFactories {
    public class DefaultProcessorFactory : HttpHostConfiguration, IProcessorProvider {
        public void RegisterRequestProcessorsForOperation(HttpOperationDescription operation, IList<Processor> processors, MediaTypeProcessorMode mode) {
            // Do nothing
            processors.Add(new LoggingProcessor(new Logger(), true));
        }

        public void RegisterResponseProcessorsForOperation(HttpOperationDescription operation, IList<Processor> processors, MediaTypeProcessorMode mode) {
            processors.Add(new PlainTextProcessor(operation, MediaTypeProcessorMode.Response));
            processors.Add(new LoggingProcessor(new Logger(), false));
            
        }
    }
}
