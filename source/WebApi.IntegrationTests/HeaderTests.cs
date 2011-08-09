using System;
using System.Net.Http;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ApplicationServer.Http.Activation;
using Microsoft.ApplicationServer.Http.Description;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebApi.IntegrationTests {
    [TestClass]
    public class HeaderTests {
        [TestMethod]
        public void ReasonPhraseShouldBeReturnedToTheClient() {
            var serviceUri = new Uri("http://localtmserver:1017/");
            var config = HttpHostConfiguration.Create();

            var host = new HttpConfigurableServiceHost<TestService>(config, new[] { serviceUri });
            host.Open();

            var httpClient = new HttpClient(serviceUri);

            var response = httpClient.Get("ResourceWithReasonPhrase");

            Assert.AreEqual("All Good", response.ReasonPhrase);
        }
    }
}
