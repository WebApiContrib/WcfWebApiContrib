namespace QueryableSilverlight
{
	using System;
	using System.Collections.ObjectModel;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Net;
	using System.Net.Browser;
	using System.Windows;
	using System.Windows.Controls;
	using System.Xml.Serialization;
	using HttpContrib;
	using HttpContrib.Client;
	using HttpContrib.Http;
	using QueryableSilverlight.Models;

	public partial class MainPage : UserControl
	{
		private ObservableCollection<Person> _people;

		public MainPage()
		{
			InitializeComponent();

			HttpWebRequest.RegisterPrefix("http://", WebRequestCreator.ClientHttp);
			HttpWebRequest.RegisterPrefix("https://", WebRequestCreator.ClientHttp);

			_people = new ObservableCollection<Person>();

			this.uxPeople.ItemsSource = this._people;
		}

		private void GetAllPeople(object sender, RoutedEventArgs e)
		{
			_people.Clear();

			SimpleHttpClient client = new SimpleHttpClient("http://localhost:1182/people");
			client.Accept = MediaType.Xml;

			var query = client.CreateQuery<Person>();

			uxQueryText.Text = query.GetFullyQualifiedQuery(client);

			HandleQuery(query);
		}

		private void GetPersonWithID(object sender, RoutedEventArgs e)
		{
			int id;

			if (Int32.TryParse(this.uxPersonID.Text, out id))
			{
				_people.Clear();

				SimpleHttpClient client = new SimpleHttpClient("http://localhost:1182/people");

				var query = client.CreateQuery<Person>().Where(c => c.ID, id);

				uxQueryText.Text = query.GetFullyQualifiedQuery(client);

				HandleQuery(query);
			}
		}

		private void GetTop3People(object sender, RoutedEventArgs e)
		{
			_people.Clear();

			SimpleHttpClient client = new SimpleHttpClient("http://localhost:1182/people");

			var query = client.CreateQuery<Person>().Take(3);

			uxQueryText.Text = query.GetFullyQualifiedQuery(client);

			HandleQuery(query);
		}

		private void Get3rdPerson(object sender, RoutedEventArgs e)
		{
			_people.Clear();

			SimpleHttpClient client = new SimpleHttpClient("http://localhost:1182/people");

			var query = client.CreateQuery<Person>().Skip(2).Take(1);

			uxQueryText.Text = query.GetFullyQualifiedQuery(client);

			HandleQuery(query);
		}

		private void CreatePerson(object sender, RoutedEventArgs e)
		{
			Uri uri = new Uri("http://localhost:1182/people");

			SimpleHttpClient client = new SimpleHttpClient(uri.ToString());

			var contact = new Person { ID = 5, Name = personName.Text };

			var stream = contact.WriteObjectAsXml();

			var request = new HttpRequestMessage(HttpMethod.Post);
			request.Accept = MediaType.Xml;
			request.ContentType = MediaType.Xml;
			request.RequestUri = uri;
			request.Content = stream;

			var task = client.SendAsync(request);
			task.ContinueWith(t =>
			{
				Execute.OnUIThread(() =>
				{
					var person = t.Result.ReadXmlAsObject<Person>();

					if (person != null)
					{
						Debug.WriteLine("Person: {0}", person);

						_people.Add(person);
					}
				});
			});
		}

		private void HandleQuery(HttpQuery<Person> query)
		{
			var task = query.ExecuteAsync();
			task.ContinueWith(t =>
			{
				Execute.OnUIThread(() =>
				{
					if (!t.IsFaulted && t.IsCompleted && t.Result != null)
					{
						t.Result.Apply(p => { Debug.WriteLine("Person: {0}", p); });

						t.Result.Apply(_people.Add);
					}
				});
			});
		}
	}
}