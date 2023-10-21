using Mango.Services.ShoppingCartAPI.Models.Dtos;

namespace Mango.Services.ShoppingCartAPI.Services.IServices
{
    public interface ICouponRestService
    {
        Task<CouponDto> GetCouponByCouponCode(string couponCode);
    }
}
