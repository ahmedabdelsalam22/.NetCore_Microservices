using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Repository.IRepository;

namespace Mango.Services.CouponAPI.Repository
{
    public class CouponRepository : Repository<Coupon>, ICouponRepository
    {
        private readonly ApplicationDbContext _context;
        public CouponRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Coupon coupon)
        {
            _context.Update(coupon);
        }
    }
}
