using System.ServiceModel.Description;
using Microsoft.ServiceModel.Http;

namespace Http.Formatters
{
    public abstract class GenericMediaTypeProcessor<T> : MediaTypeProcessor
    {
        protected GenericMediaTypeProcessor(HttpOperationDescription operation, MediaTypeProcessorMode mode)
            : base(operation, mode)
        {
        }

        public abstract void WriteToStream(T instance, System.IO.Stream stream, System.Net.Http.HttpRequestMessage request);
        public override void WriteToStream(object instance, System.IO.Stream stream, System.Net.Http.HttpRequestMessage request)
        {
            WriteToStream((T)instance, stream, request);
        }
    }
}
