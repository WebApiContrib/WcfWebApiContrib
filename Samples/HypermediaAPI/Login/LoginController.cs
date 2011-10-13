using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace HypermediaAPI.Login {
    [ServiceContract]
    public class LoginController {
        [WebGet(UriTemplate = "")]
        public HttpResponseMessage Get(HttpRequestMessage requestMesssage) {
            return new HttpResponseMessage() { StatusCode = HttpStatusCode.NotImplemented };
        }
        [WebGet(UriTemplate = "View")]
        public HttpResponseMessage GetView(HttpRequestMessage requestMesssage) {
            return new HttpResponseMessage() { StatusCode = HttpStatusCode.NotImplemented };
        }
    }
}
