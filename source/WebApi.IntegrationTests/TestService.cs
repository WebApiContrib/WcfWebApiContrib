using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using Microsoft.ApplicationServer.Http;

namespace WebApi.IntegrationTests {
    [ServiceContract]
    public class TestService {


        // Operation should be able to determine Remote Address of the request
        // Apparently this should work, as per Henrik, but it currently does not.
        [WebGet(UriTemplate="ResourceA")]
        public HttpResponseMessage GetResourceA(HttpRequestMessage httpRequestMessage) {

            var prop = OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
         
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(prop.Address);
            return response;
        }

        [WebInvoke(Method = "POST",UriTemplate = "ResourceA")]
        public HttpResponseMessage PostResourceA(HttpRequestMessage httpRequestMessage) {
            

            var stream = httpRequestMessage.Content.ContentReadStream;
            var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(memoryStream.Length.ToString());
            return response;
        }


        [WebGet(UriTemplate = "ResourceWithReasonPhrase")]
        public HttpResponseMessage GetResourceWithReasonPhrase(HttpRequestMessage httpRequestMessage) {

            
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent("Hi!");
            response.ReasonPhrase = "All Good";
            return response;
        }
    }
}
