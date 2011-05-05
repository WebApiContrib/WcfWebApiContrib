using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.ServiceModel;
using System.Text;
using Microsoft.ApplicationServer.Http.Description;
using Microsoft.Practices.ServiceLocation;

namespace SelfhostedServer.Host {
    public class ResourceFactory : IResourceFactory {
        private readonly IServiceLocator _ServiceLocator;

        public ResourceFactory(IServiceLocator serviceLocator) {
            _ServiceLocator = serviceLocator;
        }

        public object GetInstance(Type serviceType, InstanceContext instanceContext, HttpRequestMessage request) {
            return _ServiceLocator.GetInstance(serviceType);
        }

        public void ReleaseInstance(InstanceContext instanceContext, object service) {
            // 
        }
    }
}
