using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Service.IService;
using Mango.Services.AuthAPI.Utility;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Mango.Services.AuthAPI.Service
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtOptions _jwtOptions;

        public JwtTokenGenerator(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public string GenerateToken(ApplicationUser user, IEnumerable<string> roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtOptions.SecretKey);

            List<Claim> claimList = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email,user.Email!),
                new Claim(JwtRegisteredClaimNames.Name,user.Name!),
                new Claim(JwtRegisteredClaimNames.Sub,user.Id!),
            };

            claimList.AddRange(roles.Select(x=> new Claim(ClaimTypes.Role,x))); // x meaning role

            var tokenDescripter = new SecurityTokenDescriptor() 
            {
                Subject = new ClaimsIdentity(claimList),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = _jwtOptions.Audience,
                Issuer = _jwtOptions.Issuer
            };

            var token = tokenHandler.CreateToken(tokenDescripter);

            return tokenHandler.WriteToken(token);
        }
    }
}
