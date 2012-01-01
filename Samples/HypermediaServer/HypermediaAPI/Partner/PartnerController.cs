using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using HypermediaAPI.Partner.List;
using Microsoft.ApplicationServer.Http;

namespace HypermediaAPI.Partner {
    [ServiceContract]
    public class PartnerController {

        [WebGet(UriTemplate = "{id}")]
        public HttpResponseMessage Get(int id, HttpRequestMessage requestMessage) {
            return null;
        }

        internal static void CreateHosts(List<HttpServiceHost> hosts, HttpConfiguration config, string baseurl) {
            hosts.Add(new HttpServiceHost(typeof(PartnerController), config, baseurl));
            PartnerListController.CreateHosts(hosts,config,baseurl+"list/");
        }
    }
}
