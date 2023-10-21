using Mango.Services.ShoppingCartAPI.Services.IServices;
using RestSharp;

namespace Mango.Services.ShoppingCartAPI.Services
{
    public class RestService<T> : IRestService<T> where T : class
    {
        private readonly RestClient restClient;

        public RestService()
        {
            this.restClient = new RestClient();
        }

        public async Task<List<T>> GetAllAsync(string url)
        {
            var request = new RestRequest(url, Method.Get);

            var response = await restClient.ExecuteGetAsync<List<T>>(request);

            if (response.Data == null)
            {
                Console.WriteLine($"ERROR: {response.ErrorException?.Message}");
            }

            return response.Data!;
        }

       
    }
}
