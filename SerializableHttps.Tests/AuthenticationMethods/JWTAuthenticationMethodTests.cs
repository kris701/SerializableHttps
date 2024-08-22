using SerializableHttps.AuthenticationMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializableHttps.Tests.AuthenticationMethods
{
	[TestClass]
	public class JWTAuthenticationMethodTests
	{
		[TestMethod]
		[DataRow("safdasf")]
		[DataRow("456t34easwef")]
		[DataRow("sdfgw345690u<sgwop4350+p2w34u90+")]
		public void Can_SetToken(string token)
		{
			// ARRANGE

			// ACT
			var method = new JWTAuthenticationMethod(token);

			// ASSERT
			Assert.AreEqual(token, method.Token);
		}
	}
}
