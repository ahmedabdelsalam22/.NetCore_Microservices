using Mango.Web.Models;
using Mango.Web.Models.DTOS;
using Mango.Web.RestService;
using Mango.Web.RestService.IRestService;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;
        private readonly ICouponRestService _couponRest;

        public CouponController(ICouponService couponService, ICouponRestService couponRest)
        {
            _couponService = couponService;
            _couponRest = couponRest;
        }

        public async Task<IActionResult> CouponIndex()
        {
            //List<CouponDTO>? couponDTOs = new();

            //ResponseDTO? response = await _couponService.GetAllCoupons();
            //if (response!.Result != null && response.IsSuccess)
            //{
            //    couponDTOs = JsonConvert.DeserializeObject<List<CouponDTO>>(Convert.ToString(response.Result)!);
            //}

            List<Coupon> coupons = await _couponRest.GetAsync(url:"/api/couponApi/coupons");

            return View(coupons);
        }

        public IActionResult CreateCoupon() 
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCoupon(CouponDTO coupon)
        {
            if (ModelState.IsValid)
            {
                ResponseDTO? response = await _couponService.CreateCoupon(coupon);
                if (response.IsSuccess)
                {
                    return RedirectToAction(nameof(CouponIndex));
                }
            }
            return View(coupon);
        }

        public async Task<IActionResult> DeleteCoupon(int couponId) 
        {
            ResponseDTO? response = await _couponService.DeleteCoupon(couponId);
            if (response.IsSuccess)
            {
                return RedirectToAction(nameof(CouponIndex));
            }
            return RedirectToAction(nameof(CouponIndex));
        }
    }
}
