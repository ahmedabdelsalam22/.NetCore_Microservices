using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Mango.Services.AuthAPI.Models.DTOS
{
    public class RegisterRequestDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string UserName { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [MinLength(8,ErrorMessage ="password must contains 8 characters and numbers at least")]
        [RegularExpression("([a-z]|[A-Z]|[0-9]){4}[a-zA-Z0-9\\W]{3,11}", ErrorMessage = "Invalid password format")]
        public string Password { get; set; }
    }
}
