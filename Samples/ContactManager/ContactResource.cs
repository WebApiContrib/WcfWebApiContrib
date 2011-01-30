// <copyright>
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>

namespace ContactManager
{
    using System.ComponentModel.Composition;
    using System.Globalization;
    using System.Net;
    using System.Net.Http;
    using System.ServiceModel;
    using System.ServiceModel.Web;

    [ServiceContract]
    [Export]
    public class ContactResource
    {
        private readonly IContactRepository repository;

        [ImportingConstructor]
        public ContactResource(IContactRepository repository)
        {
            this.repository = repository;
        }

        [WebGet(UriTemplate = "{id}")]
        public Contact Get(string id, HttpResponseMessage response)
        {
            var contact = this.repository.Get(int.Parse(id, CultureInfo.InvariantCulture));
            if (contact == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.Content = new StringContent("Contact not found");
            }

            return contact;
        }

        [WebInvoke(UriTemplate = "{id}", Method = "PUT")]
        public Contact Put(string id, Contact contact, HttpResponseMessage response)
        {
            this.Get(id, response);
            this.repository.Update(contact);
            return contact;
        }

        [WebInvoke(UriTemplate = "{id}", Method = "DELETE")]
        public Contact Delete(string id)
        {
            var intId = int.Parse(id, CultureInfo.InvariantCulture);
            dynamic deleted = this.repository.Get(intId);
            this.repository.Delete(intId);
            return deleted;
        }
    }
}
