using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.Web;
using System.ServiceModel.Dispatcher;
using System.Json;
using System.IO;

namespace Formatters
{
    using System.Reflection;
    using System.Runtime.Serialization.Json;

    using Microsoft.ServiceModel.Http;

    public class JsonValueProcessor : MediaTypeProcessor
    {
        private bool isJsonValueParameter;
        private Type parameterType;
        private DataContractJsonSerializer serializer;

        public JsonValueProcessor(HttpParameterDescription parameter,MediaTypeProcessorMode mode )
            :base(parameter, mode)
        {
            if (parameter != null)
            {
                parameterType = parameter.ParameterType;
                isJsonValueParameter = typeof (JsonValue).IsAssignableFrom(parameterType);
                if (!isJsonValueParameter)
                    serializer = new DataContractJsonSerializer(parameterType);
            }
        }

        public override IEnumerable<string> SupportedMediaTypes
        {
            get
            {
                return new List<string> { "application/json", "application/x-www-form-urlencoded"}; 
            }
        }

        public override void WriteToStream(object instance, Stream stream)
        {
            JsonValue value = null;

            if (isJsonValueParameter)
            {
                value = (JsonValue)instance;
                value.Save(stream);
            }
            else
            {
                serializer.WriteObject(stream, instance);
            }
        }

        public override object ReadFromStream(Stream stream)
        {
            var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();
            var jsonObject = JsonObject.ParseFormUrlEncoded(json);

            if (isJsonValueParameter)
                return jsonObject;

            return serializer.ReadObject(stream);
        }
    }
}