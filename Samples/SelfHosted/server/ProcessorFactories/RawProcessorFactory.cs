//using System.Collections.Generic;
//using System.ServiceModel.Description;
//using System.ServiceModel.Dispatcher;
//using Microsoft.ServiceModel.Http;
//using SelfhostedServer.Processors;
//using SelfhostedServer.Services;

//namespace SelfhostedServer.ProcessorFactories {
//    public class RawProcessorFactory : HostConfiguration {
//        public override void RegisterRequestProcessorsForOperation(HttpOperationDescription operation, IList<Processor> processors, MediaTypeProcessorMode mode) {
//            processors.Add(new LoggingProcessor(new Logger(), true));
//        }

//        public override void RegisterResponseProcessorsForOperation(HttpOperationDescription operation, IList<Processor> processors, MediaTypeProcessorMode mode) {
//            processors.Clear();
//            processors.Add(new LoggingProcessor(new Logger(), false));
//            processors.Add(new RawHttpProcessor(operation));
            

//        }

//    }
//}