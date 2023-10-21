using Mango.Services.ShoppingCartAPI.Models.Dtos;

namespace Mango.Services.ShoppingCartAPI.Services.IServices
{
    public interface IProductRestService
    {
        Task<List<ProductDto>> GetAllProducts();
    }
}
