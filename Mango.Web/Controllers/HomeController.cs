using AutoMapper;
using IdentityModel;
using Mango.Web.Models;
using Mango.Web.Models.DTOS;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Mango.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRestService _productRestService;
        private readonly IMapper _mapper;
        private readonly ICartRestService _cartRestService;

        public HomeController(IProductRestService restService, IMapper mapper, ICartRestService cartRestService)
        {
            _productRestService = restService;
            _mapper = mapper;
            _cartRestService = cartRestService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> products = await _productRestService.GetAsync(url: $"{SD.ProductAPIBase}/api/products");

            List<ProductDto> productsDto = _mapper.Map<List<ProductDto>>(products);

            return View(productsDto);
        }
        [Authorize]
        public async Task<IActionResult> ProductDetails(int productId) 
        {
            Product product = await _productRestService.GetByIdAsync(url: $"{SD.ProductAPIBase}/api/product/{productId}");

            ProductDto productDto = _mapper.Map<ProductDto>(product);

            return View(productDto);
        }

        [Authorize]
        [HttpPost]
        [ActionName("ProductDetails")]
        public async Task<IActionResult> ProductDetails(ProductDto productDto)
        {
            string url = $"{SD.ShoppingCartAPIBase}/api/cart/cartUpdsert";

            var userId = User.Claims.Where(x => x.Type == JwtClaimTypes.Subject)?.FirstOrDefault()?.Value;

            CartDto cartDto = new() 
            {
                CartHeaderDto = new CartHeaderDto { UserId = userId}
            };

            CartDetailsDto cartDetailsDto = new() 
            {
                Count = productDto.Count,
                ProductId = productDto.ProductId
            };

            List<CartDetailsDto> listOfCartDetailsDtos = new List<CartDetailsDto>() {cartDetailsDto};

            cartDto.CartDetailsDtos = listOfCartDetailsDtos;

            var response = await _cartRestService.PostAsync(url: url, data: cartDto);

            if (response.IsSuccessful)
            {
                TempData["success"] = "item has been added to cart";
                return RedirectToAction(nameof(Index));
            }
            else 
            {
                TempData["error"] = response.ErrorMessage;
            }
            return View(productDto);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}