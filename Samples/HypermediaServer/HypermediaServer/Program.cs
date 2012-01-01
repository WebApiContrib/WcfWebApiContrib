using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ApplicationServer.Http;
using OperationHandlers;

namespace HypermediaServer {
    class Program {
        static void Main(string[] args) {

            var baseurl = new Uri("http://localhost:1000/");


            var config = new HttpConfiguration();
            //config.ResponseHandlers = (handlers, se, od) => {
            //    handlers.Add(new LoggingOperationHandler(new Logger()));
            //    handlers.Add(new CompressionHandler());
            //};

            var hosts = new List<HttpServiceHost>();
            HypermediaAPI.RootController.CreateHosts(hosts, config, baseurl);
            hosts.ForEach(h => h.Open());

            Console.WriteLine("Host open.  Hit enter to exit...");
            Console.WriteLine("Use a web browser and go to " + baseurl + " or do it right and get fiddler!");

            Console.Read();

            hosts.ForEach(h => h.Close());

        }
    }
}
