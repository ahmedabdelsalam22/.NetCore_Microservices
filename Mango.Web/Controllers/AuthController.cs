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
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Mango.Web.RestService.IRestService;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthRestService _authRestService;
        public AuthController(IAuthRestService authRestService)
        {
            _authRestService = authRestService;
        }
        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDTO loginRequestDTO = new LoginRequestDTO(); 
            return View(loginRequestDTO);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDTO model)
        {
            var response = await _authRestService.Login(url: "/api/auth/login", loginRequestDTO:model);

            if (response.Token != null) 
            {
                return RedirectToAction("Index", "Home");
            }
            return View(model);
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
            var response = await _authRestService.Register(url: "/api/auth/register", registerRequestDTO: model);

            if (response != null) 
            {
                return RedirectToAction("Login");
            }
            return View(model);
        }

        public IActionResult Logout() 
        {
            return View();
        }
    }
}
