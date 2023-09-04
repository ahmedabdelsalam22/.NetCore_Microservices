using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface ICouponService
    {
        Task<ResponseDTO> GetCouponById(int couponId);
        Task<ResponseDTO> GetCouponByCoupnCode(string couponCode);
        Task<ResponseDTO> GetAllCoupons();
        Task<ResponseDTO> CreateCoupon(CouponDTO couponDTO);
        Task<ResponseDTO> UpdateCoupon(CouponDTO couponDTO);
        Task<ResponseDTO> DeleteCoupon(int couponId);
    }
}
