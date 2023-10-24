using Mango.Web.Models.Dtos;
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
