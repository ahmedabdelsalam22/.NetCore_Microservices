using System.Net;

namespace Mango.Services.CouponAPI.Models
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public Object? Result { get; set; }
        public List<string>? ErrorMessage { get; set; }
    }
}
