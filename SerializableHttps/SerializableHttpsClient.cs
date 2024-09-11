using SerializableHttps.AuthenticationMethods;
using SerializableHttps.Exceptions;
using SerializableHttps.Serialisers;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SerializableHttps.Tests")]
namespace SerializableHttps
{
	public class SerializableHttpsClient
	{
		public HttpRequestHeaders Headers => _client.DefaultRequestHeaders;
		internal HttpClient _client;

		public TimeSpan TimeOut
		{
			get => _client.Timeout;
			set => _client.Timeout = value;
		}

		public SerializableHttpsClient(HttpClient client)
		{
			_client = client;
		}

		public SerializableHttpsClient() : this(new HttpClient())
		{
		}

		public void SetAuthentication(IAuthenticationMethod method) => _client.DefaultRequestHeaders.Authorization = method.GetHeaderValue();

		public TOut Post<TIn, TOut>(TIn input, string address)
			where TIn : notnull
			where TOut : notnull
		{
			var task = PostAsync<TIn, TOut>(input, address);
			task.Start();
			task.Wait();
			return task.Result;
		}

		public async Task<TOut> PostAsync<TIn, TOut>(TIn input, string address)
			where TIn : notnull
			where TOut : notnull
		{
			var content = BodySerialiser.SerializeContent(input);
			var response = await _client.PostAsync(address, content);
			if (response.StatusCode != System.Net.HttpStatusCode.OK)
				throw new HttpGeneralException($"Server did not respond with an OK! Response code: {response.StatusCode}", await response.Content.ReadAsStringAsync());
			return await BodySerialiser.DeserializeContentAsync<TOut>(response.Content);
		}

		public TOut Patch<TIn, TOut>(TIn input, string address)
			where TIn : notnull
			where TOut : notnull
		{
			var task = PatchAsync<TIn, TOut>(input, address);
			task.Start();
			task.Wait();
			return task.Result;
		}

		public async Task<TOut> PatchAsync<TIn, TOut>(TIn input, string address)
			where TIn : notnull
			where TOut : notnull
		{
			var content = BodySerialiser.SerializeContent(input);
			var response = await _client.PatchAsync(address, content);
			if (response.StatusCode != System.Net.HttpStatusCode.OK)
				throw new HttpGeneralException($"Server did not respond with an OK! Response code: {response.StatusCode}", await response.Content.ReadAsStringAsync());
			return await BodySerialiser.DeserializeContentAsync<TOut>(response.Content);
		}

		public TOut Get<TIn, TOut>(TIn input, string address)
			where TIn : notnull
			where TOut : notnull
		{
			var task = GetAsync<TIn, TOut>(input, address);
			task.Start();
			task.Wait();
			return task.Result;
		}

		public async Task<TOut> GetAsync<TIn, TOut>(TIn input, string address)
			where TIn : notnull
			where TOut : notnull
		{
			address += HeaderSerialiser.QuerryfiModel(input);
			var response = await _client.GetAsync(address);
			if (response.StatusCode != System.Net.HttpStatusCode.OK)
				throw new HttpGeneralException($"Server did not respond with an OK! Response code: {response.StatusCode}", await response.Content.ReadAsStringAsync());
			return await BodySerialiser.DeserializeContentAsync<TOut>(response.Content);
		}

		public TOut Delete<TIn, TOut>(TIn input, string address)
			where TIn : notnull
			where TOut : notnull
		{
			var task = DeleteAsync<TIn, TOut>(input, address);
			task.Start();
			task.Wait();
			return task.Result;
		}

		public async Task<TOut> DeleteAsync<TIn, TOut>(TIn input, string address)
			where TIn : notnull
			where TOut : notnull
		{
			address += HeaderSerialiser.QuerryfiModel(input);
			var response = await _client.DeleteAsync(address);
			if (response.StatusCode != System.Net.HttpStatusCode.OK)
				throw new HttpGeneralException($"Server did not respond with an OK! Response code: {response.StatusCode}", await response.Content.ReadAsStringAsync());
			return await BodySerialiser.DeserializeContentAsync<TOut>(response.Content);
		}
	}
}
