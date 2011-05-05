using System;
using System.Net.Http;
using Microsoft.ApplicationServer.Http.Dispatcher;

namespace SelfhostedServer.OperationHandlers {
    public class ResponseLoggingProcessor : HttpOperationHandler<HttpRequestMessage, HttpResponseMessage, HttpResponseMessage> {
        private readonly ILogger _Logger;

        public ResponseLoggingProcessor(ILogger logger) : base("httpResponseMessage") {
            _Logger = logger;
        }


        public override HttpResponseMessage OnHandle(HttpRequestMessage request, HttpResponseMessage response) {
            var w3clogEntry = string.Format(
              "{0:HH:mm:ss.fff} {1} {2} {3} {4}",
              DateTime.Now, request.Headers.From, request.Method, request.RequestUri, response.StatusCode);

            _Logger.Log(w3clogEntry);

            return response;
        }
    }
}
