using AutoMapper;
using Mango.Services.ShoppingCartAPI.Data;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.Dtos;
using Mango.Services.ShoppingCartAPI.Models.DTOS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ShoppingCartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private IMapper _mapper;
        private ResponseDTO _responseDTO;

        public CartAPIController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _responseDTO = new();
        }

        [HttpPost("cartUpdsert")]
        public async Task<ResponseDTO> CartUpsert(CartDto cartDto)
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
                    var cartDetailsFromDb = await _context.CartDetails.FirstOrDefaultAsync(x=>x.ProductId == 
                                       cartDto.CartDetailsDtos.First().ProductId && x.CartHeaderId == cartHeaderFromDb.CartHeaderId);

                    if (cartDetailsFromDb == null)
                    {
                        // create cartDetails 
                    }
                    else {
                        // update count in cartDetails
                    }

                }

            }catch(Exception ex) 
            {
                _responseDTO.Message = ex.ToString();
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }
    }
}
