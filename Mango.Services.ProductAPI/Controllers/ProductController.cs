using AutoMapper;
using Mango.Services.CouponAPI.Repository.IRepository;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;

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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        [HttpPut("product/update/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProduct(int id ,[FromBody] ProductUpdateDto productUpdateDto)
        {
            try
            {
                if (ModelState.IsValid) 
                {
                    if (id == 0 || id == null)
                    {
                        return BadRequest("no data found with this id");
                    }
                    
                    Product product = await _productRepository.Get(filter: x => x.ProductId == id, tracked: false);

                    if (product == null)
                    {
                        return BadRequest($"no product found with id: {id}");
                    }


                    Product productToUpdate = _mapper.Map<Product>(productUpdateDto);

                    productToUpdate.ProductId = id;

                    await _productRepository.Update(productToUpdate);

                    return Ok(productToUpdate);
                }
                return BadRequest("model is not valid");
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost("product/create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDto productCreateDto) 
        {

            try
            {
                if (ModelState.IsValid)
                {
                    Product productFromDb = await _productRepository.Get(filter: x => x.Name.ToLower() == productCreateDto.Name.ToLower(), tracked: false);

                    if (productFromDb != null)
                    {
                        return BadRequest("product already exists");
                    }

                    Product productToDb = _mapper.Map<Product>(productCreateDto);
                    await _productRepository.Create(productToDb);
                    return Ok(productToDb);
                }
                return BadRequest("model is not valid");
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpDelete("product/delete/{id}")]
        public async Task<IActionResult> DeleteProduct(int? id) 
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
                    return BadRequest($"no product found with id: {id}");
                }

                await _productRepository.Delete(product);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

    }
}
