using RichardSzalay.MockHttp;
using SerializableHttps.AuthenticationMethods;
using SerializableHttps.Models;
using SerializableHttps.Serialisers;
using SerializableHttps.Tests.TestClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SerializableHttps.Tests
{
	[TestClass]
	public class SerializableHttpsClientTests
	{
		public static IEnumerable<object[]> ExecuteJsonItems()
		{
			yield return new object[] {
				new TestClass1()
				{
					Text = "test",
					Value = -1
				},
				new TestClass2()
				{
					Text = "new test",
					Value = 1,
					DoubleValue = 345.12,
					Time = DateTime.Now
				},
				new Func<SerializableHttpsClient, string, dynamic, Task<dynamic>>(
					async (c, a, i) => await c.PostAsync<TestClass1,TestClass2>(i, a))
			};
			yield return new object[] {
				new TestClass1()
				{
					Text = "test",
					Value = -1
				},
				new TestClass2()
				{
					Text = "new test",
					Value = 1,
					DoubleValue = 345.12,
					Time = DateTime.Now
				},
				new Func<SerializableHttpsClient, string, dynamic, Task<dynamic>>(
					async (c, a, i) => await c.PatchAsync<TestClass1,TestClass2>(i, a))
			};
			yield return new object[] {
				new TestClass1()
				{
					Text = "test",
					Value = -1
				},
				new TestClass2()
				{
					Text = "new test",
					Value = 1,
					DoubleValue = 345.12,
					Time = DateTime.Now
				},
				new Func<SerializableHttpsClient, string, dynamic, Task<dynamic>>(
					async (c, a, i) => await c.GetAsync<TestClass1,TestClass2>(i, a))
			};
			yield return new object[] {
				new TestClass1()
				{
					Text = "test",
					Value = -1
				},
				new TestClass2()
				{
					Text = "new test",
					Value = 1,
					DoubleValue = 345.12,
					Time = DateTime.Now
				},
				new Func<SerializableHttpsClient, string, dynamic, Task<dynamic>>(
					async (c, a, i) => await c.DeleteAsync<TestClass1,TestClass2>(i, a))
			};
		}

		public static IEnumerable<object[]> ExecuteXMLItems()
		{
			yield return new object[] {
				new TestClass1()
				{
					Text = "test",
					Value = -1
				},
				new XElement("name", "value"),
				new Func<SerializableHttpsClient, string, dynamic, Task<XElement>>(
					async (c, a, i) => await c.PostAsync<TestClass1,XElement>(i, a))
			};
			yield return new object[] {
				new TestClass1()
				{
					Text = "test",
					Value = -1
				},
				new XElement("name", "value"),
				new Func<SerializableHttpsClient, string, dynamic, Task<XElement>>(
					async (c, a, i) => await c.PatchAsync<TestClass1,XElement>(i, a))
			};
			yield return new object[] {
				new TestClass1()
				{
					Text = "test",
					Value = -1
				},
				new XElement("name", "value"),
				new Func<SerializableHttpsClient, string, dynamic, Task<XElement>>(
					async (c, a, i) => await c.GetAsync<TestClass1,XElement>(i, a))
			};
			yield return new object[] {
				new TestClass1()
				{
					Text = "test",
					Value = -1
				},
				new XElement("name", "value"),
				new Func<SerializableHttpsClient, string, dynamic, Task<XElement>>(
					async (c, a, i) => await c.DeleteAsync<TestClass1,XElement>(i, a))
			};
		}

		public static IEnumerable<object[]> ExecuteStreamItems()
		{
			yield return new object[] {
				new TestClass1()
				{
					Text = "test",
					Value = -1
				},
				new FileDataModel(GenerateStreamFromString("some funny content")),
				new Func<SerializableHttpsClient, string, dynamic, Task<dynamic>>(
					async (c, a, i) => await c.PostAsync<TestClass1,FileDataModel>(i, a))
			};
			yield return new object[] {
				new TestClass1()
				{
					Text = "test",
					Value = -1
				},
				new FileDataModel(GenerateStreamFromString("some funny content")),
				new Func<SerializableHttpsClient, string, dynamic, Task<dynamic>>(
					async (c, a, i) => await c.PatchAsync<TestClass1,FileDataModel>(i, a))
			};
			yield return new object[] {
				new TestClass1()
				{
					Text = "test",
					Value = -1
				},
				new FileDataModel(GenerateStreamFromString("some funny content")),
				new Func<SerializableHttpsClient, string, dynamic, Task<dynamic>>(
					async (c, a, i) => await c.GetAsync<TestClass1,FileDataModel>(i, a))
			};
			yield return new object[] {
				new TestClass1()
				{
					Text = "test",
					Value = -1
				},
				new FileDataModel(GenerateStreamFromString("some funny content")),
				new Func<SerializableHttpsClient, string, dynamic, Task<dynamic>>(
					async (c, a, i) => await c.DeleteAsync<TestClass1,FileDataModel>(i, a))
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

		public static IEnumerable<object[]> AuthenticationMethods()
		{
			yield return new object[] {
				new JWTAuthenticationMethod("some token")
			};
		}

		[TestMethod]
		[DynamicData(nameof(AuthenticationMethods), DynamicDataSourceType.Method)]
		public void Can_SetAuthentication(IAuthenticationMethod method)
		{
			// ARRANGE
			var client = new SerializableHttpsClient();
			var header = method.GetHeaderValue();

			// ACT
			client.SetAuthentication(method);

			// ASSERT
			Assert.IsNotNull(client._client.DefaultRequestHeaders.Authorization);
			Assert.AreEqual(client._client.DefaultRequestHeaders.Authorization.Scheme, header.Scheme);
			Assert.AreEqual(client._client.DefaultRequestHeaders.Authorization.Parameter, header.Parameter);
		}

		[TestMethod]
		[DynamicData(nameof(ExecuteJsonItems), DynamicDataSourceType.Method)]
		public async Task Can_Execute_JsonResult(dynamic input, dynamic expected, Func<SerializableHttpsClient, string, dynamic, Task<dynamic>> execute)
		{
			// ARRANGE
			var address = "http://localhost/api/test";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.When(address).Respond((HttpContent)BodySerialiser.SerializeContent(expected));
			var client = new SerializableHttpsClient(new HttpClient(mockHttp));

			// ACT
			var response = await execute(client, address, input);

			// ASSERT
			Assert.AreEqual(expected, response);
		}

		[TestMethod]
		[DynamicData(nameof(ExecuteXMLItems), DynamicDataSourceType.Method)]
		public async Task Can_Execute_XMLResult(dynamic input, XElement expected, Func<SerializableHttpsClient, string, dynamic, Task<XElement>> execute)
		{
			// ARRANGE
			var address = "http://localhost/api/test";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.When(address).Respond((HttpContent)BodySerialiser.SerializeContent(expected));
			var client = new SerializableHttpsClient(new HttpClient(mockHttp));

			// ACT
			XElement response = await execute(client, address, input);

			// ASSERT
			Assert.AreEqual(expected.ToString(), response.ToString());
		}

		[TestMethod]
		[DynamicData(nameof(ExecuteStreamItems), DynamicDataSourceType.Method)]
		public async Task Can_Execute_StreamResult(dynamic input, dynamic expected, Func<SerializableHttpsClient, string, dynamic, Task<dynamic>> execute)
		{
			// ARRANGE
			var address = "http://localhost/api/test";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.When(address).Respond((StreamContent)BodySerialiser.SerializeContent(expected));
			var client = new SerializableHttpsClient(new HttpClient(mockHttp));

			// ACT
			dynamic response = await execute(client, address, input);

			// ASSERT
			Assert.AreEqual(expected.ToString(), response.ToString());
		}
	}
}
