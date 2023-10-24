using Mango.Services.OrderAPI.Models.Dtos;
using Mango.Services.OrderAPI.Services.IServices;
using Mango.Services.OrderAPI.Utility;
using RestSharp;
using System;

namespace Mango.Services.OrderAPI.Services
{
    public class ProductRestService : IProductRestService
    {
        private readonly RestClient restClient;

        public ProductRestService()
        {
            restClient = new RestClient(SD.ProductAPIUrl);
        }

        public async Task<List<ProductDto>> GetAllProducts()
        {
            var request = new RestRequest("/api/products", Method.Get);

            var response = await restClient.ExecuteGetAsync<List<ProductDto>>(request);

            if (response.Data == null)
            {
                Console.WriteLine($"ERROR: {response.ErrorException?.Message}");
            }

            return response.Data!;
        }
    }
}
