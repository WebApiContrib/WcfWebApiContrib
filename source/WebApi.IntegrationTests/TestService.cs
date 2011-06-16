using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
         
            var response = new HttpResponseMessage(HttpStatusCode.OK,"OK");
            response.Content = new StringContent(prop.Address);
            return response;
        }
    }
}
