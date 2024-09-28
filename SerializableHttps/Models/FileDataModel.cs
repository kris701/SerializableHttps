using System.Text.Json.Serialization;

namespace SerializableHttps.Models
{
	/// <summary>
	/// Model to represent a data stream
	/// </summary>
	public class FileDataModel
	{
		private readonly MemoryStream _str;

		/// <summary>
		/// The full length of the stream
		/// </summary>
		[JsonIgnore]
		public long Length => _str.Length;

		/// <summary>
		/// Main constructor
		/// </summary>
		/// <param name="stream"></param>
		public FileDataModel(MemoryStream stream)
		{
			_str = stream;
			_str.Position = 0;
		}

		/// <summary>
		/// Method to get the current file stream
		/// </summary>
		/// <returns></returns>
		public MemoryStream GetFileContent() => _str;
	}
}
