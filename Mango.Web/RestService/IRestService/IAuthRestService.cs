using Mango.FrontEnd.Models.DTOS;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Web.RestService.IRestService
{
    public interface IAuthRestService
    {
        Task<LoginResponseDTO> Login(string url, LoginRequestDTO loginRequestDTO);
        Task<UserDTO> Register(string url, RegisterRequestDTO registerRequestDTO);
    }
}
