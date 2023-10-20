using System.Net;

namespace Mango.Services.ShoppingCartAPI.Models.DTOS
{
    public class ResponseDTO
    {
        public bool IsSuccess { get; set; }
        public object? Result { get; set; }
        public string Message { get; set; }
    }
}
