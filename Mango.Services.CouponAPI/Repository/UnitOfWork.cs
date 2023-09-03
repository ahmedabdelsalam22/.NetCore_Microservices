using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Repository.IRepository;

namespace Mango.Services.CouponAPI.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            couponRepository = new CouponRepository(context);
        }
        public ICouponRepository couponRepository { get; private set; }
    }
}
