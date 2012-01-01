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
using HypermediaAPI.Tools;
using Microsoft.ApplicationServer.Http;

namespace HypermediaAPI {

    [ServiceContract]
    public class RootController {

        [WebGet(UriTemplate="")]
        public HttpResponseMessage Get(HttpRequestMessage requestMesssage) {
            var response = new HttpResponseMessage();
            var rootHal = new HalDocument(requestMesssage.RequestUri.OriginalString);
            rootHal.AddLink("urn:hapi:template", new Uri("template", UriKind.Relative));
            rootHal.AddLink("urn:hapi:partners", new Uri("partner/list",UriKind.Relative));
            rootHal.AddLink("urn:hapi:transactions", new Uri("transaction/list", UriKind.Relative));
            if (IsAuthenticated(requestMesssage)) {
                rootHal.AddProperty("Authenticated", "true");
            } else {
                rootHal.AddProperty("Authenticated", "false");
            }

            response.Content = new HalContent(rootHal);
            
            return response;
        }

        [WebGet(UriTemplate = "template")]
        public HttpResponseMessage GetTemplate(HttpRequestMessage requestMesssage) {
            return new HttpResponseMessage() {
                                                 StatusCode = HttpStatusCode.OK,
                                                 Content = new StringContent("<Textblock>Hello world</Textblock")
            };
        }


        private bool IsAuthenticated(HttpRequestMessage httpRequestMessage) {
            return false;
        }


        public static void CreateHosts(List<HttpServiceHost> hosts, HttpConfiguration config, Uri baseurl) {
            hosts.Add(new HttpServiceHost(typeof(HypermediaAPI.RootController), config, baseurl));

            Login.LoginController.CreateHosts(hosts,config,baseurl + "Login/");
            Partner.PartnerController.CreateHosts(hosts, config, baseurl + "Partner/");

        }
    }
}
