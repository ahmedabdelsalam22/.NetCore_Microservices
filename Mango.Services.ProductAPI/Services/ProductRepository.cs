using Mango.Services.CouponAPI.Repository.IRepository;
using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Models;

namespace Mango.Services.CouponAPI.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task Update(Product product)
        {
            _context.Update(product);
            await _context.SaveChangesAsync();

        }
    }
}
