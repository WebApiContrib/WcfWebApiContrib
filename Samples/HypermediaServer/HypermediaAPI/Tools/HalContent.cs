using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Hal;

namespace HypermediaAPI.Tools {
    public class HalContent : StreamContent {
        public HalContent(HalDocument halDocument) : base(halDocument.ToStream()) {
            Headers.ContentType = new MediaTypeHeaderValue("application/vnd.hal+xml");
            
        }
    }
}
