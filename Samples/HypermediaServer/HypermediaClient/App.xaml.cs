using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using RESTAgent;
using RESTAgent.Interfaces;

namespace HypermediaClient {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        protected override void OnStartup(StartupEventArgs e) {
            var restagent = new RESTAgent.RestAgent();
            restagent.RegisterSemanticsProvider(new RESTAgent.Hal.HalSemanticsProvider());
            restagent.RegisterDefaultResponseCompleteHandler(OnComplete);
            restagent.RegisterDefaultErrorHandler(OnError);
            restagent.NavigateTo(new Link("http://localtmserver:1000/"),null,OnComplete);
        }

        private void OnError(Exception ex) {

        }

        private void OnComplete(IHypermediaContent content) {

        }
    }
}
