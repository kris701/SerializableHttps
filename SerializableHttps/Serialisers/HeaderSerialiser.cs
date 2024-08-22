using SerializableHttps.Exceptions;
using SerializableHttps.Models;
using System.Globalization;
using System.Reflection;
using System.Web;

namespace SerializableHttps.Serialisers
{
	public static class HeaderSerialiser
	{
		public static string QuerryfiModel<T>(T model) where T : notnull
		{
			var query = HttpUtility.ParseQueryString("");
			Type modelTypeInfo = model.GetType();

			if (typeof(T).IsAssignableTo(typeof(FileModel)))
				throw new HttpSerialisationException("Cannot querrify a model that is based on the FileModel class!");

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
				throw new HttpSerialisationException("Please wrap in model before using parameters!");

			return $"?{query}";
		}

		private static bool IsPrimitive<T>(T value) where T : notnull
		{
			Type modelTypeInfo = value.GetType();

			return modelTypeInfo.IsPrimitive || modelTypeInfo.IsValueType || modelTypeInfo == typeof(string);
		}
	}
}
