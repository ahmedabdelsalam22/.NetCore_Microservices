using AutoMapper;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.DTOS;
using Mango.Services.CouponAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
            try
            {
                List<Coupon> coupons = await unitOfWork.couponRepository.GetAll(tracked: false);
                if (coupons == null)
                {
                    return NotFound();
                }
                List<CouponDTO> couponDTOs = mapper.Map<List<CouponDTO>>(coupons);

                apiResponse.IsSuccess = true;
                apiResponse.StatusCode = HttpStatusCode.OK;
                apiResponse.Result = couponDTOs;
                return apiResponse;
            }
            catch (Exception ex) 
            {
                apiResponse.IsSuccess = false;
                apiResponse.StatusCode = HttpStatusCode.BadRequest;
                apiResponse.ErrorMessage = new List<string>() { ex.Message };
                return apiResponse;
            }
        }

    }
}
