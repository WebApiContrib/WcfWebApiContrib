using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.ApplicationServer.Http.Dispatcher;

namespace WebApiContrib.OperationHandlers {
    public class CompressionHandler : HttpOperationHandler<HttpResponseMessage, HttpResponseMessage> {
        public CompressionHandler() : base("response") {
        }


        public override HttpResponseMessage OnHandle(HttpResponseMessage input) {

            if (input.RequestMessage.Headers.AcceptEncoding.Contains(new StringWithQualityHeaderValue("gzip"))) {
                input.Content = new CompressedContent(input.Content);
            }
            return input;
        
        }
    }

    public class CompressedContent : HttpContent {
        private readonly HttpContent _Content;
        public CompressedContent(HttpContent content) {
            _Content = content;
            Headers.ContentEncoding.Add("gzip");
            Headers.ContentType = _Content.Headers.ContentType;
        }

        protected override void SerializeToStream(Stream stream, TransportContext context) {
            using (var compressedStream = new GZipStream(stream, CompressionMode.Compress, false)) {
                _Content.CopyTo(compressedStream);
            }
        }

        protected override bool TryComputeLength(out long length) {
            length = -1;
            return false;
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context) {
            throw new NotImplementedException();
        }

    }


}
