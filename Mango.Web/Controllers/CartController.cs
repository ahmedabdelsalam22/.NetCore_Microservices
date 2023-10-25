using Mango.Web.Models.Dtos;
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
        private readonly IOrderRestService _orderRestService;
        public CartController(ICartRestService cartRestService, IOrderRestService orderRestService)
        {
            _cartRestService = cartRestService;
            _orderRestService = orderRestService;
        }
        [Authorize]
        public async Task<IActionResult> CartIndex()
        {
            var cartDto = await GetCartBasedInLoggerUser();
            return View(cartDto);
        }

        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            var cartDto = await GetCartBasedInLoggerUser();
            return View(cartDto);
        }


        [HttpPost]
        [ActionName("Checkout")]
        public async Task<IActionResult> Checkout(CartDto cartDto)
        {
            CartDto cart = await GetCartBasedInLoggerUser();
            cart.CartHeaderDto.Phone = cartDto.CartHeaderDto.Phone;
            cart.CartHeaderDto.Email = cartDto.CartHeaderDto.Email;
            cart.CartHeaderDto.Name = cartDto.CartHeaderDto.Name;

            var response = await _orderRestService.CreateOrder(cart); // the response is "OrderHeaderDto"

            if (response != null)
            {
                //get stripe session and redirect to stripe to place order

                var domain = Request.Scheme + "://" + Request.Host.Value + "/";

                StripeRequestDto stripeRequestDto = new()
                {
                    ApprovedUrl = domain + "cart/Confirmation?orderId=" + response.OrderHeaderId, 
                    CancelUrl = domain + "cart/checkout",
                    OrderHeaderDto = response
                };

                var stripeResponse = await _orderRestService.CreateStripeSession(stripeRequestDto);

                Response.Headers.Add("Location", stripeResponse.StripeSessionUrl);
                return new StatusCodeResult(303);
            }
            return View();
        }

        public async Task<IActionResult> Confirmation(int orderId)
        {
            return View(orderId);
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
