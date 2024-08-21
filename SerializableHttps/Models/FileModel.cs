using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SerializableHttps.Models
{
	public class FileModel
	{
		private readonly MemoryStream _str;

		[Required]
		public string FileName { get; set; }

		[JsonIgnore]
		public long Length => _str.Length;

		public FileModel(string fileName, MemoryStream stream)
		{
			FileName = fileName;
			_str = stream;
			_str.Position = 0;
		}

		public MemoryStream GetFileContent() => _str;
	}
}
