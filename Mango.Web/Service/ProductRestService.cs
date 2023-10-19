using Mango.Web.Models;
using Mango.Web.RestService.IRestService;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using RestCharpCourse.Services;

namespace Mango.Web.Service
{
    public class ProductRestService : RestService<Product>, IProductRestService
    {
        public ProductRestService(ITokenProvider tokenProvider) : base(tokenProvider)
        {
        }
    }
}
