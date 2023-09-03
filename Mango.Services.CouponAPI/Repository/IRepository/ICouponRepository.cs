using Mango.Services.CouponAPI.Models;

namespace Mango.Services.CouponAPI.Repository.IRepository
{
    public interface ICouponRepository : IRepository<Coupon>
    {
        void Update(Coupon coupon);
    }
}
