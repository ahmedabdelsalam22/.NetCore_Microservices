using Mango.Services.CouponAPI.Models;
using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface IBaseService
    {
        Task<ResponseDTO?> SendAsync(RequestDTO requestDTO);
    }
}
