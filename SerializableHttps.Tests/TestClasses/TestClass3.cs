using SerializableHttps.Models;

namespace SerializableHttps.Tests.TestClasses
{
	public class TestClass3 : FileDataModel
	{
		public TestClass3(MemoryStream stream) : base(stream)
		{
		}

		public Guid ID { get; set; }
	}
}
