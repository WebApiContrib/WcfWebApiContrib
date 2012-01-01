using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Hal;
using HypermediaAPI.Tools;
using Microsoft.ApplicationServer.Http;

namespace HypermediaAPI.Partner.List {
    [ServiceContract]
    public class PartnerListController {

        [WebGet(UriTemplate = "")]
        public HttpResponseMessage Get(HttpRequestMessage requestMessage) {
            var halDocument = new HalDocument(requestMessage.RequestUri.OriginalString);
            halDocument.AddProperty("Hello", "World");
            return new HttpResponseMessage {
                                            Content = new HalContent(halDocument)
                                        };
        }

        public static void CreateHosts(List<HttpServiceHost> hosts, HttpConfiguration config, string baseurl) {
            hosts.Add(new HttpServiceHost(typeof(PartnerListController), config, baseurl));
        }
    }
}
