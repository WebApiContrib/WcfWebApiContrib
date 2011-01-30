using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;
using Microsoft.ServiceModel.Http;
using Shared;

namespace Formatters
{
    public class PngFormatter : MediaTypeProcessor
    {
        public PngFormatter(HttpParameterDescription parameter, MediaTypeProcessorMode mode)
            :base(parameter, mode)
        {
            
        }

        public override IEnumerable<string> SupportedMediaTypes
        {
            get
            {
                yield return "image/png";
            }
        }

        public override void WriteToStream(object instance, Stream stream)
        {
            var contact = instance as IContact;
            if (contact != null)
            {
                var path = string.Format(@"{0}bin\Images\Image{1}.png", AppDomain.CurrentDomain.BaseDirectory, (contact.ContactID % 3) + 1);
                var fileStream = new FileStream(path, FileMode.Open);
                byte[] bytes = new byte[fileStream.Length];
                fileStream.Read(bytes, 0, (int) fileStream.Length);
                stream.Write(bytes, 0, (int) fileStream.Length);
            }
        }

        public override object ReadFromStream(Stream stream)
        {
            throw new NotImplementedException();
        }
    }

    /*
    public class PngResponseFormatter :  MediaTypeResponseFormatter
    {
        public PngResponseFormatter(IHttpParameterDescription result)
            :base(result)
        {
            
        }

        public override IEnumerable<string> GetContentTypes()
        {
            yield return "image/png";
        }

        public override System.IO.Stream GetStream(object instance)
        {
            var dynamic = (dynamic) instance;
            int id = dynamic.ContactID;
            var path = string.Format(@"{0}bin\Images\Image{1}.png", AppDomain.CurrentDomain.BaseDirectory, (id % 3)+1);
            var stream = new FileStream(path, FileMode.Open);
            return stream;
        }
    }
     * */
}
