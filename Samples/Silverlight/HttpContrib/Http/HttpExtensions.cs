namespace HttpContrib.Http
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Runtime.Serialization;
	using System.Runtime.Serialization.Json;
	using System.Xml.Serialization;
	
	public static class HttpExtensions
	{
		public static IEnumerable<T> ReadAsObjectList<T>(this HttpResponseMessage response)
		{
			if (IsJsonContent(response))
				return DeserializeFromJson<IEnumerable<T>>(response);

			if (IsXmlContent(response))
				return DeserializeFromXml<IEnumerable<T>>(response);

			throw new NotSupportedException("The response type is not supported.");
		}

		public static T ReadXmlAsObject<T>(this HttpResponseMessage response)
		{
			var serializer = new XmlSerializer(typeof(T));
			return (T)serializer.Deserialize(response.GetResponseStream());
		}

		public static Stream WriteObjectAsXml<T>(this T obj)
		{
			var serializer = new XmlSerializer(typeof(T));

			var stream = new MemoryStream();
			serializer.Serialize(stream, obj);
			stream.Position = 0;

			return stream;
		}

		public static T DeserializeFromJson<T>(this HttpResponseMessage message)
		{
			DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
			using (var stream = message.GetResponseStream())
			{
				return (T)serializer.ReadObject(stream);
			}
		}

		public static T DeserializeFromXml<T>(this HttpResponseMessage message)
		{
			DataContractSerializer serializer = new DataContractSerializer(typeof(T));
			using (var stream = message.GetResponseStream())
			{
				return (T)serializer.ReadObject(stream);
			}
		}

		public static bool IsJsonContent(this HttpResponseMessage message)
		{
			return message.ContentType == MediaType.Json;
		}

		public static bool IsXmlContent(this HttpResponseMessage message)
		{
			return message.ContentType == MediaType.Xml;
		}
	}
}