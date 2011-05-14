using System.Runtime.Serialization;

namespace DataContractExample
{
    [DataContract]
    public class Contact
    {
        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }
    }
}