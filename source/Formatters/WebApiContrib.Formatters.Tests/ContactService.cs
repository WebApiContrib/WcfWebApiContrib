using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace WebApiContrib.Formatters.Tests {
    [ServiceContract]
    public class ContactService {

        private static List<Contact> _Contacts = new List<Contact>();


        [WebGet(UriTemplate = "contacts")]
        public Contact[] Get() {
            return _Contacts.ToArray();
        }


        [WebInvoke(UriTemplate = "contacts", Method = "POST")]
        public void Post(Contact contact) {
            _Contacts.Add(contact);
            
        }
    }
}
