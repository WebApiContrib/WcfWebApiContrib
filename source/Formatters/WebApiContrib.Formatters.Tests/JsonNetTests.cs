using System;
using System.Net.Http;
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
            var formatters = new[] {new JsonNetFormatter()};
            var fromContact = new Contact { FirstName = "Brad", LastName = "Abrams" };
            
            // Act
            var fromContent = new ObjectContent<Contact>(fromContact, "application/json", formatters);
            var contentReadStream = fromContent.ContentReadStream;
            contentReadStream.Position = 0;
            var toContent = new ObjectContent<Contact>(new StreamContent(contentReadStream), formatters);
            toContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var toContact = toContent.ReadAs();

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
            var contentReadStream = fromContent.ContentReadStream;
            contentReadStream.Position = 0;
            var toContent = new ObjectContent<Contact>(new StreamContent(contentReadStream), formatters);
            toContent.Headers.ContentType = new MediaTypeHeaderValue("application/bson");
            var toContact = toContent.ReadAs();

            //Arrange
            Assert.AreEqual(fromContact.FirstName, toContact.FirstName);
            Assert.AreEqual(fromContact.LastName, toContact.LastName);

        }

    
    }
}
