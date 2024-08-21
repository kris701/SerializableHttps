using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace SerializableHttps.AuthenticationMethods
{
	public class JWTAuthenticationMethod : IAuthenticationMethod
	{
		public string Token { get; set; }

		public JWTAuthenticationMethod(string token)
		{
			Token = token;
		}

		public AuthenticationHeaderValue GetHeaderValue() => new AuthenticationHeaderValue("Bearer", Token);
	}
}
