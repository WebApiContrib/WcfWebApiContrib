// <copyright>
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>

namespace ContactManager
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Net.Http;
    using System.ServiceModel.Description;
    using Microsoft.ServiceModel.Http;

    public class PngProcessor : MediaTypeProcessor
    {
        public PngProcessor(HttpOperationDescription operation, MediaTypeProcessorMode mode)
            : base(operation, mode)
        {
        }

        public override IEnumerable<string> SupportedMediaTypes
        {
            get
            {
                yield return "image/png";
            }
        }

        public override void WriteToStream(object instance, Stream stream, HttpRequestMessage request)
        {
            var contact = instance as Contact;
            if (contact != null)
            {
                var path = string.Format(CultureInfo.InvariantCulture, @"{0}bin\Images\Image{1}.png", AppDomain.CurrentDomain.BaseDirectory, (contact.ContactId % 3) + 1);
                using (var fileStream = new FileStream(path, FileMode.Open))
                {
                    byte[] bytes = new byte[fileStream.Length];
                    fileStream.Read(bytes, 0, (int)fileStream.Length);
                    stream.Write(bytes, 0, (int)fileStream.Length);
                }
            }
        }

        public override object ReadFromStream(Stream stream, HttpRequestMessage request)
        {
            throw new NotImplementedException();
        }
    }
}
