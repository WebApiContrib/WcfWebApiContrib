// <copyright>
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>

namespace ContactManager
{
    using System.Collections.Generic;

    public interface IContactRepository
    {
        void Update(Contact updatedContact);

        Contact Get(int id);

        List<Contact> GetAll();

        void Post(Contact contact);

        void Delete(int id);
    }
}