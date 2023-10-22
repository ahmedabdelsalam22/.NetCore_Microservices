using Mango.Web.Models.DTOS;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Mango.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartRestService _cartRestService;

        public CartController(ICartRestService cartRestService)
        {
            _cartRestService = cartRestService;
        }
        [Authorize]
        public async Task<IActionResult> CartIndex()
        {
            var cartDto = await GetCartBasedInLoggerUser();
            return View(cartDto);
        }

        public async Task<CartDto> GetCartBasedInLoggerUser()
        {
            var userId = User.Claims.Where(x=>x.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;

            string url = $"{SD.ShoppingCartAPIBase}/api/cart/getCart/{userId}";

            var response = await _cartRestService.GetByIdAsync(url: url);
            return response;
        }

        public async Task<IActionResult> Remove(int cartDetailsId)
        {

            string url = $"{SD.ShoppingCartAPIBase}/api/cart/RemoveCart";

            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            var response = await _cartRestService.PostToDeleteCart(url:url , cartDetailsId: cartDetailsId);
            if (response.IsSuccessful)
            {
                TempData["success"] = "Cart updated successfully";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        {
            string url = $"{SD.ShoppingCartAPIBase}/api/cart/ApplyCoupon";

            var response = await _cartRestService.PostAsync(url: url, data: cartDto);
            if (response.IsSuccessful)
            {
                TempData["success"] = "Cart updated successfully";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
        {
            string url = $"{SD.ShoppingCartAPIBase}/api/cart/ApplyCoupon";

            cartDto.CartHeaderDto.CouponCode = "";
            var response = await _cartRestService.PostAsync(url: url, data: cartDto);
            if (response.IsSuccessful)
            {
                TempData["success"] = "Cart updated successfully";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EmailCart()
        {
            CartDto cartDto = await GetCartBasedInLoggerUser();

            cartDto.CartHeaderDto.Email = User.Claims.Where(x => x.Type == JwtRegisteredClaimNames.Email)?.FirstOrDefault()?.Value;


            string url = $"{SD.ShoppingCartAPIBase}/api/cart/EmailCartRequest";
            var response = await _cartRestService.PostAsync(url: url, data: cartDto);
            if (response.IsSuccessful)
            {
                TempData["success"] = "Email will be processed and sent shortly";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }
    }
}
