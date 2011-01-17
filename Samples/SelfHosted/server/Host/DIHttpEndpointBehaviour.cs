using System.ServiceModel.Dispatcher;
using Microsoft.Practices.ServiceLocation;
using Microsoft.ServiceModel.Description;
using Microsoft.ServiceModel.Http;

namespace SelfhostedServer.Host {
    public class DIHttpEndpointBehaviour : HttpEndpointBehavior {
        private readonly IServiceLocator _ServiceLocator;

        public DIHttpEndpointBehaviour(IServiceLocator serviceLocator, HttpHostConfiguration configuration) : base(configuration) {
            _ServiceLocator = serviceLocator;
        }

        protected override IInstanceProvider OnGetInstanceProvider(System.ServiceModel.Description.ServiceEndpoint endpoint, DispatchRuntime runtime) {
            return new DependencyInjectionInstanceProvider(_ServiceLocator, endpoint.Contract.ContractType);
        }
    }
}
