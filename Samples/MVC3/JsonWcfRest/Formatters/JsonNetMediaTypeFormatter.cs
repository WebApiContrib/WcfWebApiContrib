using System;
using System.IO;
using System.Net;
using System.Net.Http.Headers;

using Microsoft.ApplicationServer.Http;

using Newtonsoft.Json;

namespace JsonWcfRest.Formatters
{
	public class JsonNetMediaTypeFormatter : MediaTypeFormatter
	{
		private static readonly JsonSerializer jsonSerializer = new JsonSerializer();

		public JsonNetMediaTypeFormatter()
		{
			SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
		}

		public override object OnReadFromStream(Type type, Stream stream, HttpContentHeaders contentHeaders)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}

			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}

			using (var streamReader = new StreamReader(stream))
			{
				using (var jsonTextReader = new JsonTextReader(streamReader))
				{
					return jsonSerializer.Deserialize(jsonTextReader, type);
				}
			}
		}

		public override void OnWriteToStream(Type type, object value, Stream stream, HttpContentHeaders contentHeaders, TransportContext context)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}

			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}

			using (var streamWriter = new StreamWriter(stream))
			{
				using (var jsonTextWriter = new JsonTextWriter(streamWriter))
				{
					jsonSerializer.Serialize(jsonTextWriter, value);
				}
			}
		}
	}
}