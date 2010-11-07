using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.Dispatcher;
using System.Text;
using Microsoft.Http;
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
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var request = (HttpRequestMessage)input[0]; ;
                request.Properties.Add(stopwatch);
                return new ProcessorResult();

            } else {

                var request = (HttpRequestMessage)input[0]; ;
                var response = (HttpResponseMessage) input[1];
                var stopwatch = (Stopwatch) request.Properties.Where(p=> p is Stopwatch).First();
                stopwatch.Stop();

                
                var w3clogEntry = string.Format("{0:HH:mm:ss.fff} {1} {2} {3} {4} {5} {6}",
                                                DateTime.Now, request.Headers.From, request.Method, request.Uri,
                                                response.StatusCode, stopwatch.ElapsedMilliseconds,
                                                response.Content.HasLength() ? response.Content.GetLength() : 0
                    );

                _Logger.Log(w3clogEntry);
                return new ProcessorResult();
            }
            
        }
    }
}
