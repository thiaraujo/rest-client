using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestService.Utils;

namespace RestService.RestClient
{
    public class RequestService<T>
    {
        // Provides a base class for sending HTTP requests and receiving HTTP responses from a resource identified by a URI.
        private readonly HttpClient _client;
        // Url address for endpoint consumption.
        // Change this url to your own endpoint
        private readonly Uri _url = new Uri("https://jsonplaceholder.typicode.com/");

        // Username and password to form the request key - optional
        private const string User = "";
        private const string Passw = "";

        public RequestService()
        {
            // Basic httpclient parameters.
            _client = new HttpClient
            {
                BaseAddress = _url,
                Timeout = TimeSpan.FromSeconds(5000),
                MaxResponseContentBufferSize = 256000
            };

            // If you are going to use the Basic format of authorization or other type, just change the header below.
            if (!string.IsNullOrEmpty(User) && !string.IsNullOrEmpty(Passw))
            {
                var authBasic = $"{User}:{EncriptToBase64.Base64Encode(Passw)}";
                var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authBasic));
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
                // Example of how to add a custom header.
                _client.DefaultRequestHeaders.Add("version", "1.0.0");
            }
        }

        #region Get

        // Function to consume data at endpoint
        public async Task<T> GetAsync(string parans)
        {
            // Parameters are required, it is the url path that will be the complement to the query.
            if (parans == null)
            {
                throw new ArgumentNullException(nameof(parans));
            }

            try
            {
                var response = await _client.GetAsync(parans);

                if (!response.IsSuccessStatusCode) return default(T);

                // The response will be converted to some entity format,
                // the same being passed at the time of instance creation of this service.
                var content = await response.Content.ReadAsStringAsync();
                var item = JsonConvert.DeserializeObject<T>(content);

                return item;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Function to consume data at endpoint
        public async Task<IEnumerable<T>> GetAllAsync(string parans)
        {
            // Parameters are required, it is the url path that will be the complement to the query.
            if (parans == null)
            {
                throw new ArgumentNullException(nameof(parans));
            }

            try
            {
                var response = await _client.GetAsync(parans);

                if (!response.IsSuccessStatusCode) return null;

                // The response will be converted to some entity format,
                // the same being passed at the time of instance creation of this service.
                var content = await response.Content.ReadAsStringAsync();
                var itens = JsonConvert.DeserializeObject<IEnumerable<T>>(content);

                return itens;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Post

        // This post already comes in the format to be sent to the endpoint and finally returns a successful or unsent reply.
        public async Task<bool> PostAsync(string parans, string jsonContent)
        {
            var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                var response = await _client.PostAsync(parans, stringContent);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // This post already comes in the format to be sent to the endpoint and finally returns an entity in response.
        public async Task<T> PostAsyncWithReturnObject(string parans, T jsonContent)
        {
            var jsonData = JsonConvert.SerializeObject(jsonContent);

            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            try
            {
                var response = await _client.PostAsync(parans, stringContent);

                var content = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(content);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Put

        // This put already comes in the format to be sent to the endpoint and finally returns a successful or unsent reply.
        public async Task<bool> PutAsync(string parans, string jsonContent)
        {
            var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                var response = await _client.PutAsync(parans, stringContent);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // This put already comes in the format to be sent to the endpoint and finally returns an entity in response.
        public async Task<T> PutAsyncWithReturnObject(string parans, T jsonContent)
        {
            var jsonData = JsonConvert.SerializeObject(jsonContent);

            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            try
            {
                var response = await _client.PutAsync(parans, stringContent);

                var content = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(content);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Delete

        // Function to remove a record, the url should already come with the id to remove.
        public async Task<bool> DeleteAsync(string parans)
        {
            try
            {
                var response = await _client.DeleteAsync(parans);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Function to remove a record, the url should already come with the id to remove - here return a model
        public async Task<T> DeleteAsyncWithReturnObject(string parans)
        {
            try
            {
                var response = await _client.DeleteAsync(parans);

                var content = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(content);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion
    }
}
