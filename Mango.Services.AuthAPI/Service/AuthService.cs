using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.DTOS;
using Mango.Services.AuthAPI.Service.IService;
using Mango.Services.CouponAPI.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace Mango.Services.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthService(ApplicationDbContext dbContext, RoleManager<IdentityRole> roleManager,UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            this._roleManager = roleManager;
            this._userManager = userManager;
        }

        public Task<LoginResponseDTO> Login(LoginRequestDTO model)
        {
            throw new NotImplementedException();
        }

        public async Task<string> Register(RegisterRequestDTO model)
        {
            ApplicationUser user = new()
            {
                UserName = model.Email,
                Name = model.Name,
                Email = model.Email,
                NormalizedEmail = model.Email.ToUpper(),
                PhoneNumber = model.PhoneNumber
            };

            try 
            {
                var result = await _userManager.CreateAsync(user,model.Password);
                if (result.Succeeded)
                {
                    var userFromDB = _dbContext.ApplicationUsers.First(x => x.UserName == model.Email);

                    UserDTO userDTO = new()
                    {
                        ID = userFromDB.Id,
                        Email = userFromDB.Email!,
                        Name = userFromDB.Name,
                        PhoneNumber = userFromDB.PhoneNumber!
                    };
                    return "";
                }
                else {
                    return result!.Errors!.FirstOrDefault()!.Description;
                }


            }catch(Exception ex)
            {
                
            }
            return "Error Encounterd";
        }
    }
}
