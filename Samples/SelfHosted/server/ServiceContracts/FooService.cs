using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;
using Microsoft.Http;

namespace SelfhostedServer.ServiceContracts {

    [ServiceContract]
    public class FooService {
        private readonly ILogger _Logger;

        public FooService(ILogger logger) {
            _Logger = logger;
        }

        [WebGet(UriTemplate = "root")]
        [OperationContract]
        public void GetServiceRoot(HttpRequestMessage httpRequestMessage, HttpResponseMessage httpResponseMessage) {

            
            httpResponseMessage.Content = HttpContent.Create(@"<html>
                                                        <a href='foo'>Get plain text response by returning a string</a><br/>
                                                        <a href='foo2'>Get plain text response by modifiing a passed HttpResponseMessage</a>
                                                    </html>", "text/html");
            httpResponseMessage.StatusCode = HttpStatusCode.OK;
        }





        // Recommended way to impact the httpResponseMessage directly
        [WebGet(UriTemplate = "Foo2")]
        [OperationContract]
        public void GetFoo2(HttpRequestMessage httpRequestMessage, HttpResponseMessage httpResponseMessage) {

            httpResponseMessage.Content = HttpContent.Create("Hello World", "text/plain");
            httpResponseMessage.StatusCode = HttpStatusCode.OK;
        }


        
        [WebGet(UriTemplate = "Foo")]
        [OperationContract]
        public string GetFoo() {
            
            return "If you use fiddler and put text/plain it will return as text/plain, otherwise it will come back as an xml serialized string";
        }


    }
}
