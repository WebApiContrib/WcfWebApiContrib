using System;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using Microsoft.ApplicationServer.Http;
using Newtonsoft.Json;

namespace Formatters
{
    /// <summary>
    /// Formats requests for text/json and application/json using Json.Net.
    /// </summary>
    /// <remarks>
    /// Christian Weyer is the author of this MediaTypeProcessor.
    /// <see href="http://weblogs.thinktecture.com/cweyer/2010/12/using-jsonnet-as-a-default-serializer-in-wcf-httpwebrest-vnext.html"/>
    /// </remarks>
    public class JsonNetFormatter : MediaTypeFormatter
    {

        public JsonNetFormatter() {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/json"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
        }

        public override object OnReadFromStream(Type type, Stream stream, HttpContentHeaders contentHeaders) {
            var serializer = new JsonSerializer();
            using (var sr = new StreamReader(stream))
            using (var reader = new JsonTextReader(sr)) {
                var result = serializer.Deserialize(reader, type);
                return result;
            }
        }

        public override void OnWriteToStream(Type type, object value, Stream stream, HttpContentHeaders contentHeaders, TransportContext context) {
            var serializer = new JsonSerializer();
            using (var sw = new StreamWriter(stream.PreventClose()))
            using (var writer = new JsonTextWriter(sw)) {
                serializer.Serialize(writer, value);
                writer.Flush();
            }
        }
       
            
    }
}
