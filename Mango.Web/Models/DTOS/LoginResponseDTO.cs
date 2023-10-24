namespace Mango.FrontEnd.Models.Dtos
{
    public class LoginResponseDTO
    {
        public UserDTO? userDTO { get; set; }
        public string Token { get; set; }
    }
}
