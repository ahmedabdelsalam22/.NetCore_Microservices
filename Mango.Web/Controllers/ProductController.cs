using AutoMapper;
using Mango.Web.Models;
using Mango.Web.Models.Dtos;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Mango.Web.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductRestService _restService;
        private readonly IMapper _mapper;

        public ProductController(IProductRestService restService, IMapper mapper)
        {
            _restService = restService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> products = await _restService.GetAsync(url:$"{SD.ProductAPIBase}/api/products");

            List<ProductDto> productsDto = _mapper.Map<List<ProductDto>>(products);

            return View(productsDto);
        }

        [HttpGet]
        public IActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> CreateProduct(Product product)
        {

            await _restService.PostAsync(url: $"{SD.ProductAPIBase}/api/product/create", data: product);

            return RedirectToAction("Index");
        }
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            await _restService.Delete(url: $"{SD.ProductAPIBase}/api/product/delete/{productId}");
            return RedirectToAction(nameof(Index));
        }
    }
}
