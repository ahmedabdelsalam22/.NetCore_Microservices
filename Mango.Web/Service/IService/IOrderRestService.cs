using Mango.Web.Models.Dtos;
using Mango.Web.RestService.IRestService;
using RestSharp;

namespace Mango.Web.Service.IService
{
    public interface IOrderRestService 
    {
        Task<OrderHeaderDto> CreateOrder(CartDto cartDto, bool withBearer = true);
        Task<StripeRequestDto> CreateStripeSession(StripeRequestDto stripeRequestDto, bool withBearer = true);
        Task<OrderHeaderDto> ValidateStripeSession(int orderHeaderId, bool withBearer = true);
    }
}
