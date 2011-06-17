using System;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Xml.Linq;
using Microsoft.ApplicationServer.Http;

namespace WebApiContrib.Formatters.Core {
    public class XmlUsingXDocumentFormatter : MediaTypeFormatter {

        public XmlUsingXDocumentFormatter() {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/xml"));
        }
        public override object OnReadFromStream(Type type, Stream stream, HttpContentHeaders contentHeaders) {

            return XDocument.Load(stream);
        }

        public override void OnWriteToStream(Type type, object value, Stream stream, HttpContentHeaders contentHeaders, TransportContext context) {
            ((XDocument)value).Save(stream);
        }
    }
}