namespace HttpContrib.Client
{
	using System;
	using System.Collections.Generic;
	using System.Text;

	public class QueryBuilder
	{
		private const string EQUAL = " eq ";
		private const string SKIP = "$skip=";
		private const string TOP = "$top=";
		private const string WHERE = "$filter=";

		private readonly string _resourceName;
		private readonly List<string> _filters;

		public QueryBuilder(string resourceName)
		{
			_filters = new List<string>();

			_resourceName = resourceName;
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

			_filters.Add(builder.ToString());
		}

		public void Take(int count)
		{
			_filters.Add(String.Format("{0}{1}", TOP, count));
		}

		public void Skip(int count)
		{
			_filters.Add(String.Format("{0}{1}", SKIP, count));
		}

		public string Build()
		{
			StringBuilder builder = new StringBuilder();

			if (!String.IsNullOrEmpty(_resourceName))
			{
				builder.Append(_resourceName);
			}

			for (int i = 0; i < _filters.Count; i++)
			{
				if (i == 0)
					builder.Append("?");

				if (i != 0)
					builder.Append("&");

				builder.Append(_filters[i]);
			}

			return builder.ToString();
		}
	}
}
