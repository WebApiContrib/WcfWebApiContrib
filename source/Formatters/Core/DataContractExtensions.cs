using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.Serialization;
using Microsoft.ApplicationServer.Http;
using Microsoft.ApplicationServer.Http.Description;

namespace WebApiContrib.Formatters.Core
{
    /// <summary>
    /// Helpers for working with DataContracts
    /// </summary>
    public static class DataContractExtensons
    {
        //Registers a specific type to use the DataContractSerializer. 
        public static void UseDataContractSerializer<T>(this IHttpHostConfigurationBuilder configuration, params Type[] knownTypes)
        {
            var config = (HttpHostConfiguration) configuration;
            var formatter = config.OperationHandlerFactory.Formatters.XmlFormatter;

            //Add for T, for List<T> and for T[]
            formatter.SetSerializer<T>(new DataContractSerializer(typeof(T), knownTypes));
            formatter.SetSerializer<List<T>>(new DataContractSerializer(typeof(List<T>), knownTypes));
            formatter.SetSerializer<T[]>(new DataContractSerializer(typeof(T[]), knownTypes));
        }

        //Registers a specific type to use the DataContractSerializer. 
        public static ObjectContent<T> UseDataContractSerializer<T>(this ObjectContent<T> content, params Type[] knownTypes)
        {
            content.Formatters.XmlFormatter.SetSerializer<T>(new DataContractSerializer(typeof(T), knownTypes));
            return content;
        }

        //Read a type using the DataContractSerializer
        public static T ReadAsDataContract<T>(this HttpContent content, params Type[] knownTypes)
        {
            var formatter = new XmlMediaTypeFormatter();
            formatter.SetSerializer<T>(new DataContractSerializer(typeof(T), knownTypes));
            return content.ReadAs<T>(new List<MediaTypeFormatter> { formatter });
        }

    }
}
