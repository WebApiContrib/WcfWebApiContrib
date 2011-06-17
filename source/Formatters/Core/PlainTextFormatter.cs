using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.ApplicationServer.Http;

namespace WebApiContrib.Formatters.Core {
    public class PlainTextFormatter : MediaTypeFormatter {
        public PlainTextFormatter() {
            this.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain"));
        }

        public override void OnWriteToStream(Type type, object value, Stream stream, System.Net.Http.Headers.HttpContentHeaders contentHeaders, System.Net.TransportContext context) {
            var output = (string)value;
            var writer = new StreamWriter(stream);
            writer.Write(output);
        }

        public override object OnReadFromStream(Type type, Stream stream, System.Net.Http.Headers.HttpContentHeaders contentHeaders) {
            return new StreamReader(stream).ReadToEnd();
        }
    }

}
