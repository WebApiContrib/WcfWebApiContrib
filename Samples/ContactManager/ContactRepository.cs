// <copyright>
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
namespace ContactManager
{
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;

    [Export(typeof(IContactRepository))]
    public class ContactRepository : IContactRepository
    {
        private static IList<Contact> contacts;

        private static int nextContactID;

        static ContactRepository()
        {
            contacts = new List<Contact>();
            contacts.Add(new Contact { ContactId = 1, Name = "Glenn Block", Address = "1 Microsoft Way", City = "Redmond", State = "Washington", Zip = "98112", Email = "gblock@microsoft.com", Twitter = "gblock" });
            contacts.Add(new Contact { ContactId = 2, Name = "Yavor Georgiev", Address = "1 Microsoft Way", City = "Redmond", State = "Washington", Zip = "98112", Email = "yavorg@microsoft.com", Twitter = "digthepony" });
            contacts.Add(new Contact { ContactId = 3, Name = "Jeff Handley", Address = "1 Microsoft Way", City = "Redmond", State = "Washington", Zip = "98112", Email = "jeff.handley@microsoft.com", Twitter = "jeffhandley" });
            nextContactID = contacts.Count + 1;
        }

        public void Update(Contact updatedContact)
        {
            var contact = this.Get(updatedContact.ContactId);
            contact.Name = updatedContact.Name;
            contact.Address = updatedContact.Address;
            contact.City = updatedContact.City;
            contact.State = updatedContact.State;
            contact.Zip = updatedContact.Zip;
            contact.Email = updatedContact.Email;
            contact.Twitter = updatedContact.Twitter;
        }

        public Contact Get(int id)
        {
            return contacts.SingleOrDefault(c => c.ContactId == id);
        }

        public List<Contact> GetAll()
        {
            return contacts.ToList();
        }

        public void Post(Contact contact)
        {
            contact.ContactId = nextContactID++;
            contacts.Add(contact);
        }

        public void Delete(int id)
        {
            var contact = this.Get(id);
            contacts.Remove(contact);
        }
    }
}