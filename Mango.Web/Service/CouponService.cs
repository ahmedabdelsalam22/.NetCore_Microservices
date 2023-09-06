using Mango.Web.Models.DTOS;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using static Mango.Web.Utility.SD;

namespace Mango.Web.Service
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;

        public CouponService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDTO?> CreateCoupon(CouponDTO couponDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = ApiType.POST,
                Url = SD.CouponAPIBase+"/api/couponApi/create",
                Data = couponDTO
            });
        }

        public async Task<ResponseDTO?> DeleteCoupon(int couponId)
        {
            return await _baseService.SendAsync(new RequestDTO() 
            {
                ApiType = ApiType.DELETE,
                Url = SD.CouponAPIBase+$"/api/couponApi/delete/{couponId}"
            });
        }

        public async Task<ResponseDTO?> GetAllCoupons()
        {
            return await _baseService.SendAsync(
                new RequestDTO()
                {
                    ApiType = ApiType.GET,
                    Url = SD.CouponAPIBase+"/api/couponApi/coupons"
                }
            ); 
        }

        public async Task<ResponseDTO?> GetCouponByCoupnCode(string couponCode)
        {
            return await _baseService.SendAsync(
                new RequestDTO()
                {
                    ApiType = ApiType.GET,
                    Url = SD.CouponAPIBase + $"/api/couponApi/couponCode/{couponCode}"
                }
            );
        }

        public async Task<ResponseDTO?> GetCouponById(int couponId)
        {
            return await _baseService.SendAsync(
                new RequestDTO()
                {
                    ApiType = ApiType.GET,
                    Url = SD.CouponAPIBase + $"/api/couponApi/{couponId}"
                }
            );
        }

        public async Task<ResponseDTO?> UpdateCoupon(CouponDTO couponDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = ApiType.PUT,
                Url = SD.CouponAPIBase + $"api/couponApi/update",
                Data = couponDTO
            });
        }
    }
}
