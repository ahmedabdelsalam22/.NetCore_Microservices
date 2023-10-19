﻿using Mango.Web.Models;
using Mango.Web.Models.DTOS;
using Mango.Web.RestService;
using Mango.Web.RestService.IRestService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    [Authorize]
    public class CouponController : Controller
    {
        private readonly ICouponRestService _couponRest;

        public CouponController(ICouponRestService couponRest)
        {
            _couponRest = couponRest;
        }

        public async Task<IActionResult> CouponIndex()
        {
            List<Coupon> coupons = await _couponRest.GetAsync(url:"/api/couponApi/coupons");

            return View(coupons);
        }

        public IActionResult CreateCoupon() 
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles ="ADMIN")]
        public async Task<IActionResult> CreateCoupon(Coupon coupon)
        {
           await _couponRest.PostAsync(url: "/api/couponApi/create" , data:coupon);

            return RedirectToAction("CouponIndex");
        }
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteCoupon(int couponId) 
        {
            await _couponRest.Delete(url: $"/api/couponApi/delete/{couponId}"); 
           return RedirectToAction(nameof(CouponIndex));
        }
    }
}
