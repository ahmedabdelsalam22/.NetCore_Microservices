using System.Net;

namespace Mango.Web.Models.DTOS
{
    public class ResponseDTO
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public object? Result { get; set; }
        public string ErrorMessage { get; set; }
    }
}
