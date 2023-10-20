using AutoMapper;
using Mango.Services.ShoppingCartAPI.Data;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.Dtos;
using Mango.Services.ShoppingCartAPI.Models.DTOS;
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

        public CartAPIController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
    }
}
