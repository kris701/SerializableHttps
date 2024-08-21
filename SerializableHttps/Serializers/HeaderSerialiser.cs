using System.Globalization;
using System.Reflection;
using System.Web;

namespace SerializableHttps.Serializers
{
	public static class HeaderSerialiser
	{
		public static string QuerryfiModel<T>(T model) where T : notnull
		{
			var query = HttpUtility.ParseQueryString("");
			Type modelTypeInfo = model.GetType();

			if (!IsPrimitive(model))
			{
				foreach (PropertyInfo propertyInfo in modelTypeInfo.GetProperties())
				{
					var value = propertyInfo.GetValue(model, null);
					if (value != null)
					{
						if (value.GetType() == typeof(DateTime))
							query[propertyInfo.Name] = ((DateTime)value).ToString("u", CultureInfo.InvariantCulture);
						else
							query[propertyInfo.Name] = value.ToString();
					}
				}
			}
			else
				throw new TargetParameterCountException("Please wrap in model before using parameters!");

			return $"?{query}";
		}

		private static bool IsPrimitive<T>(T value)
		{
			Type modelTypeInfo = value.GetType();

			return modelTypeInfo.IsPrimitive || modelTypeInfo.IsValueType || (modelTypeInfo == typeof(string));
		}
	}
}
