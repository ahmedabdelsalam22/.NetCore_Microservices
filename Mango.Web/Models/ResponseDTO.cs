using System.Net;

namespace Mango.Web.Models
{
    public class ResponseDTO
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public Object? Result { get; set; }
        public string ErrorMessage { get; set; }
    }
}
