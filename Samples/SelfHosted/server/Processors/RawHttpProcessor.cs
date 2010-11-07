//using System.Collections.Generic;
//using System.ServiceModel.Description;
//using System.ServiceModel.Dispatcher;
//using Microsoft.Http;
//using Microsoft.ServiceModel.Dispatcher;

//namespace SelfhostedServer.Processors {
//    public class RawHttpProcessor : Processor {
//        private HttpParameterDescription _ReturnValue;

//        public RawHttpProcessor(HttpOperationDescription operation)  {
//            _ReturnValue = operation.ReturnValue;
//        }
//        protected override IEnumerable<ProcessorArgument> OnGetInArguments() {

//            var arguments = new List<ProcessorArgument>();
//            arguments.Add(new ProcessorArgument(_ReturnValue.Name, _ReturnValue.ParameterType));
//            arguments.Add(new ProcessorArgument(HttpPipelineFormatter.ArgumentHttpResponseMessage, typeof(HttpResponseMessage)));
//            return arguments.ToArray();
//        }

//        protected override IEnumerable<ProcessorArgument> OnGetOutArguments() {
        
//            return null;
//        }

//        protected override ProcessorResult OnExecute(object[] input) {
//            var result = new ProcessorResult();
//            var returnResponse = (HttpResponseMessage)input[0];
//            var pipeLineResponse = (HttpResponseMessage) input[1];

//            pipeLineResponse.Headers = returnResponse.Headers;
//            pipeLineResponse.Content = returnResponse.Content;
//            pipeLineResponse.StatusCode = returnResponse.StatusCode;

//            pipeLineResponse.Request = returnResponse.Request;
            
//            return result;

//        }
//    }
//}
