using AutoMapper;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.DTOS;
using Mango.Services.CouponAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponAPIController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly APIResponse apiResponse;
        public CouponAPIController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.apiResponse = new APIResponse();
        }

        [HttpGet("coupons")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetAllCoupons() 
        {
            List<Coupon> coupons = await unitOfWork.couponRepository.GetAll(tracked:false);
            if (coupons == null) 
            {
                return NotFound();
            }
            List<CouponDTO> couponDTOs = mapper.Map<List<CouponDTO>>(coupons);
            return Ok(couponDTOs);
        }

    }
}
