using System.Net;

namespace Mango.Services.CouponAPI.Models
{
    public class ResponseDTO
    {
        public bool IsSuccess { get; set; }
        public Object? Result { get; set; }
        public List<string>? ErrorMessage { get; set; }
    }
}
