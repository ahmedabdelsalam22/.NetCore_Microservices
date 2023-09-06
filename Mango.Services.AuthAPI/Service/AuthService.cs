using AutoMapper;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.DTOS;
using Mango.Services.AuthAPI.Service.IService;
using Mango.Services.CouponAPI.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthService(ApplicationDbContext dbContext, RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,IMapper mapper)
        {
            _dbContext = dbContext;
            this._roleManager = roleManager;
            this._userManager = userManager;
            _mapper = mapper;
        }
      
        public async Task<LoginResponseDTO> Login(LoginRequestDTO model)
        {
            ApplicationUser? user = await _dbContext.ApplicationUsers.FirstOrDefaultAsync(x=>x.UserName!.ToLower() == model.UserName.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(user!, model.Password);

            if (user == null || isValid == false)
            {
                return new LoginResponseDTO()
                {
                    userDTO = null,
                    Token = ""
                };
            }
            // there user is valid and exists in db .. so we will generate token.


            UserDTO userDTO = _mapper.Map<UserDTO>(user);

            return new LoginResponseDTO()
            {
                userDTO = userDTO,
                Token = ""
            };
        }

        public async Task<UserDTO> Register(RegisterRequestDTO model)
        {
            ApplicationUser user = new()
            {
                UserName = model.UserName,
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
                    var userFromDB = _dbContext.ApplicationUsers.First(x => x.UserName!.ToLower() == model.UserName.ToLower());

                    UserDTO userDTO = _mapper.Map<UserDTO>(userFromDB);
                    
                    return userDTO;
                }

            }catch(Exception ex){}

            return new UserDTO();
        }
    }
}
