using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ApplicationServer.Http.Description;
using OperationHandlers;
using SelfhostedServer.Services;

namespace SelfhostedServer.Host {
    public class OperationHandlerFactory : HttpOperationHandlerFactory {
        protected override System.Collections.ObjectModel.Collection<Microsoft.ApplicationServer.Http.Dispatcher.HttpOperationHandler> OnCreateRequestHandlers(System.ServiceModel.Description.ServiceEndpoint endpoint, HttpOperationDescription operation) {
            
            var collection = base.OnCreateRequestHandlers(endpoint, operation);
            return collection;
        }

        protected override System.Collections.ObjectModel.Collection<Microsoft.ApplicationServer.Http.Dispatcher.HttpOperationHandler> OnCreateResponseHandlers(System.ServiceModel.Description.ServiceEndpoint endpoint, HttpOperationDescription operation) {
            var collection = base.OnCreateResponseHandlers(endpoint, operation);
            collection.Add(new LoggingOperationHandler(new Logger()));
            
            return collection;
        }
    }
}
