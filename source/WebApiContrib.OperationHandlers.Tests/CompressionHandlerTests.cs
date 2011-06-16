
using System;
using System.Net.Http;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ApplicationServer.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebApiContrib.OperationHandlers.Tests {
    [TestClass]
    public class CompressionHandlerTests {


        [TestMethod]
        public void CompressingContentShouldRetainContentType() {

            //Arrange
            var stringContent = new StringContent("Hello World", Encoding.UTF8,"text/plain");

            //Act
            var compressedContent = new CompressedContent(stringContent);

            //Assert
            Assert.AreEqual("text/plain",compressedContent.Headers.ContentType.MediaType);
        }

        [TestMethod]
        public void CompressingObjectContentShouldRetainContentType() {

            //Arrange
            var stringContent = new ObjectContent<string>("Hello World", "text/plain");

            //Act
            var compressedContent = new CompressedContent(stringContent);

            //Assert
            Assert.AreEqual("text/plain", compressedContent.Headers.ContentType.MediaType);
        }

        [TestMethod]
        public void CompressingObjectContentShouldRetainContentType2() {

            //Arrange
            var jsonContent = new ObjectContent<Foo>(new Foo() {Bar = "Hello"}, "application/json");

            //Act
            var compressedContent = new CompressedContent(jsonContent);

            //Assert
            Assert.AreEqual("application/json", compressedContent.Headers.ContentType.MediaType);
        }
    
    }

    public class Foo {
        public string Bar { get; set; }
    }
}
