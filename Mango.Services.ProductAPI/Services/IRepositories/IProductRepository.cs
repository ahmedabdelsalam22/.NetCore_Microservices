using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Services.IRepositories;

namespace Mango.Services.CouponAPI.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        Task Update(Product product);
    }
}
