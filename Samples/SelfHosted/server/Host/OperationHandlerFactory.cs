using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ApplicationServer.Http;
using Microsoft.ApplicationServer.Http.Description;
using Microsoft.ApplicationServer.Http.Dispatcher;
using OperationHandlers;
using SelfhostedServer.Services;
using WebApiContrib.OperationHandlers;

namespace SelfhostedServer.Host {
    public class OperationHandlerFactory : HttpOperationHandlerFactory {
        private ServerState _ServerState;
        public OperationHandlerFactory(ServerState serverState) {
            _ServerState = serverState;
        }
        protected override System.Collections.ObjectModel.Collection<Microsoft.ApplicationServer.Http.Dispatcher.HttpOperationHandler> OnCreateRequestHandlers(System.ServiceModel.Description.ServiceEndpoint endpoint, HttpOperationDescription operation) {
            
            var collection = base.OnCreateRequestHandlers(endpoint, operation);
            
            collection.Add(new ServerStateOperationHandler(_ServerState));
            return collection;
        }

        protected override System.Collections.ObjectModel.Collection<Microsoft.ApplicationServer.Http.Dispatcher.HttpOperationHandler> OnCreateResponseHandlers(System.ServiceModel.Description.ServiceEndpoint endpoint, HttpOperationDescription operation) {
            var collection = base.OnCreateResponseHandlers(endpoint, operation);
            collection.Add(new LoggingOperationHandler(new Logger()));
            collection.Add(new CompressionHandler());
            
            return collection;
        }
    }

    public class ServerState : ConcurrentDictionary<string, object> {
        
    }

    public class ServerStateOperationHandler : HttpOperationHandler {
        private readonly ServerState _State;

        public ServerStateOperationHandler(ServerState state) {
            _State = state;
        }

        protected override IEnumerable<HttpParameter> OnGetInputParameters() {
            return null;
        }

        protected override IEnumerable<HttpParameter> OnGetOutputParameters() {
            yield return new HttpParameter("serverState", typeof(ServerState));
        }

        protected override object[] OnHandle(object[] input) {
            return new object[] {_State};
        }
    }
}
