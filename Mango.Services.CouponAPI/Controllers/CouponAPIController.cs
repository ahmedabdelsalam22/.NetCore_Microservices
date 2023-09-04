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
        [HttpGet("{couponId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetCouponById(int? couponId) 
        {
            try
            {
                Coupon coupon = await unitOfWork.couponRepository.Get(tracked: false, filter:x=>x.CouponId == couponId);
                if (coupon == null)
                {
                    return NotFound();
                }
                CouponDTO couponDTO = mapper.Map<CouponDTO>(coupon);

                apiResponse.IsSuccess = true;
                apiResponse.StatusCode = HttpStatusCode.OK;
                apiResponse.Result = couponDTO;
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
        [HttpGet("couponCode/{couponCode}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetCouponByCouponCode(string? couponCode) 
        {
            try
            {
                Coupon coupon = await unitOfWork.couponRepository.Get(tracked: false, filter:x=>x.CouponCode.ToLower() == couponCode.ToLower());
                if (coupon == null)
                {
                    return NotFound();
                }
                CouponDTO couponDTO = mapper.Map<CouponDTO>(coupon);

                apiResponse.IsSuccess = true;
                apiResponse.StatusCode = HttpStatusCode.OK;
                apiResponse.Result = couponDTO;
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

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> CreateCoupon([FromBody] CouponCreateDTO couponCreateDTO)
        {
            try 
            {
                if (couponCreateDTO == null)
                {
                    return BadRequest();
                }
                Coupon couponIsExists = await unitOfWork.couponRepository.Get(tracked: false, filter: x => x.CouponCode.ToLower() == couponCreateDTO.CouponCode.ToLower());
                if (couponIsExists != null)
                {
                    return BadRequest();
                }
                Coupon coupon = mapper.Map<Coupon>(couponCreateDTO);
                await unitOfWork.couponRepository.Create(coupon);
                await unitOfWork.Save();

                apiResponse.IsSuccess = true;
                apiResponse.StatusCode = HttpStatusCode.Created;
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
        [HttpPut("update/{couponId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateCoupon(int? couponId, [FromBody] CouponUpdateDTO couponUpdateDTO)
        {
            try 
            {
                if (couponId == null || couponId == 0)
                {
                    return BadRequest();
                }
                if (couponUpdateDTO == null)
                {
                    return BadRequest();
                }
                Coupon couponIsExists = await unitOfWork.couponRepository.Get(tracked: false, filter: x => x.CouponId == couponId);
                if (couponIsExists == null)
                {
                    return BadRequest();
                }

                couponUpdateDTO.CouponId = (int)couponId;

                Coupon coupon = mapper.Map<Coupon>(couponUpdateDTO);
                unitOfWork.couponRepository.Update(coupon);
                await unitOfWork.Save();
                apiResponse.IsSuccess = true;
                apiResponse.StatusCode = HttpStatusCode.OK;
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
        [HttpDelete("delete/{couponId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteCoupon(int? couponId) 
        {
            try 
            {
                if (couponId == null || couponId == 0)
                {
                    return BadRequest();
                }
                Coupon coupon = await unitOfWork.couponRepository.Get(tracked: false, filter: x => x.CouponId == couponId);
                if (coupon == null)
                {
                    return NotFound();
                }
                unitOfWork.couponRepository.Delete(coupon);
                await unitOfWork.Save();

                apiResponse.IsSuccess = true;
                apiResponse.StatusCode = HttpStatusCode.OK;
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
