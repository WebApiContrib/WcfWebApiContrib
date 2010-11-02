using System;
using Microsoft.ServiceModel.Http;
using SelfhostedServer.ProcessorFactories;
using TavisSample;

namespace SelfhostedServer {
    class Program {
        static void Main(string[] args) {


            SelfHostedWebHttpHost host = CreateHost<FooService,DefaultProcessorFactory>("http://localhost:1000/");
            host.Open();

            Console.WriteLine("Host closed.  Hit any key to exit...");
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
