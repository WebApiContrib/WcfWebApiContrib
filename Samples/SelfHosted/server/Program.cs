using System;
using Microsoft.ServiceModel.Http;
using SelfhostedServer.ProcessorFactories;
using SelfhostedServer.ServiceContracts;

namespace SelfhostedServer {
    class Program {
        static void Main(string[] args) {
            var baseurl = "http://localhost:1000/";
            SelfHostedWebHttpHost host = CreateHost<FooService,RawProcessorFactory>(baseurl);
            host.Open();

            Console.WriteLine("Host open.  Hit enter to exit...");
            Console.WriteLine("Use a web browser and go to " + baseurl + "root or do it right and get fiddler!");

            Console.Read();

            host.Close();

            
        }

        private static SelfHostedWebHttpHost CreateHost<TServiceContract, TProcessorFactory>(string baseUrl) where TProcessorFactory : HostConfiguration ,new() {
            var baseAddresses = new Uri[] { new Uri(baseUrl) };

            var configuration = new TProcessorFactory();

            return new SelfHostedWebHttpHost(typeof(TServiceContract),configuration, baseAddresses);
        }
    }
}
