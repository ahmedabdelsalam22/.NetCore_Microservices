using Mango.Services.EmailAPI.Models.DTOS;

namespace Mango.Services.EmailAPI.Service
{
    public interface IEmailService
    {
        Task EmailCartAndLog(CartDto cartDto);
    }
}
