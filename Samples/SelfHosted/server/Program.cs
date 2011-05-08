using System;
using System.Collections;
using Microsoft.ApplicationServer.Http;
using Microsoft.ApplicationServer.Http.Activation;
using Microsoft.ApplicationServer.Http.Description;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using OperationHandlers;
using SelfhostedServer.Host;
using SelfhostedServer.ServiceContracts;
using SelfhostedServer.Services;
using ServiceLocator = SelfhostedServer.Tools.ServiceLocator;

namespace SelfhostedServer {
    class Program {
        static void Main(string[] args) {

            var serviceLocator = new ServiceLocator(CreateDIContainer());

            var baseurl = new Uri("http://localhost:1000/");

            var config = HttpHostConfiguration.Create()
                .SetResourceFactory(new ResourceFactory(serviceLocator))
                .SetOperationHandlerFactory(new OperationHandlerFactory());

            HttpServiceHost host = new HttpConfigurableServiceHost<FooService>(config, baseurl);
            host.Open();

            Console.WriteLine("Host open.  Hit enter to exit...");
            Console.WriteLine("Use a web browser and go to " + baseurl + " or do it right and get fiddler!");

            Console.Read();

            host.Close();
        }



        /// <summary>
        /// This was a test to see how painful it is to create a large number of service hosts.
        /// </summary>
        /// <param name="args"></param>
        public static void Main2(string[] args) {
            var serviceLocator = new ServiceLocator(CreateDIContainer());

            for (int i = 0; i < 1000; i++) {
                
            
            var baseurl = new Uri("http://localhost:1000/service" + i);

            var config = HttpHostConfiguration.Create()
                .SetResourceFactory(new ResourceFactory(serviceLocator))
                .SetOperationHandlerFactory(new OperationHandlerFactory());

            HttpServiceHost host = new HttpConfigurableServiceHost<FooService>(config, baseurl);
            host.Open();
            Console.WriteLine("Opening host open " + baseurl);
            }

            Console.WriteLine("Host open.  Hit enter to exit...");
           // Console.WriteLine("Use a web browser and go to " + baseurl + " or do it right and get fiddler!");

            Console.Read();

         //   host.Close();
        }

        private static UnityContainer CreateDIContainer() {
            var unityContainer = new UnityContainer();
//            unityContainer.RegisterType<FooService, FooService>();
            unityContainer.RegisterType<ILogger, Logger>();
            return unityContainer;
        }

    }
}
