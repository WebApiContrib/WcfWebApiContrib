using System;
using System.Collections;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.ServiceModel.Http;
using SelfhostedServer.Host;
using SelfhostedServer.ProcessorFactories;
using SelfhostedServer.ServiceContracts;
using SelfhostedServer.Services;
using ServiceLocator = SelfhostedServer.Tools.ServiceLocator;
using Microsoft.Http;
using System.Net;

namespace SelfhostedServer {
    class Program {
        static void Main(string[] args) {
            var serviceLocator = new ServiceLocator(CreateDIContainer());

            var baseurl = "http://localhost:1000/";
            SelfHostedWebHttpHost host = CreateHost<FooService, DefaultProcessorFactory>(serviceLocator, baseurl);
            //SelfHostedWebHttpHost host = CreateSingletonHost(serviceLocator, baseurl);
            host.Open();

            Console.WriteLine("Host open.  Hit enter to exit...");
            Console.WriteLine("Use a web browser and go to " + baseurl + "root or do it right and get fiddler!");

            Console.Read();

            host.Close();
        }

        private static UnityContainer CreateDIContainer() {
            var unityContainer = new UnityContainer();
            unityContainer.RegisterType<FooService, FooService>();
            unityContainer.RegisterType<ILogger, Logger>();
            return unityContainer;
        }

        private static SelfHostedWebHttpHost CreateHost<TServiceContract, TProcessorFactory>(IServiceLocator container, string baseUrl) where TProcessorFactory : HostConfiguration ,new() {
            var baseAddresses = new Uri[] { new Uri(baseUrl) };

            var configuration = new TProcessorFactory();

            return new SelfHostedWebHttpHost(container, typeof(TServiceContract), configuration, baseAddresses);
        }

        private static SelfHostedWebHttpHost CreateSingletonHost(IServiceLocator container, string baseUrl)
        {
            object singletonInstance = new SingletonService(request => {
                return new HttpResponseMessage {
                    Content = HttpContent.Create("Hello World", "text/plain"),
                    StatusCode = HttpStatusCode.OK,
                };
            });
            HostConfiguration configuration = new DefaultProcessorFactory();
            return new SelfHostedWebHttpHost(container, singletonInstance, configuration, new Uri(baseUrl));
        }
    }
}
