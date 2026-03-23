using SerializableHttps.Exceptions;
using SerializableHttps.Models;
using System.Globalization;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Web;

namespace SerializableHttps.Serialisers
{
    internal static class HeaderSerialiser
	{
		public static string QuerryfiModel<T>(T model)
		{
			if (model == null)
				return "";

			var query = HttpUtility.ParseQueryString("");
			Type modelTypeInfo = model.GetType();

			if (typeof(T).IsAssignableTo(typeof(FileDataModel)))
				throw new HttpSerialisationException("Cannot querrify a model that is based on the FileModel class!");
			if (model is string str)
				return str;

			if (!IsPrimitive(model))
			{
				foreach (PropertyInfo propertyInfo in modelTypeInfo.GetProperties())
				{
					var targetName = propertyInfo.Name;
					var nameAttribute = propertyInfo.GetCustomAttribute<JsonPropertyNameAttribute>();
					if (nameAttribute is JsonPropertyNameAttribute attr)
						targetName = attr.Name;

					var value = propertyInfo.GetValue(model, null);
					if (value != null)
					{
						if (value.GetType() == typeof(DateTime))
							query[targetName] = ((DateTime)value).ToString("u", CultureInfo.InvariantCulture);
						else
							query[targetName] = value.ToString();
					}
				}
			}
			else
				throw new HttpSerialisationException("Please wrap in model before using parameters!");

			return $"?{query}";
		}

		private static bool IsPrimitive<T>(T value)
		{
			Type modelTypeInfo = value!.GetType();

			return modelTypeInfo.IsPrimitive || modelTypeInfo.IsValueType || modelTypeInfo == typeof(string);
		}
	}
}
