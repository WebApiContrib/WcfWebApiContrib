using System.IO;

namespace Formatters
{
    /// <summary>
    /// Prevents consuming types (TextReader/TextWriter) from closing the Stream.
    /// </summary>
    /// <see href="http://ydie22.blogspot.com/2008/02/about-idisposable-close-streams-and.html"/>
    public class NonClosingStreamDecorator : Stream
    {
        private readonly Stream _innerStream;

        public NonClosingStreamDecorator(Stream stream)
        {
            _innerStream = stream;
        }

        public override void Close()
        {
            // do not delegate !!
            //_innerStream.Close();
        }

        protected override void Dispose(bool disposing)
        {
            // do not delegate !!
            //_innerStream.Dispose();
        }

        public override bool CanRead
        {
            get { return _innerStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return _innerStream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return _innerStream.CanWrite; }
        }

        public override void Flush()
        {
            _innerStream.Flush();
        }

        public override long Length
        {
            get { return _innerStream.Length; }
        }

        public override long Position
        {
            get { return _innerStream.Position; }
            set { _innerStream.Position = value; }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _innerStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _innerStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _innerStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _innerStream.Write(buffer, offset, count);
        }
    }

    public static class StreamExtensions
    {
        public static Stream PreventClose(this Stream stream)
        {
            return new NonClosingStreamDecorator(stream);
        }
    }
}
