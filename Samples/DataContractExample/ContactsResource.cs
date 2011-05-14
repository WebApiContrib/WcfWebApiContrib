using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace DataContractExample
{
    [ServiceContract]
    public class ContactsResource
    {
        static List<Contact> contacts;

        static ContactsResource()
        {
            contacts = new List<Contact>
                {
                    new Contact {FirstName = "Glenn", LastName = "Block"},
                    new Contact { FirstName = "Jeff", LastName = "Handley" }
                };

        }

        [WebGet(UriTemplate="contacts")]
        public Contact[] Get()
        {
            return contacts.ToArray();
        }

        [WebInvoke(UriTemplate="contacts", Method="POST")]
        public void Post(Contact contact)
        {
            contacts.Add(contact);
        }
    }
}