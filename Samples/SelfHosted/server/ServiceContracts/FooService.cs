using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.ServiceModel.Web;
using OperationHandlers;
using SelfhostedServer.Host;
using SelfhostedServer.Resources;

namespace SelfhostedServer.ServiceContracts {

    [ServiceContract]
    public class FooService {
        private readonly ILogger _Logger;

        public FooService(ILogger logger) {
            _Logger = logger;
        }

        [WebGet(UriTemplate = "")]
        public HttpResponseMessage GetServiceRoot(HttpRequestMessage httpRequestMessage  ) {
            var httpResponseMessage = new HttpResponseMessage();
            httpResponseMessage.StatusCode = HttpStatusCode.OK;
            var content = new StringContent(@"<html>
                                                <a href='foo'>Get plain text response by returning a string</a><br/>
                                                <a href='foo2'>Get plain text response by modifiing a passed HttpResponseMessage</a>
                                              </html>");
            content.Headers.ContentType.MediaType = "text/html";
            httpResponseMessage.Content = content;
            return httpResponseMessage;
        }


        // Recommended way to impact the httpResponseMessage directly
        [WebGet(UriTemplate = "Foo2")]
        [OperationContract]
        public HttpResponseMessage GetFoo2(HttpRequestMessage httpRequestMessage) {
            var httpResponseMessage = new HttpResponseMessage();
            httpResponseMessage.StatusCode = HttpStatusCode.OK;
            httpResponseMessage.Content = new StringContent("Hello world", System.Text.Encoding.UTF8, "text/plain");
            return httpResponseMessage;
        }

        
        [WebGet(UriTemplate = "Foo")]
        public string GetFoo() {
            
            return "If you use fiddler and put text/plain it will return as text/plain, otherwise it will come back as an xml serialized string";
        }


        /// <summary>
        /// Return a strongly typed object.  We can use the default formatters in the response pipeline
        /// to convert this to Xml and Json.
        /// </summary>
        /// <returns></returns>
        [WebGet(UriTemplate = "PersonInfo")]
        public PersonInfo GetPersonInfo() {

            return new PersonInfo() {Name = "Bob Brown"};
        }


        /// <summary>
        /// Accept a strongly typed object and echo it back
        /// </summary>
        /// <param name="personInfo"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "PersonInfo", Method = "POST")]
        public PersonInfo PostPersonInfo(PersonInfo personInfo) {

            return new PersonInfo();
        }

        /// <summary>
        /// Return information based on some global stored state.
        /// </summary>
        /// <param name="serverState"></param>
        /// <returns></returns>
        [WebGet(UriTemplate = "ServerState")]
        public HttpResponseMessage GetServerState(ServerState serverState) {

            var httpResponseMessage = new HttpResponseMessage();
            httpResponseMessage.StatusCode = HttpStatusCode.OK;
            httpResponseMessage.Content = new StringContent((string)serverState["Hello"], System.Text.Encoding.UTF8, "text/plain");
            return httpResponseMessage;
        }

        [WebGet(UriTemplate = "CustomHeader")]
        public HttpResponseMessage CustomHeader(HttpRequestMessage requestMessage) {

            var httpResponseMessage = new HttpResponseMessage();
            var header = requestMessage.Headers.GetValues("x-foo");
            httpResponseMessage.Content = new StringContent(header.First(), System.Text.Encoding.UTF8, "text/plain");
            return httpResponseMessage;
            
        }

        

    }
}
