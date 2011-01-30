using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.ServiceModel.Description;
using Microsoft.ServiceModel.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace Http.Formatters
{
    /// <summary>
    /// Formats requests for application/bson.
    /// </summary>
    /// <remarks>
    /// Christian Weyer is the author of this MediaTypeProcessor.
    /// <see href="http://weblogs.thinktecture.com/cweyer/2010/12/using-jsonnet-as-a-default-serializer-in-wcf-httpwebrest-vnext.html"/>
    /// </remarks>
    public class BsonProcessor : MediaTypeProcessor
    {
        private Type parameterType;

        public BsonProcessor(HttpOperationDescription operation, MediaTypeProcessorMode mode)
            : base(operation, mode)
        {
            if (this.Parameter != null)
            {
                this.parameterType = this.Parameter.ParameterType;
            }
        }

        public override IEnumerable<string> SupportedMediaTypes
        {
            get { return new List<string> { "application/bson" }; }
        }

        public override void WriteToStream(object instance, Stream stream, HttpRequestMessage request)
        {
            var serializer = new JsonSerializer();
            using (var writer = new BsonWriter(stream))
            {
                serializer.Serialize(writer, instance);
            }
        }

        public override object ReadFromStream(Stream stream, HttpRequestMessage request)
        {
            var serializer = new JsonSerializer();
            using (var reader = new BsonReader(stream))
            {
                var result = serializer.Deserialize(reader, parameterType);
                return result;
            }
        }
    }
}
