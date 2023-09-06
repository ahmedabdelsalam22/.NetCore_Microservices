using Mango.FrontEnd.Models.DTOS;
using Mango.Web.Models.DTOS;

namespace Mango.Web.Service.IService
{
    public interface IAuthService
    {
        Task<ResponseDTO?> Login(LoginRequestDTO loginRequestDTO);
        Task<ResponseDTO?> Register(RegisterRequestDTO registerRequestDTO);
        Task<ResponseDTO?> AssignRole(RegisterRequestDTO registerRequestDTO);
    }
}
