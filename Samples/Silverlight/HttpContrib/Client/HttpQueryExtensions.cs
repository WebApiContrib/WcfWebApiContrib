namespace HttpContrib.Client
{
	using System;
	using System.Linq.Expressions;
	using System.Reflection;
	using HttpContrib.Http;

	public static class HttpQueryExtensions
	{
		public static HttpQuery<T> Save<T>(this HttpQuery<T> query, T value)
		{
			throw new NotImplementedException();

			query.Method = HttpMethod.Post;

			return query;
		}

		public static HttpQuery<T> Delete<T, TValue>(this HttpQuery<T> query, TValue value)
		{
			query.QueryBuilder.Delete(value);

			query.Method = HttpMethod.Delete;

			return query;
		}

		public static HttpQuery<T> Where<T, TProperty, TValue>(this HttpQuery<T> query, Expression<Func<T, TProperty>> property, TValue value)
		{
			return WhereInternal(query, property, value);
		}

		private static HttpQuery<T> WhereInternal<T, TProperty>(this HttpQuery<T> query, Expression<Func<T, TProperty>> property, object value)
		{
			query.QueryBuilder.Where(property.GetMemberInfo().Name, value);

			return query;
		}

		public static HttpQuery<T> Take<T>(this HttpQuery<T> query, int count)
		{
			query.QueryBuilder.Take(count);

			return query;
		}

		public static HttpQuery<T> Skip<T>(this HttpQuery<T> query, int count)
		{
			query.QueryBuilder.Skip(count);

			return query;
		}

		public static Uri GetFullyQualifiedQuery<T>(this HttpQuery<T> query, SimpleHttpClient client)
		{
			return GetFullyQualifiedQueryInternal(client, query);
		}

		public static Uri GetFullyQualifiedQuery<T>(this SimpleHttpClient client, HttpQuery<T> query)
		{
			return GetFullyQualifiedQueryInternal(client, query);
		}

		private static Uri GetFullyQualifiedQueryInternal<T>(SimpleHttpClient client, HttpQuery<T> query)
		{
			string baseAddress = client.BaseAddress.ToString();

			if (!String.IsNullOrEmpty(query.Path) && !client.BaseAddress.ToString().EndsWith("/"))
				baseAddress += "/";

			UriBuilder uriBuilder = new UriBuilder(new Uri(baseAddress));
			uriBuilder.Path += query.Path;
			uriBuilder.Query += query.Query;

			return uriBuilder.Uri;
		}

		/// <summary>
		/// Converts an expression into a <see cref="MemberInfo"/>.
		/// </summary>
		/// <param name="expression">The expression to convert.</param>
		/// <returns>The member info.</returns>
		public static MemberInfo GetMemberInfo(this Expression expression)
		{
			var lambda = (LambdaExpression)expression;

			MemberExpression memberExpression;
			if (lambda.Body is UnaryExpression)
			{
				var unaryExpression = (UnaryExpression)lambda.Body;
				memberExpression = (MemberExpression)unaryExpression.Operand;
			}
			else memberExpression = (MemberExpression)lambda.Body;

			return memberExpression.Member;
		}
	}
}