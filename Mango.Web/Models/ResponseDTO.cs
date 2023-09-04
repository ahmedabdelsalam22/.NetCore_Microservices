using System.Net;

namespace Mango.Services.CouponAPI.Models
{
    public class ResponseDTO
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public Object? Result { get; set; }
        public string ErrorMessage { get; set; }
    }
}
