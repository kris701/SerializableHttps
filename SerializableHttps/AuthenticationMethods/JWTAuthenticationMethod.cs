using System.Net.Http.Headers;

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
