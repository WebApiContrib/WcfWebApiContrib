// <copyright>
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>

namespace ContactManager
{
    using System.Data;
    using System.Globalization;

    public class Contact
    {
        public int ContactId { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }
        
        public string City { get; set; }
        
        public string State { get; set; }
        
        public string Zip { get; set; }
        
        public string Email { get; set; }
        
        public string Twitter { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "value", Justification = "Json Serializer doesn't support read only")]
        public string Self
        {
            get { return string.Format(CultureInfo.CurrentCulture, "contact/{0}", this.ContactId); }
            set { }
        }
    }
}