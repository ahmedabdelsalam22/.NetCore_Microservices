using System.Linq.Expressions;

namespace Mango.Services.CouponAPI.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAll();
        Task<T> Get(Expression<Func<T,bool>>? filter = null);
        Task Create(T entity);
        void Delete(T entity);
    }
}
