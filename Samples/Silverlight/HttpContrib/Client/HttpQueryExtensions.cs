namespace HttpContrib.Client
{
	using System;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;
	using HttpContrib.Http;

	public static class HttpQueryExtensions
	{
		public static HttpQuery<T> AsHttpQuery<T>(this IQueryable<T> queryable)
		{
			return (HttpQuery<T>)queryable;
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

		public static string GetFullyQualifiedQuery<T>(this HttpQuery<T> query, SimpleHttpClient client)
		{
			Uri uri = new Uri(client.BaseAddress, query.ToString());

			return uri.ToString();
		}

		public static string GetFullyQualifiedQuery<T>(this SimpleHttpClient client, HttpQuery<T> query)
		{
			Uri uri = new Uri(client.BaseAddress, query.ToString());

			return uri.ToString();
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