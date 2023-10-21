using AutoMapper;
using Azure;
using Mango.Services.ShoppingCartAPI.Data;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.Dtos;
using Mango.Services.ShoppingCartAPI.Models.DTOS;
using Mango.Services.ShoppingCartAPI.Services.IServices;
using Mango.Services.ShoppingCartAPI.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;

namespace Mango.Services.ShoppingCartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private IMapper _mapper;
        private readonly IProductRestService _productRestService;
        private readonly ICouponRestService _couponRestService;
        public CartAPIController(ApplicationDbContext context, IMapper mapper, IProductRestService productRestService,ICouponRestService couponRestService)
        {
            _context = context;
            _mapper = mapper;
            _productRestService = productRestService;
            _couponRestService = couponRestService;
        }


        [HttpGet("getCart/{userId}")]
        public async Task<ActionResult> GetCart(string userId)
        {
            try 
            {
                CartDto cart = new()
                {
                    CartHeaderDto = _mapper.Map<CartHeaderDto>(_context.CartHeaders.First(u => u.UserId == userId))
                };
                cart.CartDetailsDtos = _mapper.Map<IEnumerable<CartDetailsDto>>(_context.CartDetails
                    .Where(u => u.CartHeaderId == cart.CartHeaderDto.CartHeaderId));

                List<ProductDto> productDtos = await _productRestService.GetAllProducts();


                foreach (var item in cart.CartDetailsDtos)
                {
                    item.Product = productDtos.FirstOrDefault(u => u.ProductId == item.ProductId);
                    cart.CartHeaderDto.CartTotal += (item.Count * item.Product.Price); // here we should get product from "ProductAPI" which means microservices. 
                }

                //apply coupon if any
                //if (!string.IsNullOrEmpty(cart.CartHeaderDto.CouponCode))
                //{
                //    CouponDto coupon = await _couponRestService.GetCoupon(cart.CartHeaderDto.CouponCode);
                //    if (coupon != null && cart.CartHeaderDto.CartTotal > coupon.MinAmount)
                //    {
                //        cart.CartHeaderDto.CartTotal -= coupon.DiscountAmount;
                //        cart.CartHeaderDto.Discount = coupon.DiscountAmount;
                //    }
                //}

                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost("cartUpdsert")]
        public async Task<IActionResult> CartUpsert(CartDto cartDto)
        {
            try 
            {
                var cartHeaderFromDb = await _context.CartHeaders.FirstOrDefaultAsync(x=>x.UserId == cartDto.CartHeaderDto.UserId);
                if (cartHeaderFromDb == null)
                {
                    // create cartHeader and details 
                    CartHeader cartHeader = _mapper.Map<CartHeader>(cartDto.CartHeaderDto);
                    await _context.CartHeaders.AddAsync(cartHeader);
                    await _context.SaveChangesAsync();

                    cartDto.CartDetailsDtos.First().CartHeaderId = cartHeader.CartHeaderId;

                    await _context.CartDetails.AddAsync(_mapper.Map<CartDetails>(cartDto.CartDetailsDtos.First()));
                    await _context.SaveChangesAsync();

                }
                else {
                    // check if details has the same product
                    var cartDetailsFromDb = await _context.CartDetails.AsNoTracking().FirstOrDefaultAsync(x=>x.ProductId == 
                                       cartDto.CartDetailsDtos.First().ProductId && x.CartHeaderId == cartHeaderFromDb.CartHeaderId);

                    if (cartDetailsFromDb == null)
                    {
                        // create cartDetails 
                        cartDto.CartDetailsDtos.First().CartHeaderId = cartHeaderFromDb.CartHeaderId;

                        await _context.CartDetails.AddAsync(_mapper.Map<CartDetails>(cartDto.CartDetailsDtos.First()));
                        await _context.SaveChangesAsync();
                    }
                    else {
                        // update count in cartDetails
                        cartDto.CartDetailsDtos.First().Count += cartDetailsFromDb.Count;
                        cartDto.CartDetailsDtos.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                        cartDto.CartDetailsDtos.First().CartDetailsId = cartDetailsFromDb.CartDetailsId;

                        _context.CartDetails.Update(_mapper.Map<CartDetails>(cartDto.CartDetailsDtos.First()));
                        await _context.SaveChangesAsync();
                    }
                }
                return Ok(cartDto);
            }
            catch(Exception ex) 
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost("RemoveCart")]
        public async Task<IActionResult> RemoveCart([FromBody]int cartDetailsId)
        {
            try
            {
                CartDetails cartDetails = await _context.CartDetails.FirstAsync(x=>x.CartDetailsId == cartDetailsId);

                int totalCountofCartItem = _context.CartDetails.Where(u => u.CartHeaderId == cartDetails.CartHeaderId).Count();

                _context.CartDetails.Remove(cartDetails);

                if (totalCountofCartItem == 1)
                {
                    var cartHeaderToRemove = await _context.CartHeaders
                       .FirstOrDefaultAsync(u => u.CartHeaderId == cartDetails.CartHeaderId);

                    _context.CartHeaders.Remove(cartHeaderToRemove);
                }
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost("ApplyCoupon")]
        public async Task<object> ApplyCoupon([FromBody] CartDto cartDto)
        {
            try
            {
                var cartFromDb = await _context.CartHeaders.FirstAsync(u => u.UserId == cartDto.CartHeaderDto.UserId);
                cartFromDb.CouponCode = cartDto.CartHeaderDto.CouponCode;
                _context.CartHeaders.Update(cartFromDb);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        [HttpPost("RemoveCoupon")]
        public async Task<object> RemoveCoupon([FromBody] CartDto cartDto)
        {
            try
            {
                var cartFromDb = await _context.CartHeaders.FirstAsync(u => u.UserId == cartDto.CartHeaderDto.UserId);
                cartFromDb.CouponCode = "";
                _context.CartHeaders.Update(cartFromDb);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

    }
}
