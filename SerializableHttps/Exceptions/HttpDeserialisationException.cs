namespace SerializableHttps.Exceptions
{
	public class HttpDeserialisationException : Exception
	{
		public string Body { get; set; }
		public HttpDeserialisationException(string? message, string body) : base(message)
		{
			Body = body;
		}
	}
}
