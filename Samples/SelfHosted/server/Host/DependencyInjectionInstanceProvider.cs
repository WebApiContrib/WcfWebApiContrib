using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using Microsoft.Practices.ServiceLocation;

namespace SelfhostedServer.Host {
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