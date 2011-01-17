using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using Microsoft.Practices.ServiceLocation;
using Microsoft.ServiceModel.Http;

namespace SelfhostedServer.Host {
        public class SelfHostedWebHttpHost : ServiceHost {
            private readonly IServiceLocator _ServiceLocator;

            public SelfHostedWebHttpHost( Type serviceType, params Uri[] baseAddresses) : this(null,serviceType, null, baseAddresses) {
                
            }

            public SelfHostedWebHttpHost(IServiceLocator serviceLocator, Type serviceType, HttpHostConfiguration processorFactory, params Uri[] baseAddresses)
                : base(serviceType, baseAddresses) {
                _ServiceLocator = serviceLocator;

                foreach (Uri baseAddress in baseAddresses) {
                    ConfigureEndpoint(serviceType, baseAddress, processorFactory);
                }
            }

            public SelfHostedWebHttpHost(IServiceLocator serviceLocator, object singletonInstance, HttpHostConfiguration processorFactory, params Uri[] baseAddresses)
                : base(singletonInstance, baseAddresses) {
                _ServiceLocator = serviceLocator;

                foreach (Uri baseAddress in baseAddresses) {
                    ConfigureEndpoint(singletonInstance.GetType(), baseAddress, processorFactory);
                }
            }

            private void ConfigureEndpoint(Type serviceType, Uri baseAddress, HttpHostConfiguration processorFactory) {
                var endpoint = this.AddServiceEndpoint(serviceType, new HttpMessageBinding(), baseAddress);
                endpoint.Behaviors.Add(new DIHttpEndpointBehaviour(_ServiceLocator, processorFactory));
            }
        }
    }
