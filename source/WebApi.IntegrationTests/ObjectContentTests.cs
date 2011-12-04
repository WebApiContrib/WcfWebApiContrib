using System;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.ApplicationServer.Http;
using Microsoft.ApplicationServer.Http.Activation;
using Microsoft.ApplicationServer.Http.Description;
using Microsoft.ApplicationServer.Http.Dispatcher;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApiContrib.Formatters.Core;
using WebApiContrib.OperationHandlers;
using PlainTextFormatter = WebApiContrib.Formatters.Core.PlainTextFormatter;

namespace WebApi.IntegrationTests {
    [TestClass]
    public class ObjectContentTests {


            /// <summary>
            /// Issue 1:  If no formatter exists that supports the media-type, then media type of
            /// the HttpContent is changes to match a formatter that is capable of writing the type
            /// 
            /// I believe if the Operation returns an ObjectContent with an explicitly specified media-type 
            /// and no appropriate Formatter has been provided, then the operation should fail.
            /// </summary>
            [TestMethod]
            public void SpecifyMediaTypeAndSerialize() {
                //Arrange
                var content = new ObjectContent<string>("woof", "text/plain");
                // This will cause the test to pass: content.Formatters.Add(new PlainTextFormatter());

                //Act
                var result = content.ReadAsStringAsync().Result; // Why does this change the media type of my content?
                var mediaType = content.Headers.ContentType.MediaType;

                //Assert
                Assert.AreEqual("text/plain", mediaType);  // This fails because it falls back the XmlFormatter

            }





            /// <summary>
            /// Issue 2:  The operation called by this test returns an ObjectContent that has the media-type
            /// set to 'text/plain' and provides an instance of the TextPlainFormatter.
            /// However, ResponseContentHandler removes all Formatters and replaces them with those configured by the host.
            /// I think it would be much better to combine the two collections.  Imagine a host with many operations and
            /// a small percentage of those operations require a set of specialized formatters.  It would be necessary to configure
            /// the specialized formatters centrally rather than just where they are used.
            /// 
            /// </summary>
            [TestMethod]
            public void RequestOverridesProvidedFormatters() {
                //Arrange

                var serviceUri = new Uri("http://localhost:1001/");
                var config = new HttpConfiguration();

                var host = new HttpServiceHost(typeof(FooService), config, new[] { serviceUri });
                host.Open();

                var client = new HttpClient();
                client.BaseAddress = serviceUri;

                //Act
                var response = client.GetAsync("foo2").Result;

                var mediaType = response.Content.Headers.ContentType.MediaType;

                //Assert
                Assert.AreEqual("text/plain", mediaType);

                host.Close();
            }


            /// <summary>
            /// Issue 3:  Although the operation explictly sets the media type of the ObjectContent that is to be returned
            /// the Accept header is considered a higher priority and causes the response to be sent as application/json
            /// instead of text/plain.
            /// </summary>
            [TestMethod]
            public void RequestAcceptHeaderOverridesSpecifiedMediaType() {
                //Arrange


                var request = new HttpRequestMessage();
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var serviceUri = new Uri("http://localhost:1001/");
                var config = new HttpConfiguration();
                    config.Formatters.Add(new PlainTextFormatter());

                var host = new HttpServiceHost(typeof(FooService), config, new[] { serviceUri });
                host.Open();

                var client = new HttpClient();
                client.BaseAddress = serviceUri;

                //Act
                request.RequestUri = new Uri(serviceUri, "foo");
                var response = client.SendAsync(request).Result;

                var mediaType = response.Content.Headers.ContentType.MediaType;

                //Assert
                Assert.AreEqual("text/plain", mediaType); //Fails with Actual = application/json

                host.Close();
            }

            // Since Preview 6 this now fails. Not exactly sure why, but I think it is related to ObjectContent
            [TestMethod]
            public void CompressionHandlerChangesMediaType() {
                //Arrange


                var request = new HttpRequestMessage();
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var serviceUri = new Uri("http://localhost:1001/");
                var config = new HttpConfiguration();
                
                config.ResponseHandlers = (handlers, se, od) => handlers.Add(new CompressionHandler());


                var host = new HttpServiceHost(typeof(FooService), config, new[] { serviceUri });
                host.Open();

                var client = new HttpClient();
                client.BaseAddress = serviceUri;
                client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                //Act
                request.RequestUri = new Uri(serviceUri, "foo3");
                var response = client.SendAsync(request).Result;

                var mediaType = response.Content.Headers.ContentType.MediaType;

                //Assert
                Assert.AreEqual("application/json", mediaType);

                host.Close();
            }



            #region Tests that work!

            [TestMethod]
            public void RoundTripAnObject() {
                //Arrange
                var input = new foo { Bar = "hello" };
                var content = new ObjectContent<foo>(input);

                //Act
                var output = content.ReadAsAsync<foo>().Result;

                //Assert
                Assert.AreEqual(input, output);

            }


            [TestMethod]
            public void CheckDefaultFormatters() {
                //Arrange
                var input = new foo { Bar = "hello" };
                var content = new ObjectContent<foo>(input);

                //Act
                var formatters = content.Formatters;

                //Assert
                Assert.AreEqual(4, formatters.Count);

            }

            [TestMethod]
            public void SpecifyMediaType() {
                //Arrange
                var input = new foo { Bar = "hello" };
                var content = new ObjectContent<foo>(input, "text/plain");

                //Act
                var mediaType = content.Headers.ContentType.MediaType;

                //Assert
                Assert.AreEqual("text/plain", mediaType);

            }


            [TestMethod]
            public void SpecifyMediaTypeAndSerializeAsJson() {
                //Arrange
                var input = new foo { Bar = "hello" };
                var content = new ObjectContent<foo>(input, "application/json");

                //Act
                var result = content.ReadAsStringAsync().Result;
                var mediaType = content.Headers.ContentType.MediaType;

                //Assert
                Assert.AreEqual("application/json", mediaType);

            }

            [TestMethod]
            public void SpecifyMediaTypeAndSerializeAsPlainTextWithACustomFormatter() {
                //Arrange

                var content = new ObjectContent<string>("woof", new MediaTypeHeaderValue("text/plain"), new List<MediaTypeFormatter> { new PlainTextFormatter() });
                var request = new HttpRequestMessage();

                //Act
                var result = content.ReadAsStringAsync().Result;
                var mediaType = content.Headers.ContentType.MediaType;

                //Assert
                Assert.AreEqual("text/plain", mediaType);

            }


            [TestMethod]
            public void DeserializeContent() {
                //Arrange
                var httpContent = new StringContent("Hello world");
                var content = new ObjectContent<string>(httpContent, new[] { new PlainTextFormatter() });

                //Act
                var result = content.ReadAsAsync<string>().Result;

                //Assert
                Assert.AreEqual("Hello world", result);  // This fails because it falls back the XmlFormatter

            }

            #endregion

            [TestMethod]
            public void ObjectContentDoesNotNeedType() {

                var content = new ByteArrayContent(Encoding.UTF8.GetBytes("Hello World"));
                content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");

                var objectContent = new ObjectContent(typeof(object), content, new[] { new PlainTextFormatter() });
                var result = objectContent.ReadAsAsync().Result;

                Assert.AreEqual(typeof(string), result.GetType());

            }

            [TestMethod]
            public void ObjectContentDoesNotNeedType1() {

                var content = new ByteArrayContent(Encoding.UTF8.GetBytes("<foo>bar</foo>"));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/xml");

                var objectContent = new ObjectContent(typeof(object), content, new[] { new XmlUsingXDocumentFormatter() });
                var result = objectContent.ReadAsAsync().Result;

                Assert.AreEqual(typeof(XDocument), result.GetType());

            }


        }

    public class foo {
            public string Bar;

            public static explicit operator string(foo value) {
                return "foo: " + value.Bar;
            }
        }



        [ServiceContract]
        public class FooService {

            [WebGet(UriTemplate = "foo")]
            public HttpResponseMessage GetFoo() {
                var content = new ObjectContent<string>("woof", "text/plain", new[] { new PlainTextFormatter() });  // including formatters here doesn't actually help!
                var response = new HttpResponseMessage { Content = content };

                return response;
            }

            [WebGet(UriTemplate = "foo2")]
            public HttpResponseMessage<string> GetFoo2() {

                var response = new HttpResponseMessage<string>("woof");
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");

                return response;
            }

            [WebGet(UriTemplate = "foo3")]
            public foo GetFoo3() {

                return new foo() { Bar = "hello" };
            }

        }
    }

