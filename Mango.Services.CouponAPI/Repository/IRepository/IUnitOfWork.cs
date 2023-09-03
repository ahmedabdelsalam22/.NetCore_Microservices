namespace Mango.Services.CouponAPI.Repository.IRepository
{
    public interface IUnitOfWork
    {
        public ICouponRepository couponRepository { get; }
    }
}
