using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.ServiceModel.Web;
using Microsoft.ApplicationServer.Http;

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

        internal static void CreateHosts(System.Collections.Generic.List<Microsoft.ApplicationServer.Http.HttpServiceHost> hosts, Microsoft.ApplicationServer.Http.HttpConfiguration config, string baseurl) {
            hosts.Add(new HttpServiceHost(typeof(HypermediaAPI.Login.LoginController), config, baseurl));
        }
    }
}
