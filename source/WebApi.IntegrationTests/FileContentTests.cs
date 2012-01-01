using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ApplicationServer.Http;
using Microsoft.ApplicationServer.Http.Activation;
using Microsoft.ApplicationServer.Http.Description;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebApi.IntegrationTests {
    [TestClass]
    public class FileContentTests {
        [TestMethod]
        public void LoadStreamContentFromEmbeddedResource() {
            var content = new StreamContent(GetType().Assembly.GetManifestResourceStream(GetType(), "XForms.pdf"));
            //var content = new StringContent("Hwllo world");
            var serviceUri = new Uri("http://localhost:1017/");
            var config = new HttpConfiguration();

            var host = new HttpServiceHost(typeof(TestService),config, new[] { serviceUri });
            // Configure Endpoint for transferring large files
            host.AddDefaultEndpoints();
            var endpoint = ((HttpEndpoint)host.Description.Endpoints[0]);
            endpoint.TransferMode = TransferMode.Streamed;
            endpoint.MaxReceivedMessageSize = 1024 * 1024 * 10;

            host.Open();

            var httpClient = new HttpClient();
            httpClient.BaseAddress = serviceUri;

            var response = httpClient.PostAsync("ResourceA",content).Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }


        [TestMethod]
        public void PostContent() {
            var content = new StringContent("This is some content");
            content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
            var serviceUri = new Uri("http://localhost:1017/");
            var config = new HttpConfiguration();

            var host = new HttpServiceHost(typeof(TestService), config, new[] { serviceUri });

            host.Open();

            var httpClient = new HttpClient();
            httpClient.BaseAddress = serviceUri;

            var response = httpClient.PostAsync("ResourceA", content).Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

    }
}
