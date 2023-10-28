using AutoMapper;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.DTOS;
using Mango.Services.CouponAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/")]
    [Authorize]
    [ApiController]
    public class CouponAPIController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public CouponAPIController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpGet("coupons")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<CouponDTO>>> GetAllCoupons() 
        {
            try
            {
                List<Coupon> coupons = await unitOfWork.couponRepository.GetAll(tracked: false);
                if (coupons == null)
                {
                    return NotFound();
                }
                List<CouponDTO> couponDTOs = mapper.Map<List<CouponDTO>>(coupons);
               
                return Ok(couponDTOs);
            }
            catch (Exception ex) 
            {
                return BadRequest(new List<CouponDTO>());
            }
        }
        [HttpGet("coupon/{couponId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CouponDTO>> GetCouponById(int? couponId) 
        {
            try
            {
                Coupon coupon = await unitOfWork.couponRepository.Get(tracked: false, filter:x=>x.CouponId == couponId);
                if (coupon == null)
                {
                    return NotFound();
                }
                CouponDTO couponDTO = mapper.Map<CouponDTO>(coupon);

                return Ok(couponDTO);
            }
            catch (Exception ex) 
            {
                return BadRequest(new CouponDTO());
            }
        } 
        [HttpGet("coupon/couponCode/{couponCode}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CouponDTO>> GetCouponByCouponCode(string? couponCode) 
        {
            try
            {
                Coupon coupon = await unitOfWork.couponRepository.Get(tracked: false, filter:x=>x.CouponCode.ToLower() == couponCode.ToLower());
                if (coupon == null)
                {
                    return NotFound();
                }
                CouponDTO couponDTO = mapper.Map<CouponDTO>(coupon);

                return Ok(couponDTO);
            }
            catch (Exception ex) 
            {
                return BadRequest(new CouponDTO());
            }
        }

        [HttpPost("coupon/create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CouponDTO>> CreateCoupon([FromBody] CouponCreateDTO couponCreateDTO)
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

                // add coupon in stripe Payment Gateway


                var options = new Stripe.CouponCreateOptions
                {
                    AmountOff = (long) (coupon.DiscountAmount*100),
                    Name = coupon.CouponCode,
                    Currency = "usd",
                    Id = coupon.CouponCode
                };
                var service = new Stripe.CouponService();
                service.Create(options);

                Coupon couponFromDb = await unitOfWork.couponRepository.Get(filter: x => x.CouponCode.ToLower() == couponCreateDTO.CouponCode.ToLower());

                CouponDTO couponDTO = mapper.Map<CouponDTO>(couponFromDb);

                return Ok(couponDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(new CouponDTO());
            }
        }
        [HttpPut("coupon/update/{couponId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CouponDTO>> UpdateCoupon(int? couponId, [FromBody] CouponUpdateDTO couponUpdateDTO)
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

                Coupon couponFromDb = await unitOfWork.couponRepository.Get(filter: x => x.CouponCode.ToLower() == couponUpdateDTO.CouponCode.ToLower());

                CouponDTO couponDTO = mapper.Map<CouponDTO>(couponFromDb);

                return Ok(couponDTO);

            }
            catch (Exception ex)
            {
                return BadRequest(new CouponDTO());
            }
        }
        [HttpDelete("coupon/delete/{couponId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteCoupon(int? couponId) 
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

                var service = new Stripe.CouponService();
                service.Delete(coupon.CouponCode);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
