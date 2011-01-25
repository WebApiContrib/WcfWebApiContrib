namespace HttpContrib.Client
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using HttpContrib.Http;

	public class HttpQuery<T>
	{
		private readonly string _resourceName;
		private readonly IHttpQueryProvider _provider;

		public HttpQuery(IHttpQueryProvider provider)
			: this(provider, String.Empty)
		{
		}

		public HttpQuery(IHttpQueryProvider provider, string resourceName)
		{
			this.Method = HttpMethod.Get;
			this.QueryBuilder = new QueryBuilder(resourceName);

			_provider = provider;
			_resourceName = resourceName;
		}

		public string Method { get; set; }

		public string Query
		{
			get
			{
				return this.QueryBuilder.BuildQuery();
			}
		}

		public string Path
		{
			get
			{
				return this.QueryBuilder.BuildPath();
			}
		}

        internal QueryBuilder QueryBuilder { get; private set; }

		public Task<T> ExecuteSingleAsync()
		{
			return this._provider.ExecuteSingleAsync<T>(this);
		}

		public Task<IEnumerable<T>> ExecuteAsync()
		{
			return this._provider.ExecuteAsync<T>(this);
		}

		public override string ToString()
		{
			return this.QueryBuilder.Build();
		}
	}
}