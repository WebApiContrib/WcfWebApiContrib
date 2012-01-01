using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ApplicationServer.Http;
using Microsoft.ApplicationServer.Http.Activation;
using Microsoft.ApplicationServer.Http.Description;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApiContrib.Formatters.JsonNet;

namespace WebApiContrib.Formatters.Tests {
    [TestClass]
    public class JsonNetTests {
 
        [TestMethod]
        public void RoundTripUsingJsonNet() {

            //Arrange
            var formatters = new MediaTypeFormatterCollection() {new JsonNetFormatter()};
            var fromContact = new Contact { FirstName = "Brad", LastName = "Abrams" };
            
            // Act
            var fromContent = new ObjectContent<Contact>(fromContact, new MediaTypeHeaderValue("application/json"), formatters);
            var contentString = fromContent.ReadAsStringAsync().Result;

            var toContent = new ObjectContent<Contact>(new StringContent(contentString), formatters);
            toContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var toContact = toContent.ReadAsAsync<Contact>().Result;

            //Arrange
            Assert.AreEqual(fromContact.FirstName, toContact.FirstName);
            Assert.AreEqual(fromContact.LastName, toContact.LastName);            

        }


        [TestMethod]
        public void RoundTripUsingBson() {

            //Arrange
            var formatters = new[] { new JsonNetFormatter() };
            var fromContact = new Contact { FirstName = "Brad", LastName = "Abrams" };

            // Act
            var fromContent = new ObjectContent<Contact>(fromContact, "application/bson", formatters);
            var contentReadStream = fromContent.ReadAsStreamAsync().Result;
            var newStream = new MemoryStream();
            contentReadStream.Position = 0;
            contentReadStream.CopyTo(newStream); 
            newStream.Position = 0;
            var toContent = new ObjectContent<Contact>(new StreamContent(newStream), formatters);
            toContent.Headers.ContentType = new MediaTypeHeaderValue("application/bson");
            var toContact = toContent.ReadAsAsync<Contact>().Result;

            //Arrange
            Assert.AreEqual(fromContact.FirstName, toContact.FirstName);
            Assert.AreEqual(fromContact.LastName, toContact.LastName);

        }

    
    }
}
