namespace HttpContrib.Client
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public class QueryBuilder
	{
		private const string EQUAL = " eq ";
		private const string SKIP = "$skip=";
		private const string TOP = "$top=";
		private const string WHERE = "$filter=";

		private readonly List<Filter> _filters;

		public QueryBuilder(string resourceName)
		{
			_filters = new List<Filter>();

			if (!String.IsNullOrEmpty(resourceName))
			{
				_filters.Add(new Filter { IsResourcePath = true, Value = resourceName });
			}
		}

		public void Delete(object key)
		{
			_filters.Add(new Filter { IsResourcePath = true, Value = key.ToString() });
		}

        public void Where(string property, object value)
		{
			StringBuilder builder = new StringBuilder();

			builder.Append(WHERE);
			builder.Append(property);
			builder.Append(EQUAL);

			var typeCode = Type.GetTypeCode(value.GetType());

			switch (typeCode)
			{
				case TypeCode.Boolean:
					builder.Append((true == (bool)value) ? 1 : 0);
					break;
				case TypeCode.String:
					builder.AppendFormat("'{0}'", value);
					break;
				case TypeCode.Object:
				case TypeCode.DateTime:
					throw new NotSupportedException(string.Format("The value '{0}' of Type '{1}' is not supported", value, typeCode));
				default:
					builder.Append(value);
					break;
			}

			_filters.Add(new Filter { Value = builder.ToString() });
		}

		public void Take(int count)
		{
			_filters.Add(new Filter { Value = String.Format("{0}{1}", TOP, count) });
		}

		public void Skip(int count)
		{
			_filters.Add(new Filter { Value = String.Format("{0}{1}", SKIP, count) });
		}

		public string Build()
		{
			return this.BuildPath() + this.BuildQuery();
		}

		public string BuildQuery()
		{
			StringBuilder builder = new StringBuilder();

			var filters = _filters.Where(f => !f.IsResourcePath).ToList();

			for (int i = 0; i < filters.Count; i++)
			{
				if (i != 0)
					builder.Append("&");

				builder.Append(filters[i].Value);
			}

			return builder.ToString();
		}

		public string BuildPath()
		{
			UriBuilder builder = new UriBuilder();

			var keys = _filters.Where(f => f.IsResourcePath).ToList();

			for (int i = 0; i < keys.Count; i++)
			{
				if (i > 0)
					builder.Path += "/";
				builder.Path += keys[i].Value;
			}

			return builder.ToString().Replace("http://localhost/", "");
		}

		private class Filter
		{
			public bool IsResourcePath { get; set; }
			public string Value { get; set; }
		}
	}
}
