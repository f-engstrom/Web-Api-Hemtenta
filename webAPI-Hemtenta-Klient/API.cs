using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using static WebAPI_Hemtenta.AuthenticationAndAuthorization;

namespace WebAPI_Hemtenta
{
    class Api
    {
        static HttpClient _client = new HttpClient();

        public static Uri ProductApi = new Uri("https://localhost:44373/api/product");
        public static Uri CategoryApi = new Uri("https://localhost:44373/api/category");
        public static Uri TokenApi = new Uri("https://localhost:44373/api/token");




        public async Task<HttpResponseMessage> DeleteResourceAsync(Uri resourceUri, int id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token.RawData);

            var response = await _client.DeleteAsync($"{resourceUri}/{id}");

            return response;

        }

        public async Task<HttpResponseMessage> PostResourceAsync<T>(Uri resourceUri, T resource)
        {
           

                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token.RawData);


                HttpResponseMessage response = await _client.PostAsJsonAsync(
                    resourceUri, resource);


            return response;



        }

        public async Task<HttpResponseMessage> PatchResourceAsync(Uri resourceUri, HttpContent content)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token.RawData);


            HttpResponseMessage response = await _client.PatchAsync(
                resourceUri, content);



            return response;

        }


        public async Task<T> GetResourceAsync<T>(Uri resourceUri)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token.RawData);


            HttpResponseMessage response = await _client.GetAsync(resourceUri);

            string responseBody = await response.Content.ReadAsStringAsync();

            var deserializedResource = JsonConvert.DeserializeObject<T>(responseBody);

            return deserializedResource;

        }
    }
}
