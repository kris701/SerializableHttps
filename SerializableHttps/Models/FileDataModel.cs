using System.Text.Json.Serialization;

namespace SerializableHttps.Models
{
	public class FileDataModel
	{
		private readonly MemoryStream _str;

		[JsonIgnore]
		public long Length => _str.Length;

		public FileDataModel(MemoryStream stream)
		{
			_str = stream;
			_str.Position = 0;
		}

		public MemoryStream GetFileContent() => _str;
	}
}
