using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

using Microsoft.ApplicationServer.Http;
using Microsoft.ApplicationServer.Http.Activation;
using Microsoft.ApplicationServer.Http.Description;
using WebApiContrib.Formatters.Core;

namespace DataContractExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new HttpConfiguration();
            config.Formatters.UseDataContractSerializer<Contact>();

            var host = new HttpServiceHost(typeof(ContactsResource), config, new Uri("http://localhost:8080/"));
            host.Open();
            
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:8080/");
            var newContact = new Contact {FirstName = "Brad", LastName = "Abrams"};
            var content = new ObjectContent<Contact>(newContact,"application/xml").UseDataContractSerializer();
            var response = client.Post("contacts", content);
            response = client.Get("contacts");
            
            var contacts = response.Content.ReadAsDataContract<Contact[]>();
            foreach (var contact in contacts)
            {
                Console.WriteLine(string.Format("Contact: FirstName={0}, LastName={1}", contact.FirstName, contact.LastName));
            }
            Console.ReadLine();
        }
    }
}
