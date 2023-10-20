using AutoMapper;
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
        private readonly IProductRestService _restService;
        private readonly IMapper _mapper;

        public HomeController(IProductRestService restService, IMapper mapper)
        {
            _restService = restService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> products = await _restService.GetAsync(url: $"{SD.ProductAPIBase}/api/products");

            List<ProductDto> productsDto = _mapper.Map<List<ProductDto>>(products);

            return View(productsDto);
        }
        [Authorize]
        public async Task<IActionResult> ProductDetails(int productId) 
        {
            Product product = await _restService.GetByIdAsync(url: $"{SD.ProductAPIBase}/api/product/{productId}");

            ProductDto productDto = _mapper.Map<ProductDto>(product);

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