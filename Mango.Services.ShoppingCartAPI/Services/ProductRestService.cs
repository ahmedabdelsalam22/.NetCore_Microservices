using Mango.Services.ShoppingCartAPI.Models.Dtos;
using Mango.Services.ShoppingCartAPI.Services.IServices;
using Mango.Services.ShoppingCartAPI.Utility;
using RestSharp;
using System;

namespace Mango.Services.ShoppingCartAPI.Services
{
    public class ProductRestService : IProductRestService
    {
        private readonly RestClient restClient;

        public ProductRestService()
        {
            restClient = new RestClient(SD.ProductAPIUrl);
        }

        public async Task<List<ProductDto>> GetProducts()
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
