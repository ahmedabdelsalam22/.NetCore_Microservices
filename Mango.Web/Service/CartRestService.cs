using Mango.Web.Models.DTOS;
using Mango.Web.Service.IService;
using RestCharpCourse.Services;

namespace Mango.Web.Service
{
    public class CartRestService : RestService<CartDto>, ICartRestService
    {
        public CartRestService(ITokenProvider tokenProvider) : base(tokenProvider)
        {
        }
    }
}
