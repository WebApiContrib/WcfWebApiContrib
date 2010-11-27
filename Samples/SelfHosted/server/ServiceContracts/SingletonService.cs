using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;
using Microsoft.Http;

namespace SelfhostedServer.ServiceContracts
{
    [ServiceContract]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class SingletonService {
        private readonly Func<HttpRequestMessage, HttpResponseMessage> _app;

        public SingletonService(Func<HttpRequestMessage, HttpResponseMessage> app) {
            _app = app;
        }

//        [WebGet(UriTemplate = "root")]
//        public HttpResponseMessage GetServiceRoot(HttpRequestMessage httpRequestMessage) {
//            return new HttpResponseMessage
//            {
//                StatusCode = HttpStatusCode.OK,
//                Content = HttpContent.Create(@"<html>
//  <a href='foo'>Get plain text response by returning a string</a><br/>
//  <a href='foo2'>Get plain text response by modifying a passed HttpResponseMessage</a>
//</html>", "text/html")
//            };
//        }

        [WebGet(UriTemplate = "*")]
        public void Get(HttpRequestMessage httpRequestMessage, HttpResponseMessage httpResponseMessage) {
            Handle(httpRequestMessage, httpResponseMessage);
        }

        [WebInvoke(UriTemplate = "*", Method = "HEAD")]
        public void Head(HttpRequestMessage httpRequestMessage, HttpResponseMessage httpResponseMessage) {
            Handle(httpRequestMessage, httpResponseMessage);
        }

        [WebInvoke(UriTemplate = "*", Method = "POST")]
        public void Post(HttpRequestMessage httpRequestMessage, HttpResponseMessage httpResponseMessage) {
            Handle(httpRequestMessage, httpResponseMessage);
        }

        [WebGet(UriTemplate = "Foo")]
        public string GetFoo()
        {
            return "If you use fiddler and put text/plain it will return as text/plain, otherwise it will come back as an xml serialized string";
        }

        private static void Map(HttpResponseMessage to, HttpResponseMessage from) {
            to.StatusCode = from.StatusCode;
            to.Method = from.Method;
            to.Uri = from.Uri;
            to.Headers = from.Headers;
            to.Content = from.Content;
            to.Properties.Clear();
            foreach (object property in from.Properties) {
                to.Properties.Add(property);
            }
        }

        private void Handle(HttpRequestMessage request, HttpResponseMessage response) {
            HttpResponseMessage returned = _app(request);
            Map(response, returned);
        }
    }
}
