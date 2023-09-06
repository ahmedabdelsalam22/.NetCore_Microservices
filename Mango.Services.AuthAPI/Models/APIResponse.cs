using System.Net;

namespace Mango.Services.AuthAPI.Models
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }

        public object? Result;
    }
}
