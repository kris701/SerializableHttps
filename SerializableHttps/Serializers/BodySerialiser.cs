using SerializableHttps.Models;
using System.Text.Json;
using System.Xml.Linq;

namespace SerializableHttps.Serializers
{
	public static class BodySerialiser
	{
		private static readonly JsonSerializerOptions _options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

		public static async Task<T> DeserializeContentAsync<T>(HttpContent content) where T : notnull
		{
			var targetType = typeof(T);
			if (targetType == typeof(FileModel))
			{
				if (content.Headers.ContentDisposition != null && content.Headers.ContentDisposition.FileName != null)
				{
					var split = content.Headers.ContentDisposition.FileName.Split('.');
					var contentStream = await content.ReadAsStreamAsync();
					var str = new MemoryStream();
					contentStream.CopyTo(str);
					str.Position = 0;
					var info = new FileModel(
						split[0],
						split[1],
						str);
					return (dynamic)info;
				}
				throw new Exception("Attempted to deserialise to a FileModel, however content disposition was not set!");
			}
			if (targetType == typeof(string))
				return (dynamic)await content.ReadAsStringAsync();
			if (targetType == typeof(XElement))
				return (dynamic)XElement.Parse(await content.ReadAsStringAsync());

			var deserialized = JsonSerializer.Deserialize<T>((await content.ReadAsStringAsync()), _options);
			if (deserialized == null)
				throw new Exception("Could not deserialise to target type!");
			return deserialized;
		}

		public static HttpContent SerializeContent<T>(T model) where T : notnull
		{
			if (model is FileModel fileHeader)
			{
				MultipartFormDataContent newContent = new MultipartFormDataContent();
				newContent.Add(new StreamContent(fileHeader.GetFileContent()), "file", $"{fileHeader.FileName}.{fileHeader.FileType}");
				return newContent;
			}
			if (model is string modelString)
				return new StringContent(modelString, System.Text.Encoding.UTF8, "text/plain");
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
