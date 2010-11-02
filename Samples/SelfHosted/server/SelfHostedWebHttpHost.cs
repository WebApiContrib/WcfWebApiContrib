using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using Microsoft.Practices.ServiceLocation;
using Microsoft.ServiceModel.Description;
using Microsoft.ServiceModel.Http;

namespace SelfhostedServer {
        public class SelfHostedWebHttpHost : ServiceHost {
            private readonly IServiceLocator _ServiceLocator;

            public SelfHostedWebHttpHost( Type serviceType, params Uri[] baseAddresses) : this(null,serviceType, null, baseAddresses) {
                
            }

            public SelfHostedWebHttpHost(IServiceLocator serviceLocator, Type serviceType, HostConfiguration processorFactory, params Uri[] baseAddresses)
                : base(serviceType, baseAddresses) {
                _ServiceLocator = serviceLocator;

                var contract = ContractDescription.GetContract(serviceType);

                foreach (Uri baseAddress in baseAddresses) {
                    ConfigureEndpoint(contract, baseAddress, processorFactory);
                }
            }

            private void ConfigureEndpoint(ContractDescription contract, Uri baseAddress, HostConfiguration processorFactory) {
                var endpoint = new ServiceEndpoint(contract, new HttpMessageBinding(), new EndpointAddress(baseAddress));
                endpoint.Behaviors.Add(new DIHttpEndpointBehaviour(_ServiceLocator, processorFactory));
                AddServiceEndpoint(endpoint);
            }
        }
    }
