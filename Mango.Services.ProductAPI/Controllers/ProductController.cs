using AutoMapper;
using Mango.Services.CouponAPI.Repository.IRepository;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductController(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        [HttpGet("products")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllProducts() 
        {
            try
            {
                IEnumerable<Product> products = await _productRepository.GetAll(tracked:false);

                if (products == null)
                {
                    return NotFound("no data found");
                }

                List<ProductDto> productDtos = _mapper.Map<List<ProductDto>>(products);
                return Ok(productDtos);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("product/{id}")]
        public async Task<IActionResult> GetProductById(int? id) 
        {
            try 
            {
                if (id == 0 || id == null)
                {
                    return BadRequest("no data found with this id");
                }
                Product product = await _productRepository.Get(filter: x => x.ProductId == id, tracked: false);

                if (product == null)
                {
                    return NotFound("no data found");
                }

                ProductDto productDto = _mapper.Map<ProductDto>(product);
                return Ok(productDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
