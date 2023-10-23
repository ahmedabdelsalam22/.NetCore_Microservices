namespace Mango.Services.OrderAPI.Models.Dtos
{
    public class CartDto
    {
        public CartHeaderDto CartHeaderDto { get; set; }
        public IEnumerable<CartDetailsDto>? CartDetailsDtos { get; set; }
    }
}
