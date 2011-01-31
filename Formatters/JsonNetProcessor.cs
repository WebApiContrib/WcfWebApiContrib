using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.ServiceModel.Description;
using Microsoft.ServiceModel.Http;
using Newtonsoft.Json;

namespace Http.Formatters
{
    /// <summary>
    /// Formats requests for text/json and application/json using Json.Net.
    /// </summary>
    /// <remarks>
    /// Christian Weyer is the author of this MediaTypeProcessor.
    /// <see href="http://weblogs.thinktecture.com/cweyer/2010/12/using-jsonnet-as-a-default-serializer-in-wcf-httpwebrest-vnext.html"/>
    /// </remarks>
    public class JsonNetProcessor : MediaTypeProcessor
    {
        private Type parameterType;

        public JsonNetProcessor(HttpOperationDescription operation, MediaTypeProcessorMode mode)
            : base(operation, mode)
        {
            if (this.Parameter != null)
            {
                this.parameterType = this.Parameter.ParameterType;
            }
        }

        public override IEnumerable<string> SupportedMediaTypes
        {
            get { return new List<string> { "text/json", "application/json" }; }
        }

        public override void WriteToStream(object instance, Stream stream, HttpRequestMessage request)
        {
            var serializer = new JsonSerializer();
            using (var sw = new StreamWriter(stream.PreventClose()))
            using (var writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, instance);
                writer.Flush();
            }
        }

        public override object ReadFromStream(Stream stream, HttpRequestMessage request)
        {
            var serializer = new JsonSerializer();
            using (var sr = new StreamReader(stream))
            using (var reader = new JsonTextReader(sr))
            {
                var result = serializer.Deserialize(reader, parameterType);
                return result;
            }
        }
    }
}
