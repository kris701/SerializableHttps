using System.Net.Http.Headers;

namespace SerializableHttps.AuthenticationMethods
{
	public class APIKeyAuthenticationMethod : IAuthenticationMethod
	{
		public string Key { get; set; }

		public APIKeyAuthenticationMethod(string key)
		{
			Key = key;
		}

		public AuthenticationHeaderValue GetHeaderValue() => new AuthenticationHeaderValue("Api-Key", Key);
	}
}
