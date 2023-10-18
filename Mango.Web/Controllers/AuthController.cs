using Mango.FrontEnd.Models.DTOS;
using Mango.Web.Models.DTOS;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Mango.Web.RestService.IRestService;
using Mango.Web.Service.IService;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthRestService _authRestService;
        private readonly ITokenProvider _tokenProvider;
        public AuthController(IAuthRestService authRestService, ITokenProvider tokenProvider)
        {
            _authRestService = authRestService;
            _tokenProvider = tokenProvider;
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
                await SignInUser(response);
                _tokenProvider.SetToken(response.Token);
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
            if (ModelState.IsValid) 
            {
                var response = await _authRestService.Register(url: "/api/auth/register", registerRequestDTO: model);

                if (response.ID != null)
                {
                    return RedirectToAction("Login");
                }
                return View(model);
            }
            return View(model);
        }

        public IActionResult Logout() 
        {
            return View();
        }

        public async Task SignInUser(LoginResponseDTO model) 
        {
            var handler =new JwtSecurityTokenHandler();

            var jwt = handler.ReadJwtToken(model.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            identity.AddClaim(
                new Claim(JwtRegisteredClaimNames.Email, jwt.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value)
                );
            identity.AddClaim(
                new Claim(JwtRegisteredClaimNames.Name, jwt.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Name).Value)
                );
            identity.AddClaim(
                new Claim(JwtRegisteredClaimNames.Sub, jwt.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub).Value)
                );

            identity.AddClaim(
                new Claim(JwtRegisteredClaimNames.Name, jwt.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value)
                );


            var principle = new ClaimsPrincipal(identity);
        }
    }
}
