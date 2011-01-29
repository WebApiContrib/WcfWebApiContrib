namespace HttpContrib.Client
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
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
		public T Content { get; set; }

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

		public virtual HttpQuery<T> Create(T value)
		{
			this.Method = HttpMethod.Post;
			this.Content = value;

			return this;
		}

		public virtual HttpQuery<T> Update(object key, T value)
		{
			this.QueryBuilder.Update(key);
			this.Method = HttpMethod.Put;
			this.Content = value;

			return this;
		}

		public virtual HttpQuery<T> Delete<TValue>(TValue value)
		{
			this.QueryBuilder.Delete(value);

			this.Method = HttpMethod.Delete;

			return this;
		}

		public virtual HttpQuery<T> Where<TProperty, TValue>(Expression<Func<T, TProperty>> property, TValue value)
		{
			return WhereInternal(property, value);
		}

		public virtual HttpQuery<T> Take(int count)
		{
			this.QueryBuilder.Take(count);

			return this;
		}

		public virtual HttpQuery<T> Skip(int count)
		{
			this.QueryBuilder.Skip(count);

			return this;
		}

		private HttpQuery<T> WhereInternal<TProperty>(Expression<Func<T, TProperty>> property, object value)
		{
			this.QueryBuilder.Where(property.GetMemberInfo().Name, value);

			return this;
		}
	}
}