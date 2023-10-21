using Mango.Services.ShoppingCartAPI.Models.Dtos;
using Mango.Services.ShoppingCartAPI.Services.IServices;
using Mango.Services.ShoppingCartAPI.Utility;
using RestSharp;

namespace Mango.Services.ShoppingCartAPI.Services
{
    public class CouponRestService : ICouponRestService
    {
        private readonly RestClient restClient;

        public CouponRestService()
        {
            restClient = new RestClient(SD.CouponAPIUrl);
        }

        public async Task<CouponDto> GetCouponByCouponCode(string couponCode)
        {
            var request = new RestRequest($"/api/couponapi/couponCode/{couponCode}", Method.Get);

            var response = await restClient.ExecuteGetAsync<CouponDto>(request);

            if (response.Data == null)
            {
                Console.WriteLine($"ERROR: {response.ErrorException?.Message}");
            }

            return response.Data!;
        }
    }
}
