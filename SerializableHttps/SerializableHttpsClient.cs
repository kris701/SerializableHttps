using SerializableHttps.AuthenticationMethods;
using SerializableHttps.Exceptions;
using SerializableHttps.Models;
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

		/// <summary>
		/// Automatically add 'set-cookie' responses to the header
		/// </summary>
		public bool AutoAddCookies { get; set; } = false;

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
        public TOut? Post<TOut>(string address) => Post<EmptyModel, TOut>(null, address);
        /// <summary>
        /// Send a POST request to a given <paramref name="address"/>
        /// </summary>
        /// <typeparam name="TIn">What to serialize the request as</typeparam>
        /// <typeparam name="TOut">What to deserialize the response as</typeparam>
        /// <param name="input">Input model to serialize</param>
        /// <param name="address">Target URL address</param>
        /// <returns>An instance of <typeparamref name="TOut"/></returns>
        public TOut? Post<TIn, TOut>(TIn? input, string address)
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
        public async Task<TOut?> PostAsync<TOut>(string address) => await PostAsync<EmptyModel, TOut>(null, address);
        /// <summary>
        /// Send a POST request to a given <paramref name="address"/> asynchronously
        /// </summary>
        /// <typeparam name="TIn">What to serialize the request as</typeparam>
        /// <typeparam name="TOut">What to deserialize the response as</typeparam>
        /// <param name="input">Input model to serialize</param>
        /// <param name="address">Target URL address</param>
        /// <returns>An instance of <typeparamref name="TOut"/></returns>
        public async Task<TOut?> PostAsync<TIn, TOut>(TIn? input, string address)
		{
			var content = BodySerialiser.SerializeContent(input);
			var response = await _client.PostAsync(address, content);
			if (AutoAddCookies)
				UpdateRequestCookie(response);
			if (!IsStatusCodeOK(response))
				throw new HttpGeneralException($"Server did not respond with an OK! Response code: {response.StatusCode}", response.StatusCode, await response.Content.ReadAsStringAsync());
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
        public TOut? Patch<TOut>(string address) => Patch<EmptyModel, TOut>(null, address);
        /// <summary>
        /// Send a PATCH request to a given <paramref name="address"/>
        /// </summary>
        /// <typeparam name="TIn">What to serialize the request as</typeparam>
        /// <typeparam name="TOut">What to deserialize the response as</typeparam>
        /// <param name="input">Input model to serialize</param>
        /// <param name="address">Target URL address</param>
        /// <returns>An instance of <typeparamref name="TOut"/></returns>
        public TOut? Patch<TIn, TOut>(TIn? input, string address)
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
        public async Task<TOut?> PatchAsync<TOut>(string address) => await PatchAsync<EmptyModel, TOut>(null, address);
        /// <summary>
        /// Send a PATCH request to a given <paramref name="address"/> asynchronously
        /// </summary>
        /// <typeparam name="TIn">What to serialize the request as</typeparam>
        /// <typeparam name="TOut">What to deserialize the response as</typeparam>
        /// <param name="input">Input model to serialize</param>
        /// <param name="address">Target URL address</param>
        /// <returns>An instance of <typeparamref name="TOut"/></returns>
        public async Task<TOut?> PatchAsync<TIn, TOut>(TIn? input, string address)
		{
			var content = BodySerialiser.SerializeContent(input);
			var response = await _client.PatchAsync(address, content);
			if (AutoAddCookies)
				UpdateRequestCookie(response);
			if (!IsStatusCodeOK(response))
				throw new HttpGeneralException($"Server did not respond with an OK! Response code: {response.StatusCode}", response.StatusCode, await response.Content.ReadAsStringAsync());
			return await BodySerialiser.DeserializeContentAsync<TOut>(response.Content);
		}
		#endregion

		#region PUT
		/// <summary>
		/// Send a PUT request to a given <paramref name="address"/>
		/// </summary>
		/// <typeparam name="TOut">What to deserialize the response as</typeparam>
		/// <param name="address">Target URL address</param>
		/// <returns>An instance of <typeparamref name="TOut"/></returns>
		public TOut? Put<TOut>(string address) => Put<EmptyModel, TOut>(null, address);
		/// <summary>
		/// Send a PUT request to a given <paramref name="address"/>
		/// </summary>
		/// <typeparam name="TIn">What to serialize the request as</typeparam>
		/// <typeparam name="TOut">What to deserialize the response as</typeparam>
		/// <param name="input">Input model to serialize</param>
		/// <param name="address">Target URL address</param>
		/// <returns>An instance of <typeparamref name="TOut"/></returns>
		public TOut? Put<TIn, TOut>(TIn? input, string address)
		{
			var task = PutAsync<TIn, TOut>(input, address);
			task.Start();
			task.Wait();
			return task.Result;
		}

		/// <summary>
		/// Send a PUT request to a given <paramref name="address"/> asynchronously
		/// </summary>
		/// <typeparam name="TOut">What to deserialize the response as</typeparam>
		/// <param name="address">Target URL address</param>
		/// <returns>An instance of <typeparamref name="TOut"/></returns>
		public async Task<TOut?> PutAsync<TOut>(string address) => await PutAsync<EmptyModel, TOut>(null, address);
		/// <summary>
		/// Send a PUT request to a given <paramref name="address"/> asynchronously
		/// </summary>
		/// <typeparam name="TIn">What to serialize the request as</typeparam>
		/// <typeparam name="TOut">What to deserialize the response as</typeparam>
		/// <param name="input">Input model to serialize</param>
		/// <param name="address">Target URL address</param>
		/// <returns>An instance of <typeparamref name="TOut"/></returns>
		public async Task<TOut?> PutAsync<TIn, TOut>(TIn? input, string address)
		{
			var content = BodySerialiser.SerializeContent(input);
			var response = await _client.PutAsync(address, content);
			if (AutoAddCookies)
				UpdateRequestCookie(response);
			if (!IsStatusCodeOK(response))
				throw new HttpGeneralException($"Server did not respond with an OK! Response code: {response.StatusCode}", response.StatusCode, await response.Content.ReadAsStringAsync());
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
		public TOut? Get<TOut>(string address) => Get<EmptyModel, TOut>(null, address);
        /// <summary>
        /// Send a GET request to a given <paramref name="address"/>
        /// </summary>
        /// <typeparam name="TIn">What to serialize the request as</typeparam>
        /// <typeparam name="TOut">What to deserialize the response as</typeparam>
        /// <param name="input">Input model to serialize</param>
        /// <param name="address">Target URL address</param>
        /// <returns>An instance of <typeparamref name="TOut"/></returns>
        public TOut? Get<TIn, TOut>(TIn? input, string address)
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
        public async Task<TOut?> GetAsync<TOut>(string address) => await GetAsync<EmptyModel, TOut>(null, address);
        /// <summary>
        /// Send a GET request to a given <paramref name="address"/> asynchronously
        /// </summary>
        /// <typeparam name="TIn">What to serialize the request as</typeparam>
        /// <typeparam name="TOut">What to deserialize the response as</typeparam>
        /// <param name="input">Input model to serialize</param>
        /// <param name="address">Target URL address</param>
        /// <returns>An instance of <typeparamref name="TOut"/></returns>
        public async Task<TOut?> GetAsync<TIn, TOut>(TIn? input, string address)
		{
			address += HeaderSerialiser.QuerryfiModel(input);
			var response = await _client.GetAsync(address);
			if (AutoAddCookies)
				UpdateRequestCookie(response);
			if (!IsStatusCodeOK(response))
				throw new HttpGeneralException($"Server did not respond with an OK! Response code: {response.StatusCode}", response.StatusCode, await response.Content.ReadAsStringAsync());
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
        public TOut? Delete<TOut>(string address) => Delete<EmptyModel, TOut>(null, address);
        /// <summary>
        /// Send a DELETE request to a given <paramref name="address"/>
        /// </summary>
        /// <typeparam name="TIn">What to serialize the request as</typeparam>
        /// <typeparam name="TOut">What to deserialize the response as</typeparam>
        /// <param name="input">Input model to serialize</param>
        /// <param name="address">Target URL address</param>
        /// <returns>An instance of <typeparamref name="TOut"/></returns>
        public TOut? Delete<TIn, TOut>(TIn? input, string address)
		{
			var task = DeleteAsync<TIn, TOut>(input, address);
			task.Start();
			task.Wait();
			return task.Result;
		}

		/// <summary>
		/// Send a DELETE request to a given <paramref name="address"/> asynchronously
		/// </summary>
		/// <param name="address">Target URL address</param>
		public async Task DeleteAsync(string address) => await DeleteAsync<EmptyModel, EmptyModel>(null, address);
		/// <summary>
		/// Send a DELETE request to a given <paramref name="address"/> asynchronously
		/// </summary>
		/// <typeparam name="TOut">What to deserialize the response as</typeparam>
		/// <param name="address">Target URL address</param>
		/// <returns>An instance of <typeparamref name="TOut"/></returns>
		public async Task<TOut?> DeleteAsync<TOut>(string address) => await DeleteAsync<EmptyModel, TOut>(null, address);
        /// <summary>
        /// Send a DELETE request to a given <paramref name="address"/> asynchronously
        /// </summary>
        /// <typeparam name="TIn">What to serialize the request as</typeparam>
        /// <typeparam name="TOut">What to deserialize the response as</typeparam>
        /// <param name="input">Input model to serialize</param>
        /// <param name="address">Target URL address</param>
        /// <returns>An instance of <typeparamref name="TOut"/></returns>
        public async Task<TOut?> DeleteAsync<TIn, TOut>(TIn? input, string address)
		{
			address += HeaderSerialiser.QuerryfiModel(input);
			var response = await _client.DeleteAsync(address);
			if (AutoAddCookies)
				UpdateRequestCookie(response);
			if (!IsStatusCodeOK(response))
				throw new HttpGeneralException($"Server did not respond with an OK! Response code: {response.StatusCode}", response.StatusCode, await response.Content.ReadAsStringAsync());
			return await BodySerialiser.DeserializeContentAsync<TOut>(response.Content);
		}
        #endregion

        private bool IsStatusCodeOK(HttpResponseMessage response)
        {
            return response.StatusCode == System.Net.HttpStatusCode.OK || 
                response.StatusCode == System.Net.HttpStatusCode.Created ||
				response.StatusCode == System.Net.HttpStatusCode.Accepted ||
				response.StatusCode == System.Net.HttpStatusCode.NoContent;
		}

		private void UpdateRequestCookie(HttpResponseMessage response)
		{
			var targets = response.Headers.Where(x => x.Key.ToLower() == "set-cookie");
			var cookieHeader = "";
			foreach (var target in targets)
				foreach(var value in target.Value)
					cookieHeader += value + ";";
			if (cookieHeader.EndsWith(';'))
				cookieHeader = cookieHeader.Substring(0, cookieHeader.Length - 1);
			if (cookieHeader != "")
			{
				_client.DefaultRequestHeaders.Remove("Cookie");
				_client.DefaultRequestHeaders.Add("Cookie", cookieHeader);
			}
		}
	}
}
