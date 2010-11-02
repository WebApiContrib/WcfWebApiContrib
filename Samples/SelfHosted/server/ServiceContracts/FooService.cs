using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;
using Microsoft.Http;

namespace SelfhostedServer.ServiceContracts {

    [ServiceContract]
    public class FooService {


        [WebGet(UriTemplate = "root")]
        [OperationContract]
        public void GetServiceRoot(HttpRequestMessage httpRequestMessage, HttpResponseMessage httpResponseMessage) {

            
            httpResponseMessage.Content = HttpContent.Create(@"<html>
                                                        <a href='foo'>Get plain text response using a returned HttpResponseMessage</a><br/>
                                                        <a href='foo2'>Get plain text response by modifiing a passed HttpResponseMessage</a>
                                                    </html>", "text/html");
            httpResponseMessage.StatusCode = HttpStatusCode.OK;
        }



        // Returning HttpResponseMEssage is enabled by the RawHttpProcessor
        [WebGet(UriTemplate = "Foo")]
        [OperationContract]
        public HttpResponseMessage GetFoo(HttpRequestMessage httpRequestMessage) {

            var response = new HttpResponseMessage();
            response.Content = HttpContent.Create("Hello World", "text/plain");
            return response;
        }


        // Recommended way to impact the httpResponseMessage directly
        [WebGet(UriTemplate = "Foo2")]
        [OperationContract]
        public void GetFoo2(HttpRequestMessage httpRequestMessage, HttpResponseMessage httpResponseMessage) {

            httpResponseMessage.Content = HttpContent.Create("Hello World", "text/plain");
            httpResponseMessage.StatusCode = HttpStatusCode.OK;
        }


    }
}
