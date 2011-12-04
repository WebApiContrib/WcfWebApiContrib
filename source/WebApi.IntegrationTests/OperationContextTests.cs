using System;
using System.Net.Http;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ApplicationServer.Http;
using Microsoft.ApplicationServer.Http.Activation;
using Microsoft.ApplicationServer.Http.Description;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebApi.IntegrationTests {
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class OperationContextTests {

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void ShouldReturnRemoteAddress() {

            var serviceUri = new Uri("http://localhost:1017/");
            var config = new HttpConfiguration();

            var host = new HttpServiceHost(typeof(TestService),config, new[] { serviceUri });
            host.Open();

            var httpClient = new HttpClient();
            httpClient.BaseAddress = serviceUri;

            var response = httpClient.GetAsync("ResourceA").Result;

            Assert.AreEqual("127.0.0.1", response.Content.ReadAsStringAsync().Result);
        }
    }
}
