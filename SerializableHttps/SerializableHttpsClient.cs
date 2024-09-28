using SerializableHttps.AuthenticationMethods;
using SerializableHttps.Exceptions;
using SerializableHttps.Serialisers;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SerializableHttps.Tests")]
namespace SerializableHttps
{
	/// <summary>
	/// Main HTTP client
	/// </summary>
	public class SerializableHttpsClient
	{
		/// <summary>
		/// Additional headers to send with the HTTP request
		/// </summary>
		public HttpRequestHeaders Headers => _client.DefaultRequestHeaders;

		/// <summary>
		/// Timeout for HTTP requests
		/// </summary>
		public TimeSpan TimeOut
		{
			get => _client.Timeout;
			set => _client.Timeout = value;
		}

        internal HttpClient _client;

		/// <summary>
		/// Constructor with base HTTP client
		/// </summary>
		/// <param name="client"></param>
        public SerializableHttpsClient(HttpClient client)
		{
			_client = client;
		}

		/// <summary>
		/// Main constructor
		/// </summary>
		public SerializableHttpsClient() : this(new HttpClient())
		{
		}

		/// <summary>
		/// Set authentication headers
		/// </summary>
		/// <param name="method"></param>
		public void SetAuthentication(IAuthenticationMethod method) => _client.DefaultRequestHeaders.Authorization = method.GetHeaderValue();

        #region POST
        /// <summary>
        /// Send a POST request to a given <paramref name="address"/>
        /// </summary>
        /// <typeparam name="TOut">What to deserialize the response as</typeparam>
        /// <param name="address">Target URL address</param>
        /// <returns>An instance of <typeparamref name="TOut"/></returns>
        public TOut Post<TOut>(string address) where TOut : notnull => Post<EmptyModel, TOut>(new EmptyModel(), address);
        /// <summary>
        /// Send a POST request to a given <paramref name="address"/>
        /// </summary>
        /// <typeparam name="TIn">What to serialize the request as</typeparam>
        /// <typeparam name="TOut">What to deserialize the response as</typeparam>
        /// <param name="input">Input model to serialize</param>
        /// <param name="address">Target URL address</param>
        /// <returns>An instance of <typeparamref name="TOut"/></returns>
        public TOut Post<TIn, TOut>(TIn input, string address)
			where TIn : notnull
			where TOut : notnull
		{
			var task = PostAsync<TIn, TOut>(input, address);
			task.Start();
			task.Wait();
			return task.Result;
		}

        /// <summary>
        /// Send a POST request to a given <paramref name="address"/> asynchronously
        /// </summary>
        /// <typeparam name="TOut">What to deserialize the response as</typeparam>
        /// <param name="address">Target URL address</param>
        /// <returns>An instance of <typeparamref name="TOut"/></returns>
        public async Task<TOut> PostAsync<TOut>(string address) where TOut : notnull => await PostAsync<EmptyModel, TOut>(new EmptyModel(), address);
        /// <summary>
        /// Send a POST request to a given <paramref name="address"/> asynchronously
        /// </summary>
        /// <typeparam name="TIn">What to serialize the request as</typeparam>
        /// <typeparam name="TOut">What to deserialize the response as</typeparam>
        /// <param name="input">Input model to serialize</param>
        /// <param name="address">Target URL address</param>
        /// <returns>An instance of <typeparamref name="TOut"/></returns>
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
        #endregion

        #region PATCH
		/// <summary>
		/// Send a PATCH request to a given <paramref name="address"/>
		/// </summary>
		/// <typeparam name="TOut">What to deserialize the response as</typeparam>
		/// <param name="address">Target URL address</param>
		/// <returns>An instance of <typeparamref name="TOut"/></returns>
        public TOut Patch<TOut>(string address) where TOut : notnull => Patch<EmptyModel, TOut>(new EmptyModel(), address);
        /// <summary>
        /// Send a PATCH request to a given <paramref name="address"/>
        /// </summary>
        /// <typeparam name="TIn">What to serialize the request as</typeparam>
        /// <typeparam name="TOut">What to deserialize the response as</typeparam>
        /// <param name="input">Input model to serialize</param>
        /// <param name="address">Target URL address</param>
        /// <returns>An instance of <typeparamref name="TOut"/></returns>
        public TOut Patch<TIn, TOut>(TIn input, string address)
			where TIn : notnull
			where TOut : notnull
		{
			var task = PatchAsync<TIn, TOut>(input, address);
			task.Start();
			task.Wait();
			return task.Result;
		}

        /// <summary>
        /// Send a PATCH request to a given <paramref name="address"/> asynchronously
        /// </summary>
        /// <typeparam name="TOut">What to deserialize the response as</typeparam>
        /// <param name="address">Target URL address</param>
        /// <returns>An instance of <typeparamref name="TOut"/></returns>
        public async Task<TOut> PatchAsync<TOut>(string address) where TOut : notnull => await PatchAsync<EmptyModel, TOut>(new EmptyModel(), address);
        /// <summary>
        /// Send a PATCH request to a given <paramref name="address"/> asynchronously
        /// </summary>
        /// <typeparam name="TIn">What to serialize the request as</typeparam>
        /// <typeparam name="TOut">What to deserialize the response as</typeparam>
        /// <param name="input">Input model to serialize</param>
        /// <param name="address">Target URL address</param>
        /// <returns>An instance of <typeparamref name="TOut"/></returns>
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
        #endregion

        #region GET
        /// <summary>
        /// Send a GET request to a given <paramref name="address"/>
        /// </summary>
        /// <typeparam name="TOut">What to deserialize the response as</typeparam>
        /// <param name="address">Target URL address</param>
        /// <returns>An instance of <typeparamref name="TOut"/></returns>
        public TOut Get<TOut>(string address) where TOut : notnull => Get<EmptyModel, TOut>(new EmptyModel(), address);
        /// <summary>
        /// Send a GET request to a given <paramref name="address"/>
        /// </summary>
        /// <typeparam name="TIn">What to serialize the request as</typeparam>
        /// <typeparam name="TOut">What to deserialize the response as</typeparam>
        /// <param name="input">Input model to serialize</param>
        /// <param name="address">Target URL address</param>
        /// <returns>An instance of <typeparamref name="TOut"/></returns>
        public TOut Get<TIn, TOut>(TIn input, string address)
			where TIn : notnull
			where TOut : notnull
		{
			var task = GetAsync<TIn, TOut>(input, address);
			task.Start();
			task.Wait();
			return task.Result;
		}

        /// <summary>
        /// Send a GET request to a given <paramref name="address"/> asynchronously
        /// </summary>
        /// <typeparam name="TOut">What to deserialize the response as</typeparam>
        /// <param name="address">Target URL address</param>
        /// <returns>An instance of <typeparamref name="TOut"/></returns>
        public async Task<TOut> GetAsync<TOut>(string address) where TOut : notnull => await GetAsync<EmptyModel, TOut>(new EmptyModel(), address);
        /// <summary>
        /// Send a GET request to a given <paramref name="address"/> asynchronously
        /// </summary>
        /// <typeparam name="TIn">What to serialize the request as</typeparam>
        /// <typeparam name="TOut">What to deserialize the response as</typeparam>
        /// <param name="input">Input model to serialize</param>
        /// <param name="address">Target URL address</param>
        /// <returns>An instance of <typeparamref name="TOut"/></returns>
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
        #endregion

        #region DELETE
        /// <summary>
        /// Send a DELETE request to a given <paramref name="address"/>
        /// </summary>
        /// <typeparam name="TOut">What to deserialize the response as</typeparam>
        /// <param name="address">Target URL address</param>
        /// <returns>An instance of <typeparamref name="TOut"/></returns>
        public TOut Delete<TOut>(string address) where TOut : notnull => Delete<EmptyModel, TOut>(new EmptyModel(), address);
        /// <summary>
        /// Send a DELETE request to a given <paramref name="address"/>
        /// </summary>
        /// <typeparam name="TIn">What to serialize the request as</typeparam>
        /// <typeparam name="TOut">What to deserialize the response as</typeparam>
        /// <param name="input">Input model to serialize</param>
        /// <param name="address">Target URL address</param>
        /// <returns>An instance of <typeparamref name="TOut"/></returns>
        public TOut Delete<TIn, TOut>(TIn input, string address)
			where TIn : notnull
			where TOut : notnull
		{
			var task = DeleteAsync<TIn, TOut>(input, address);
			task.Start();
			task.Wait();
			return task.Result;
		}

        /// <summary>
        /// Send a DELETE request to a given <paramref name="address"/> asynchronously
        /// </summary>
        /// <typeparam name="TOut">What to deserialize the response as</typeparam>
        /// <param name="address">Target URL address</param>
        /// <returns>An instance of <typeparamref name="TOut"/></returns>
        public async Task<TOut> DeleteAsync<TOut>(string address) where TOut : notnull => await DeleteAsync<EmptyModel, TOut>(new EmptyModel(), address);
        /// <summary>
        /// Send a DELETE request to a given <paramref name="address"/> asynchronously
        /// </summary>
        /// <typeparam name="TIn">What to serialize the request as</typeparam>
        /// <typeparam name="TOut">What to deserialize the response as</typeparam>
        /// <param name="input">Input model to serialize</param>
        /// <param name="address">Target URL address</param>
        /// <returns>An instance of <typeparamref name="TOut"/></returns>
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
        #endregion

        private class EmptyModel
		{

		}
	}
}
