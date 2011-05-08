using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel.Description;
using Microsoft.ApplicationServer.Http;
using Microsoft.ApplicationServer.Http.Description;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace Formatters
{
    /// <summary>
    /// Formats requests for application/bson.
    /// </summary>
    /// <remarks>
    /// Christian Weyer is the author of this MediaTypeProcessor.
    /// <see href="http://weblogs.thinktecture.com/cweyer/2010/12/using-jsonnet-as-a-default-serializer-in-wcf-httpwebrest-vnext.html"/>
    /// </remarks>
    public class BsonFormatter : MediaTypeFormatter
    {
        
        public BsonFormatter() 
        {
              SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/bson"));
        }

   


        public override object OnReadFromStream(Type type, Stream stream, HttpContentHeaders contentHeaders) {
            var serializer = new JsonSerializer();
            using (var reader = new BsonReader(stream)) {
                var result = serializer.Deserialize(reader, type);
                return result;
            }
        }

        public override void OnWriteToStream(Type type, object value, Stream stream, HttpContentHeaders contentHeaders, TransportContext context) {
            var serializer = new JsonSerializer();
            using (var writer = new BsonWriter(stream.PreventClose())) {
                serializer.Serialize(writer, value);
                writer.Flush();
            }
        }
    }
}
