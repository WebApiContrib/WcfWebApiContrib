namespace QueryableSilverlight.Web
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.ServiceModel;
	using System.ServiceModel.Activation;
	using System.ServiceModel.Web;
	using QueryableSilverlight.Models;

	[ServiceContract]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
	public class PeopleResource
	{
		private static int _totalPeople = 1;
		private static List<Person> _people = new List<Person>()
        {
            new Person { ID = _totalPeople++, Name = "First"},
            new Person { ID = _totalPeople++, Name = "Second" },
            new Person { ID = _totalPeople++, Name = "Third"},
            new Person { ID = _totalPeople++, Name = "Fourth" },
        };

		[WebGet(UriTemplate = "")]
		public IQueryable<Person> Get()
		{
			return _people.AsQueryable();
		}

		[WebInvoke(UriTemplate = "", Method = "POST")]
		public Person Post(Person person)
		{
			if (person == null)
			{
				throw new ArgumentNullException("person");
			}

			person.ID = _totalPeople++;
			_people.Add(person);
			return person;
		}

		[WebInvoke(UriTemplate = "{id}", Method = "PUT")]
		public Person Put(int id, Person person)
		{
			Person localPerson = _people.First(p => p.ID == id);

			localPerson.Name = person.Name;

			return localPerson;
		}

		[WebInvoke(UriTemplate = "{id}", Method = "DELETE")]
		public Person Delete(string id)
		{
			var intId = int.Parse(id, CultureInfo.InvariantCulture);
			Person deleted = _people.First(p => p.ID == intId);

			_people.Remove(deleted);

			return deleted;
		}
	}
}