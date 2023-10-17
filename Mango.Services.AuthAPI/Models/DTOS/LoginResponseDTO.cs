namespace Mango.Services.AuthAPI.Models.DTOS
{
    public class LoginResponseDTO
    {
        public UserDTO? userDTO { get; set; }
        public string? Token { get; set; }
    }
}
