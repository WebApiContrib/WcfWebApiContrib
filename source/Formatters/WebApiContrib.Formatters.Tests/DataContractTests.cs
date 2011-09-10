using System;
using System.Net.Http;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ApplicationServer.Http;
using Microsoft.ApplicationServer.Http.Activation;
using Microsoft.ApplicationServer.Http.Description;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApiContrib.Formatters.Core;

namespace WebApiContrib.Formatters.Tests {
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class DataContractTests {
        [TestMethod]
        public void RoundTripUsingDataContract() {

            var config = new HttpConfiguration();
            config.Formatters.UseDataContractSerializer<Contact>();

            var host = new HttpServiceHost(typeof(ContactService),config, new Uri("http://localhost:8080/"));
            host.Open();

            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:8080/");
            var newContact = new Contact { FirstName = "Brad", LastName = "Abrams" };
            var content = new ObjectContent<Contact>(newContact, "application/xml").UseDataContractSerializer();
            var response = client.Post("contacts", content);
            response = client.Get("contacts");

            var contacts = response.Content.ReadAsDataContract<Contact[]>();

            Assert.AreEqual(1, contacts.Count());
            var contact = contacts.First();
            Assert.AreEqual("Brad", contact.FirstName);
            Assert.AreEqual("Abrams", contact.LastName);

        }
    }
}
