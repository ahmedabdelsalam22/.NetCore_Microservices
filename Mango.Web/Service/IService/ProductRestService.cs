using Mango.Web.Models;
using Mango.Web.RestService.IRestService;
using RestCharpCourse.Services;

namespace Mango.Web.Service.IService
{
    public class ProductRestService : RestService<Product>, IProductRestService
    {
        public ProductRestService(ITokenProvider tokenProvider) : base(tokenProvider)
        {
        }
    }
}
