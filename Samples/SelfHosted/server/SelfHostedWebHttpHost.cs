using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using Microsoft.ServiceModel.Description;
using Microsoft.ServiceModel.Http;

namespace SelfhostedServer {
        public class SelfHostedWebHttpHost : ServiceHost {

            public SelfHostedWebHttpHost(Type serviceType, params Uri[] baseAddresses) : this(serviceType, null, baseAddresses) {
            }

            public SelfHostedWebHttpHost(Type serviceType, HostConfiguration processorFactory, params Uri[] baseAddresses) : base(serviceType, baseAddresses) {

                var contract = ContractDescription.GetContract(serviceType);

                foreach (Uri baseAddress in baseAddresses) {
                    ConfigureEndpoint(contract, baseAddress, processorFactory);
                }
            }

            private void ConfigureEndpoint(ContractDescription contract, Uri baseAddress, HostConfiguration processorFactory) {
                var endpoint = new ServiceEndpoint(contract, new HttpMessageBinding(), new EndpointAddress(baseAddress));
                endpoint.Behaviors.Add(new HttpEndpointBehavior(processorFactory));
                AddServiceEndpoint(endpoint);
            }
        }
    }
