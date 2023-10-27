using Mango.Services.EmailAPI.Models;
using Mango.Services.EmailAPI.Models.DTOS;

namespace Mango.Services.EmailAPI.Service
{
    public interface IEmailService
    {
        Task EmailCartAndLog(CartDto cartDto);
        Task RegisterUserEmailAndLog(string email);
        Task LogOrderPlaced(OrderEmailMessage rewardsMessage);
    }
}
