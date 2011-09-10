using System;
using System.IO;
using System.Net;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Xml.Linq;

namespace WebApiContrib.Formatters.Core {
    public class XmlUsingXDocumentFormatter : XmlMediaTypeFormatter {

        public XmlUsingXDocumentFormatter() {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/xml"));
        }

        protected override object OnReadFromStream(Type type, Stream stream, HttpContentHeaders contentHeaders) {
            stream.Position = 0;
            return XDocument.Load(stream);
        }

        protected override void OnWriteToStream(Type type, object value, Stream stream, HttpContentHeaders contentHeaders, TransportContext context) {
            ((XDocument)value).Save(stream);
        }
    }
}