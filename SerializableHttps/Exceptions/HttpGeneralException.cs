using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SerializableHttps.Exceptions
{
	public class HttpGeneralException : Exception
	{
		public HttpStatusCode ErrorCode { get; set; }
		public string Body { get; set; }
		public HttpGeneralException(string? message, HttpStatusCode errorCode, string body) : base(message)
		{
			ErrorCode = errorCode;
			Body = body;
		}
	}
}
