namespace HttpContrib.Client
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;

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
			this.QueryBuilder = new QueryBuilder(resourceName);

			_provider = provider;
			_resourceName = resourceName;
		}

		internal QueryBuilder QueryBuilder { get; private set; }

		public Task<IEnumerable<T>> ExecuteAsync()
		{
			return this._provider.ExecuteAsync<T>(this.QueryBuilder.Build());
		}

		public override string ToString()
		{
			return this.QueryBuilder.Build();
		}
	}
}