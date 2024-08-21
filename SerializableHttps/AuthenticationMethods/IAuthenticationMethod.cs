using System.Net.Http.Headers;

namespace SerializableHttps.AuthenticationMethods
{
	public interface IAuthenticationMethod
	{
		public AuthenticationHeaderValue GetHeaderValue();
	}
}
