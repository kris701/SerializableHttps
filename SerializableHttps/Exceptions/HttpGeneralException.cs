using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializableHttps.Exceptions
{
	public class HttpGeneralException : Exception
	{
		public string Body { get; set; }
		public HttpGeneralException(string? message, string body) : base(message)
		{
			Body = body;
		}
	}
}
