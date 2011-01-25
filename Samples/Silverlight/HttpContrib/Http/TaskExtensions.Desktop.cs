namespace HttpContrib.Http
{
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Net;
	using System.Threading.Tasks;

	public static class TaskExtensions
	{
		public static Task<Stream> GetRequestStreamAsync(this WebRequest source)
		{
			return Task<Stream>.Factory.FromAsync(source.BeginGetRequestStream, source.EndGetRequestStream, source);
		}

		public static Task<WebResponse> GetResponseAsync(this WebRequest source)
		{
			return Task<WebResponse>.Factory.FromAsync(source.BeginGetResponse, source.EndGetResponse, source);
		}

		public static Task CopyToAsync(this Stream source, Stream destination)
		{
			var tasks = CopyStreamToStreamAsync(source, destination).ToArray();

			var result = Task.Factory.ContinueWhenAll(tasks, action => { });

			return result;
		}

		static IEnumerable<Task> CopyStreamToStreamAsync(Stream source, Stream destination)
		{
			byte[] buffer = new byte[0x2000];
			while (true)
			{
				var read = source.ReadTask(buffer, 0, buffer.Length);
				yield return read;
				if (read.Result == 0) break;
				yield return destination.WriteTask(buffer, 0, read.Result);
			}
		}

		public static Task<int> ReadTask(this Stream stream, byte[] buffer, int offset, int count)
		{
			return Task<int>.Factory.FromAsync(stream.BeginRead, stream.EndRead,
				buffer, offset, count, null, TaskCreationOptions.None);
		}

		public static Task WriteTask(this Stream stream, byte[] buffer, int offset, int count)
		{
			return Task.Factory.FromAsync(stream.BeginWrite, stream.EndWrite,
				buffer, offset, count, null, TaskCreationOptions.None);
		}
	}
}