using Azure;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.DTOS;
using Mango.Services.AuthAPI.Service.IService;
using Mango.Services.CouponAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Mango.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _service;
        private readonly APIResponse _apiResponse;

        public AuthAPIController(IAuthService service, IConfiguration configuration)
        {
            _service = service;
            _apiResponse = new();
        }
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginRequestDTO loginRequestDTO) 
        {
            LoginResponseDTO loginResponse = await _service.Login(loginRequestDTO);
            if (loginResponse.userDTO == null || string.IsNullOrEmpty(loginResponse.Token)) 
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessage = "username or password is not correct";
                return BadRequest(_apiResponse);
            }

            _apiResponse.IsSuccess = true;
            _apiResponse.Result = loginResponse;
            return Ok(loginResponse);
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDTO>> Register([FromBody] RegisterRequestDTO model)
        {
            if (model.Name.ToLower() == model.UserName.ToLower())
            {
               ModelState.AddModelError("", "username and name are the same!");
            }
            bool ifUserNameUnique = _service.IsUniqueUser(model.UserName);
            if (!ifUserNameUnique)
            {
                ModelState.AddModelError("", "Username already exists");
            }

            var userDTO = await _service.Register(model);

            if (userDTO != null)
            {
                return userDTO;
            }
            else
            {
                return new UserDTO();
            }
        }

        [HttpPost("assignRole")]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult> AssignRole(RegisterRequestDTO model , string roleName) 
        {
            bool roleIsAssigned = await _service.AssignRole(model.Email , roleName!.ToUpper());
            if (!roleIsAssigned) 
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessage = "Error Occured";
                return BadRequest(_apiResponse);
            }
            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }
    }
}
