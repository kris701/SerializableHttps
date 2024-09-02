using SerializableHttps.Serialisers;
using SerializableHttps.Tests.TestClasses;
using System.Xml.Linq;

namespace SerializableHttps.Tests.Serialisers
{
	[TestClass]
	public class BodySerialiserTests
	{
		public static IEnumerable<object[]> SerialisableData()
		{
			yield return new object[]
			{
				new TestClass1()
				{
					Text = "some value",
					Value = 1421
				},
				"{\"Text\":\"some value\",\"Value\":1421}"
			};
			yield return new object[] {
				new TestClass1()
				{
					Text = "some value     a",
					Value = -23
				},
				"{\"Text\":\"some value     a\",\"Value\":-23}"
			};
			yield return new object[] {
				new TestClass2()
				{
					Text = "some value",
					Value = 1421,
					DoubleValue = -1,
					Time = DateTime.MinValue
				},
				"{\"double_value\":-1,\"Time\":\"0001-01-01T00:00:00\",\"Text\":\"some value\",\"Value\":1421}"
			};
			yield return new object[] {
				new XElement("name", "some value"),
				$"<?xml version=\"1.0\" encoding=\"utf-8\"?>{Environment.NewLine}<name>some value</name>"
			};
			yield return new object[] {
				new TestClass3(GenerateStreamFromString("file content"))
				{
					ID = new Guid("926c3b14-f368-4c68-9819-ddbbc26551d1")
				},
				"file content",
			};
		}

		public static MemoryStream GenerateStreamFromString(string s)
		{
			var stream = new MemoryStream();
			var writer = new StreamWriter(stream);
			writer.Write(s);
			writer.Flush();
			stream.Position = 0;
			return stream;
		}

		[TestMethod]
		[DynamicData(nameof(SerialisableData), DynamicDataSourceType.Method)]
		public async Task Can_Serialise(dynamic item, string expected)
		{
			// ARRANGE
			// ACT
			HttpContent serialised = BodySerialiser.SerializeContent(item);

			// ASSERT
			Assert.AreEqual(expected, await serialised.ReadAsStringAsync());
		}
	}
}
