using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using Microsoft.Practices.ServiceLocation;
using Microsoft.ServiceModel.Description;
using Microsoft.ServiceModel.Http;

namespace SelfhostedServer {
    public class DIHttpEndpointBehaviour : HttpEndpointBehavior {
        private readonly IServiceLocator _ServiceLocator;

        public DIHttpEndpointBehaviour(IServiceLocator serviceLocator, HostConfiguration configuration) : base(configuration) {
            _ServiceLocator = serviceLocator;
        }

        protected override IInstanceProvider OnGetInstanceProvider(System.ServiceModel.Description.ServiceEndpoint endpoint, DispatchRuntime runtime) {
            return new DependencyInjectionInstanceProvider(_ServiceLocator, endpoint.Contract.ContractType);
        }
    }



    public class DependencyInjectionInstanceProvider : IInstanceProvider {
        private readonly IServiceLocator _ServiceLocator;
        private Type _serviceType;

        public DependencyInjectionInstanceProvider(IServiceLocator serviceLocator, Type serviceType) {
            _ServiceLocator = serviceLocator;
            _serviceType = serviceType;
        }

        public object GetInstance(InstanceContext instanceContext) {
            return GetInstance(instanceContext, null);
        }

        public object GetInstance(InstanceContext instanceContext, Message message) {
            return _ServiceLocator.GetInstance(_serviceType);
        }

        public void ReleaseInstance(System.ServiceModel.InstanceContext instanceContext, object instance) { }
    }
}
