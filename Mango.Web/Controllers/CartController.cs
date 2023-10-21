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
    }
}
