namespace HttpContrib.Client
{
	using System;
	using System.Linq.Expressions;
	using System.Reflection;
	using HttpContrib.Http;

	public static class HttpQueryExtensions
	{
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