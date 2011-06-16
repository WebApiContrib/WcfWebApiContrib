namespace Phone
{
	using System;
	using System.Collections.ObjectModel;
	using System.Diagnostics;
	using System.Linq;
	using System.Windows;
	using HttpContrib;
	using HttpContrib.Client;
	using HttpContrib.Http;
	using Microsoft.Phone.Controls;
	using QueryableSilverlight.Models;

	public partial class MainPage : PhoneApplicationPage
	{
		private ObservableCollection<Person> _people;

		public MainPage()
		{
			InitializeComponent();

			//HttpWebRequest.RegisterPrefix("http://", WebRequestCreator.ClientHttp);
			//HttpWebRequest.RegisterPrefix("https://", WebRequestCreator.ClientHttp);

			_people = new ObservableCollection<Person>();

			this.uxPeople.ItemsSource = this._people;
		}

		private void GetAllPeople( object sender, RoutedEventArgs e )
		{
			_people.Clear();

			SimpleHttpClient client = new SimpleHttpClient( "http://localhost:1182/people" );

			IHttpQueryProvider queryProvider = new HttpQueryProvider( client );

			var query = new HttpQuery<Person>( queryProvider );
			// query.ToString() == http://localhost:1182/people

			//var query = new HttpQuery<Person>(new HttpQueryProvider(new SimpleHttpClient("http://localhost:1182")), /* resource name*/ "people");
			//// query.ToString() == http://localhost:1182/people

			//var query = new HttpQuery<Person>(queryProvider).Skip(5).Take(10);
			//// query.ToString() == http://localhost:1182/people?$skip=5$top=10

			//int id = 1;
			//var query = new HttpQuery<Person>(queryProvider).Where(c => c.ID, id);
			//// query.ToString() == http://localhost:1182/people?$filter=ID eq 1

			//var query = new HttpQuery<Person>(null, /* resource name*/ "people");
			//// query.ToString() == people

			//var query = new HttpQuery<Person>(null, /* resource name*/ "people").Take(10);
			//// query.ToString() == people?$top=10

			uxQueryText.Text = query.GetFullyQualifiedQuery( client ).ToString();

			HandleQuery( query );
		}

		private void GetPersonWithID( object sender, RoutedEventArgs e )
		{
			int id;

			if ( Int32.TryParse( this.uxPersonID.Text, out id ) )
			{
				_people.Clear();

				SimpleHttpClient client = new SimpleHttpClient( "http://localhost:1182/people" );

				var query = client.CreateQuery<Person>().Where( c => c.ID, id );

				uxQueryText.Text = query.GetFullyQualifiedQuery( client ).ToString();

				this.uxPersonID.Text = String.Empty;

				HandleQuery( query );
			}
		}

		private void GetTop3People( object sender, RoutedEventArgs e )
		{
			_people.Clear();

			SimpleHttpClient client = new SimpleHttpClient( "http://localhost:1182/people" );

			var query = client.CreateQuery<Person>().Take( 3 );

			uxQueryText.Text = query.GetFullyQualifiedQuery( client ).ToString();

			HandleQuery( query );
		}

		private void Get3rdPerson( object sender, RoutedEventArgs e )
		{
			_people.Clear();

			SimpleHttpClient client = new SimpleHttpClient( "http://localhost:1182/" );

			var query = client.CreateQuery<Person>( "people" ).Skip( 2 ).Take( 1 );

			uxQueryText.Text = query.GetFullyQualifiedQuery( client ).ToString();

			HandleQuery( query );
		}

		private void CreatePerson( object sender, RoutedEventArgs e )
		{
			Uri uri = new Uri( "http://localhost:1182/people" );

			SimpleHttpClient client = new SimpleHttpClient( uri.ToString() );

			var person = new Person { ID = 0, Name = personName.Text };

			var query = client.CreateQuery<Person>();
			query.Create( person );

			var task = query.ExecuteSingleAsync();

			task.ContinueWith( t =>
			{
				Execute.OnUIThread( () =>
				{
					if ( !t.IsFaulted && t.IsCompleted && t.Result != null )
					{
						Debug.WriteLine( "Person: {0}", t.Result );

						_people.Add( t.Result );
					}
				} );
			} );
		}

		private void DeletePerson( object sender, RoutedEventArgs e )
		{
			Person person = this.uxPeople.SelectedItem as Person;

			if ( person != null )
			{
				SimpleHttpClient client = new SimpleHttpClient( "http://localhost:1182/people" );

				var query = client.CreateQuery<Person>().Delete( person.ID );

				uxQueryText.Text = query.GetFullyQualifiedQuery( client ).ToString();

				var task = query.ExecuteSingleAsync();
				task.ContinueWith( t =>
				{
					Execute.OnUIThread( () =>
					{
						if ( !t.IsFaulted && t.IsCompleted && t.Result != null )
						{
							Debug.WriteLine( "Person: {0}", t.Result );

							_people.Remove( _people.First( p => p.ID == t.Result.ID ) );
						}
					} );
				} );
			}
		}

		private void UpdatePerson( object sender, RoutedEventArgs e )
		{
			Person person = this.uxPeople.SelectedItem as Person;

			if ( person != null )
			{
				person.Name = DateTime.Now.ToString();

				SimpleHttpClient client = new SimpleHttpClient( "http://localhost:1182/people" );

				var query = client.CreateQuery<Person>().Update( person.ID, person );

				uxQueryText.Text = query.GetFullyQualifiedQuery( client ).ToString();

				var task = query.ExecuteSingleAsync();
				task.ContinueWith( t =>
				{
					Execute.OnUIThread( () =>
					{
						if ( !t.IsFaulted && t.IsCompleted && t.Result != null )
						{
							Debug.WriteLine( "Person: {0}", t.Result );
						}
					} );
				} );
			}
		}

		private void HandleQuery( HttpQuery<Person> query )
		{
			var task = query.ExecuteAsync();
			task.ContinueWith( t =>
			{
				Execute.OnUIThread( () =>
				{
					if ( !t.IsFaulted && t.IsCompleted && t.Result != null )
					{
						t.Result.Apply( p => { Debug.WriteLine( "Person: {0}", p ); } );

						t.Result.Apply( _people.Add );
					}
				} );
			} );
		}
	}
}