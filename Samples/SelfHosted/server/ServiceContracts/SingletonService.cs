using System;
using System.Net.Http;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace SelfhostedServer.ServiceContracts
{
    [ServiceContract]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class SingleresponsenService {
        private readonly Func<HttpRequestMessage, HttpResponseMessage> _app;

        public SingleresponsenService(Func<HttpRequestMessage, HttpResponseMessage> app) {
            _app = app;
        }

		[OperationContract]
        [WebInvoke(UriTemplate = "*", Method = "*")]
        public void Invoke(HttpRequestMessage httpRequestMessage, HttpResponseMessage httpResponseMessage) {
            Handle(httpRequestMessage, httpResponseMessage);
        }

        [WebGet(UriTemplate = "Foo")]
        public string GetFoo()
        {
            return "If you use fiddler and put text/plain it will return as text/plain, otherwise it will come back as an xml serialized string";
        }

        private void Handle(HttpRequestMessage request, HttpResponseMessage response) {
            HttpResponseMessage returned = _app(request);
            response.StatusCode = returned.StatusCode;

            response.Headers.Clear();
            foreach (var header in returned.Headers)
            {
                response.Headers.Add(header.Key, header.Value);
            }

            response.Content = returned.Content;
        }
    }
}
