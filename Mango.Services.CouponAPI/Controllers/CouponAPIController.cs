using AutoMapper;
using Mango.Services.CouponAPI.Models;
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
        public CouponAPIController(IUnitOfWork unitOfWork,IMapper mapper, APIResponse apiResponse)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.apiResponse = apiResponse;
        }


    }
}
