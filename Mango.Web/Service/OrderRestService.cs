using Mango.Web.Models.Dtos;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using RestSharp;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Policy;
using Newtonsoft.Json;

namespace Mango.Web.Service
{
    public class OrderRestService : IOrderRestService
    {
        private readonly RestClient _restClient;
        private readonly ITokenProvider _tokenProvider;

        public OrderRestService(ITokenProvider tokenProvider)
        {
            _restClient = new RestClient(SD.OrderAPIBase);
            _tokenProvider = tokenProvider;
        }

        public async Task<OrderHeaderDto> CreateOrder(CartDto cartDto, bool withBearer = true)
        {
            var request = new RestRequest("/api/order/CreateOrder", Method.Post);

            request.AddJsonBody(cartDto);

            request.AddHeader("Accept", "application/json");
            if (withBearer)
            {
                request.AddHeader("Authorization", $"Bearer {_tokenProvider.GetToken()}"); // read token from method parameter
            }

            var resposne = await _restClient.ExecutePostAsync<OrderHeaderDto>(request);

            return resposne.Data;
        }

        public async Task<StripeRequestDto> CreateStripeSession(StripeRequestDto stripeRequestDto, bool withBearer = true)
        {
            var request = new RestRequest("/api/order/CreateStripeSession", Method.Post);

            request.AddJsonBody(stripeRequestDto);

            request.AddHeader("Accept", "application/json");
            if (withBearer)
            {
                request.AddHeader("Authorization", $"Bearer {_tokenProvider.GetToken()}"); // read token from method parameter
            }

            var resposne = await _restClient.ExecutePostAsync<StripeRequestDto>(request);

            return resposne.Data;
        }
    }
}
