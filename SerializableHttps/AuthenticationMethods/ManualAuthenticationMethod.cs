using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SerializableHttps.AuthenticationMethods
{
	public class ManualAuthenticationMethod : IAuthenticationMethod
	{
		public AuthenticationHeaderValue Header { get; set; }

		public ManualAuthenticationMethod(AuthenticationHeaderValue header)
		{
			Header = header;
		}

		public AuthenticationHeaderValue GetHeaderValue() => Header;
	}
}
