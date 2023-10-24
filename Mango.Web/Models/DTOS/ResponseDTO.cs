using System.Net;

namespace Mango.Web.Models.Dtos
{
    public class ResponseDTO
    {
        public bool IsSuccess { get; set; }
        public object? Result { get; set; }
        public string Message { get; set; }
    }
}
