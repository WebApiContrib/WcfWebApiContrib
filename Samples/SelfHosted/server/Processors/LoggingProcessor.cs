using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Dispatcher;
using System.Text;
using Microsoft.ServiceModel.Dispatcher;

namespace SelfhostedServer.Processors {
    public class LoggingProcessor : Processor {
        private readonly ILogger _Logger;
        private readonly bool _InRequestPipeline;
        

        public LoggingProcessor(ILogger logger, bool inRequestPipeline) {
            _Logger = logger;
            _InRequestPipeline = inRequestPipeline;
        }

        protected override IEnumerable<ProcessorArgument> OnGetInArguments() {
            var arguments = new List<ProcessorArgument>();

            arguments.Add(new ProcessorArgument(HttpPipelineFormatter.ArgumentHttpRequestMessage, typeof(HttpRequestMessage)));
            arguments.Add(new ProcessorArgument(HttpPipelineFormatter.ArgumentHttpResponseMessage,typeof (HttpResponseMessage)));
            
            
            return arguments.ToArray();

        }

        protected override IEnumerable<ProcessorArgument> OnGetOutArguments() {
            
                return null;
            
        }

        protected override ProcessorResult OnExecute(object[] input) {
            
            if (_InRequestPipeline) {
                var request = (HttpRequestMessage)input[0]; ;
                return new ProcessorResult();

            } else {

                var request = (HttpRequestMessage)input[0]; ;
                var response = (HttpResponseMessage) input[1];
                
                var w3clogEntry = string.Format(
                    "{0:HH:mm:ss.fff} {1} {2} {3} {4}", 
                    DateTime.Now, request.Headers.From, request.Method, request.RequestUri, response.StatusCode);

                _Logger.Log(w3clogEntry);
                return new ProcessorResult();
            }
            
        }
    }
}
