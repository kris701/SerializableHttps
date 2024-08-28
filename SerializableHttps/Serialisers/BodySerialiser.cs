using SerializableHttps.Exceptions;
using SerializableHttps.Models;
using System.Text.Json;
using System.Xml.Linq;

namespace SerializableHttps.Serialisers
{
	public static class BodySerialiser
	{
		private static readonly JsonSerializerOptions _options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

		public static async Task<T> DeserializeContentAsync<T>(HttpContent content) where T : notnull
		{
			try
			{
				var targetType = typeof(T);
				if (targetType.IsAssignableTo(typeof(FileDataModel)) && targetType != typeof(FileDataModel))
					throw new HttpDeserialisationException($"Cannot deserialise a FileDataModel into an inhertied type!", await content.ReadAsStringAsync());

				if (targetType == typeof(FileDataModel))
					return (dynamic)new FileDataModel((MemoryStream)(await content.ReadAsStreamAsync()));
				if (targetType == typeof(XElement))
					return (dynamic)XElement.Parse(await content.ReadAsStringAsync());

				var deserialized = JsonSerializer.Deserialize<T>(await content.ReadAsStringAsync(), _options);
				if (deserialized == null)
					throw new HttpDeserialisationException($"Could not deserialise to target type: {typeof(T)}!", await content.ReadAsStringAsync());
				return deserialized;
			}
			catch(Exception e)
			{
				throw new HttpDeserialisationException($"An error occured during deserialisation: {e.Message}", await content.ReadAsStringAsync());
			}
		}

		public static HttpContent SerializeContent<T>(T model) where T : notnull
		{
			if (model is FileDataModel fileHeader)
				return new StreamContent(fileHeader.GetFileContent());
			if (model is XElement xml)
				return new StringContent(ConvertXElementToString(xml), System.Text.Encoding.UTF8, "text/xml");

			string content = JsonSerializer.Serialize(model);
			return new StringContent(content, System.Text.Encoding.UTF8, "application/json");
		}

		private static string ConvertXElementToString(XElement element)
		{
			string result;

			if (element.Document != null)
				element.Document.Declaration = new XDeclaration("1.0", "utf-8", null);

			using (StringWriter writer = new StringWriter())
			{
				element.Save(writer);
				result = writer.ToString();
			}

			result = result.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "<?xml version=\"1.0\" encoding=\"utf-8\"?>");

			return result;
		}
	}
}
