using AutoMapper;
using Mango.Web.Models;
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

            return View(products);
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
    }
}
