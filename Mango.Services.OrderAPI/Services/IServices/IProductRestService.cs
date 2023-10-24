using Mango.Services.OrderAPI.Models.Dtos;

namespace Mango.Services.OrderAPI.Services.IServices
{
    public interface IProductRestService
    {
        Task<List<ProductDto>> GetAllProducts();
    }
}
