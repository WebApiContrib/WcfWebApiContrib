
using System;
using System.Net.Http;
using System.Text;
using System.Collections.Generic;
using System.Linq;
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
    }
}
