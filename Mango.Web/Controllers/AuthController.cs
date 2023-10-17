using Mango.FrontEnd.Models.DTOS;
using Mango.Web.Models.DTOS;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDTO model)
        {
            ResponseDTO? response = await _authService.Login(model);
            if (response.IsSuccess)
            {
                LoginResponseDTO? loginResponseDTO = 
                    JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(response.Result));

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(model);
            }
        }
        [HttpGet]
        public IActionResult Register()
        {
            RegisterRequestDTO registerRequestDTO = new();
            return View(registerRequestDTO);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterRequestDTO model)
        {
            ResponseDTO? result = await _authService.Register(model);
            if (result != null && result.IsSuccess)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        public IActionResult Logout() 
        {
            return View();
        }
    }
}
