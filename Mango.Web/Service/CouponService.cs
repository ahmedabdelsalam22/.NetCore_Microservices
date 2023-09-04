using Mango.Web.Models;
using Mango.Web.Service.IService;

namespace Mango.Web.Service
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;

        public CouponService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public Task<ResponseDTO?> CreateCoupon(CouponDTO couponDTO)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO?> DeleteCoupon(int couponId)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO?> GetAllCoupons()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO?> GetCouponByCoupnCode(string couponCode)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO?> GetCouponById(int couponId)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO?> UpdateCoupon(CouponDTO couponDTO)
        {
            throw new NotImplementedException();
        }
    }
}
