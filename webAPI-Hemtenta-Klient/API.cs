using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebAPI_Hemtenta
{
    class API
    {
        static HttpClient client = new HttpClient();

       public static Uri ProductAPI = new Uri("https://localhost:44373/api/product");
       public static Uri CategoryAPI = new Uri("https://localhost:44373/api/category");





        public async Task<HttpStatusCode> DeleteResourceAsync(Uri resourceUri,int Id)
        {

           var response = await client.DeleteAsync($"{resourceUri}/{Id}");

           return response.StatusCode;

        }

        public async Task<HttpResponseMessage> PostResourceAsync<T>(Uri resourceUri, T resource)
        {


            HttpResponseMessage response = await client.PostAsJsonAsync(
                resourceUri, resource);

            response.EnsureSuccessStatusCode();


            return response;

        }

        public async Task<HttpResponseMessage> PatchResourceAsync(Uri resourceUri, HttpContent content)
        {


            HttpResponseMessage response = await client.PatchAsync(
                resourceUri, content);

            response.EnsureSuccessStatusCode();


            return response;

        }


        public async Task<List<T>> GetResourceAsync<T>(Uri resourceUri)
        {

            HttpResponseMessage response = await client.GetAsync(resourceUri);

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            List<T> deserializedResource = JsonConvert.DeserializeObject<List<T>>(responseBody);

            return deserializedResource;

        }
    }
}
