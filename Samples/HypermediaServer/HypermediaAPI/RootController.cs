using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Hal;

namespace HypermediaAPI {

    [ServiceContract]
    public class RootController {

        [WebGet(UriTemplate="")]
        public HttpResponseMessage Get(HttpRequestMessage requestMesssage) {
            var response = new HttpResponseMessage();
            var rootHal = new HalDocument(requestMesssage.RequestUri.OriginalString);
            rootHal.AddProperty("Hello", "world");
            var content = new StreamContent(rootHal.ToStream());
            content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.hal+xml");
            response.Content = content;
            
            return response;
        }

        [WebGet(UriTemplate = "View")]
        public HttpResponseMessage GetView(HttpRequestMessage requestMesssage) {
            return new HttpResponseMessage() { StatusCode = HttpStatusCode.NotImplemented };
        }


        



    }
}
