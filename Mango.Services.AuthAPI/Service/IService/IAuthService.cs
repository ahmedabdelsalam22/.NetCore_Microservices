using Mango.Services.AuthAPI.Models.DTOS;

namespace Mango.Services.AuthAPI.Service.IService
{
    public interface IAuthService
    {
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<UserDTO> Register(RegisterRequestDTO registerRequestDTO);
        Task<bool> AssignRole(string email,string roleName);
    }
}
