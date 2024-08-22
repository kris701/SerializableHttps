using SerializableHttps.AuthenticationMethods;
using SerializableHttps.Serialisers;

namespace SerializableHttps
{
	public class SerializableHttpsClient
	{
		private readonly HttpClient _client;

		public TimeSpan TimeSpan
		{
			get => _client.Timeout;
			set => _client.Timeout = value;
		}

		public SerializableHttpsClient()
		{
			_client = new HttpClient();
		}

		public void SetAuthentication(IAuthenticationMethod method) => _client.DefaultRequestHeaders.Authorization = method.GetHeaderValue();

		public TOut Post<TIn, TOut>(TIn input, string address)
			where TIn : notnull
			where TOut : notnull
		{
			var task = PostAsync<TIn, TOut>(input, address);
			task.Wait();
			return task.Result;
		}

		public async Task<TOut> PostAsync<TIn, TOut>(TIn input, string address)
			where TIn : notnull
			where TOut : notnull
		{
			var content = BodySerialiser.SerializeContent(input);
			var response = await _client.PostAsync(address, content);
			return await BodySerialiser.DeserializeContentAsync<TOut>(response.Content);
		}

		public TOut Patch<TIn, TOut>(TIn input, string address)
			where TIn : notnull
			where TOut : notnull
		{
			var task = PatchAsync<TIn, TOut>(input, address);
			task.Wait();
			return task.Result;
		}

		public async Task<TOut> PatchAsync<TIn, TOut>(TIn input, string address)
			where TIn : notnull
			where TOut : notnull
		{
			var content = BodySerialiser.SerializeContent(input);
			var response = await _client.PatchAsync(address, content);
			return await BodySerialiser.DeserializeContentAsync<TOut>(response.Content);
		}

		public TOut Get<TIn, TOut>(TIn input, string address)
			where TIn : notnull
			where TOut : notnull
		{
			var task = GetAsync<TIn, TOut>(input, address);
			task.Wait();
			return task.Result;
		}

		public async Task<TOut> GetAsync<TIn, TOut>(TIn input, string address)
			where TIn : notnull
			where TOut : notnull
		{
			address += HeaderSerialiser.QuerryfiModel(input);
			var response = await _client.GetAsync(address);
			return await BodySerialiser.DeserializeContentAsync<TOut>(response.Content);
		}

		public TOut Delete<TIn, TOut>(TIn input, string address)
			where TIn : notnull
			where TOut : notnull
		{
			var task = DeleteAsync<TIn, TOut>(input, address);
			task.Wait();
			return task.Result;
		}

		public async Task<TOut> DeleteAsync<TIn, TOut>(TIn input, string address)
			where TIn : notnull
			where TOut : notnull
		{
			address += HeaderSerialiser.QuerryfiModel(input);
			var response = await _client.DeleteAsync(address);
			return await BodySerialiser.DeserializeContentAsync<TOut>(response.Content);
		}
	}
}
