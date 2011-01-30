// <copyright>
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>

namespace ContactManager
{
    using System;
    using System.ComponentModel.Composition.Hosting;
    using System.ServiceModel.Activation;
    using System.ServiceModel.Web;
    using System.Web.Routing;
    using Microsoft.ServiceModel.Http;

    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // use MEF for providing instances
            var catalog = new AssemblyCatalog(typeof(Global).Assembly);
            var container = new CompositionContainer(catalog);
            var configuration = new ContactManagerConfiguration(container);

            RouteTable.Routes.AddServiceRoute<ContactResource>("contact", configuration);
            RouteTable.Routes.AddServiceRoute<ContactsResource>("contacts", configuration);
        }
    }
}