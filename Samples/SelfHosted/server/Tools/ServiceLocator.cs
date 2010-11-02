using System;
using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace SelfhostedServer.Tools {
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