using Mango.Web.RestService.IRestService;
using Mango.Web.Service.IService;
using RestCharpCourse.Services;

namespace Mango.Web.RestService
{
    public class CouponRestService : RestService<Coupon>, ICouponRestService
    {
        public CouponRestService(ITokenProvider tokenProvider) : base(tokenProvider)
        {
        }
    }
}
