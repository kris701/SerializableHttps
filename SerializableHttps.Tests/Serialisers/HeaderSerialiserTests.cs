using SerializableHttps.Exceptions;
using SerializableHttps.Serialisers;
using SerializableHttps.Tests.TestClasses;

namespace SerializableHttps.Tests.Serialisers
{
	[TestClass]
	public class HeaderSerialiserTests
	{
		public static IEnumerable<object[]> QuerifiableData()
		{
			yield return new object[] {
				new TestClass1()
				{
					Text = "some value",
					Value = 1421
				},
				"?Text=some+value&Value=1421"
			};
			yield return new object[] {
				new TestClass1()
				{
					Text = "some value     a",
					Value = -23
				},
				"?Text=some+value+++++a&Value=-23"
			};
			yield return new object[] {
				new TestClass2()
				{
					Text = "some value",
					Value = 1421,
					DoubleValue = -1,
					Time = DateTime.MinValue
				},
				"?DoubleValue=-1&Time=0001-01-01+00%3a00%3a00Z&Text=some+value&Value=1421"
			};
		}

		public static IEnumerable<object[]> NonhuerifiableData()
		{
			yield return new object[] { 
				(int)352
			};
			yield return new object[] {
				(double)6.24
			};
			yield return new object[] {
				DateTime.Now
			};
			yield return new object[] {
				"test"
			};
		}

		[TestMethod]
		[DynamicData(nameof(QuerifiableData), DynamicDataSourceType.Method)]
		public void Can_QueryfiModel(dynamic item, string expected)
		{
			// ARRANGE
			// ACT
			var querry = HeaderSerialiser.QuerryfiModel(item);

			// ASSERT
			Assert.AreEqual(expected, querry);
		}

		[TestMethod]
		[ExpectedException(typeof(HttpSerialisationException), "Please wrap in model before using parameters!")]
		[DynamicData(nameof(NonhuerifiableData), DynamicDataSourceType.Method)]
		public void Cant_QueryfiModel_IfNotModel(dynamic item)
		{
			// ARRANGE
			// ACT
			HeaderSerialiser.QuerryfiModel(item);
		}

		[TestMethod]
		[ExpectedException(typeof(HttpSerialisationException), "Cannot querrify a model that is based on the FileModel class!")]
		public void Cant_QueryfiModel_IfItIsFileModel()
		{
			// ARRANGE
			var item = new TestClass3(new MemoryStream()) { ID = Guid.NewGuid() };

			// ACT
			HeaderSerialiser.QuerryfiModel(item);
		}
	}
}
