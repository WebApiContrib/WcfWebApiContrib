using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.ServiceModel.Http;
using SelfhostedServer.ProcessorFactories;
using SelfhostedServer.ServiceContracts;
using SelfhostedServer.Services;

namespace SelfhostedServer {
    class Program {
        static void Main(string[] args) {
            

            var serviceLocator = new ServiceLocator(CreateDIContainer());

            var baseurl = "http://localhost:1000/";
            SelfHostedWebHttpHost host = CreateHost<FooService, RawProcessorFactory>(serviceLocator, baseurl);
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
    }


    public class ServiceLocator : ServiceLocatorImplBase {
        private readonly IUnityContainer _Container;

        public ServiceLocator(IUnityContainer container) {
            _Container = container;
        }

        protected override object DoGetInstance(Type serviceType, string key) {
            return _Container.Resolve(serviceType, key);
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType) {
            return _Container.ResolveAll(serviceType);
        }
    }
}
