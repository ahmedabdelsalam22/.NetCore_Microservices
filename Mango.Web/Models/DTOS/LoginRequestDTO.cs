using System.ComponentModel.DataAnnotations;

namespace Mango.FrontEnd.Models.Dtos
{
    public class LoginRequestDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
