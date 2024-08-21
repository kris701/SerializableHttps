using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SerializableHttps.Models
{
	public class FileModel
	{
		private MemoryStream _str;
		public MemoryStream GetFileContent() { return _str; }

		[Required]
		public string FileName { get; set; }
		[Required]
		public string FileType { get; set; }

		[JsonIgnore]
		public long Length => _str.Length;

		public FileModel(string fileName, string fileType, MemoryStream stream)
		{
			FileName = fileName;
			FileType = fileType;
			_str = stream;
			_str.Position = 0;
		}
	}
}
